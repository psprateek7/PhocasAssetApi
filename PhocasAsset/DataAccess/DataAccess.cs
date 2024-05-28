using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Options;
public class DataAccess : IDataAccess
{
    private readonly AwsDynamoOptions _awsDynamoOptions;
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;
    private readonly ILogger<DataAccess> _logger;

    public DataAccess(IOptions<AwsDynamoOptions> _options, ILogger<DataAccess> logger)
    {
        _awsDynamoOptions = _options.Value;

        // Connect to local DynamoDB
        AWSCredentials credentials = new BasicAWSCredentials(_awsDynamoOptions.AccessKey, _awsDynamoOptions.SecretKey);
        AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig
        {
            ServiceURL = _awsDynamoOptions.ServiceUrl
        };

        _client = new AmazonDynamoDBClient(credentials, clientConfig);
        _context = new DynamoDBContext(_client);
        _logger = logger;
    }

    public async Task<TableState> CreateTableIfNotExistAsync()
    {
        Console.WriteLine("CreateTableIfNotExistAsync");
        TableState tableState = TableState.AlreadyExist;
        // Create an AssetTracking table if it doesn't exist
        var tableResponse = await _client.ListTablesAsync();
        if (!tableResponse.TableNames.Contains(Constants.DBConstants.TableName))
        {
            Console.WriteLine("Creating");
            var request = GetTableCreationRequest();
            await _client.CreateTableAsync(request);
            tableState = TableState.Created;
        }
        Console.WriteLine("Created");
        // Wait for table to become available
        bool isTableAvailable;
        do
        {
            var tableStatus = await _client.DescribeTableAsync(Constants.DBConstants.TableName);
            isTableAvailable = tableStatus.Table.TableStatus == "ACTIVE";
            if (!isTableAvailable)
            {
                Thread.Sleep(5000);
            }
        } while (!isTableAvailable);
        Console.WriteLine("return CreateTableIfNotExistAsync");
        return tableState;
    }

    private CreateTableRequest GetTableCreationRequest()
    {
        return new CreateTableRequest
        {
            TableName = Constants.DBConstants.TableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition(Constants.DBConstants.AssetAttribute, ScalarAttributeType.N),
                 new AttributeDefinition(Constants.DBConstants.SortKeyAttribute, ScalarAttributeType.S),
                new AttributeDefinition(Constants.DBConstants.TripIdCreatedAtAttribute, ScalarAttributeType.S),
                new AttributeDefinition(Constants.DBConstants.StatusAttribute, ScalarAttributeType.S)
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement(Constants.DBConstants.AssetAttribute, KeyType.HASH),
                new KeySchemaElement(Constants.DBConstants.SortKeyAttribute, KeyType.RANGE)
            },
            GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
            {
                new GlobalSecondaryIndex
                {
                    IndexName = Constants.DBConstants.TripCreatedAtGsi,
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement(Constants.DBConstants.AssetAttribute, KeyType.HASH),
                        new KeySchemaElement(Constants.DBConstants.TripIdCreatedAtAttribute, KeyType.RANGE)
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                },
                 new GlobalSecondaryIndex
                {
                    IndexName = Constants.DBConstants.LatestGSI,
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement(Constants.DBConstants.StatusAttribute, KeyType.HASH)
                    },
                    Projection = new Projection
                    {
                        ProjectionType = ProjectionType.ALL
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                }

            },

            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 10
            }
        };
    }

    public async Task SeedDataAsync()
    {
        try
        {
            _logger.LogDebug("----------------------------------------------------------------------------");
            _logger.LogDebug("----------------------------------------------------------------------------");
            _logger.LogDebug("------------------------------SEEDING DATA----------------------------------");
            _logger.LogDebug("------This might take a while before application is up and running----------");
            _logger.LogDebug("----------------------------------------------------------------------------");
            _logger.LogDebug("----------------------------------------------------------------------------");

            string filePath = "Assets/data.json";
            var (dataPoints, latestEvents) = await JsonDataReader.ReadJsonFileAsync(_logger, filePath);
            if (dataPoints != null)
            {
                /*Ideally the write operation  await ProcessAllBatchesAsync(dataPoints) could be perfomed in parallely in smaller batches
                  but the local dynamo setup is unable to handle the required insertion rate 
                  and SDK is throwing exception after around 80% data is sent across
                  hence relying on single batch request */

                var batchWrite = _context.CreateBatchWrite<AssetTracking>();
                batchWrite.AddPutItems(dataPoints);

                await batchWrite.ExecuteAsync();

                var latest = latestEvents.Values.ToList();
                if (latest.Count > 0)
                {
                    var updateBatch = _context.CreateBatchWrite<AssetTracking>();
                    updateBatch.AddPutItems(latest);
                    await updateBatch.ExecuteAsync();
                }
                _logger.LogDebug("----------------------Data Seeding Completed--------------");
            }
            else
            {
                _logger.LogWarning("No data points to insert");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error while inserting data in db {ex.Message}");
        }

    }

    #region  Parallel Batch Write
    public async Task ProcessAllBatchesAsync(List<AssetTracking> items)
    {
        const int batchSize = 25;
        var tasks = new List<Task>();

        for (int i = 0; i < items.Count; i += batchSize)
        {
            _logger.LogDebug($"Writing Items from {i} to {i + batchSize}");
            var batch = items.Skip(i).Take(batchSize).ToList();
            tasks.Add(BatchWriteItemsAsync(batch));
            await Task.Delay(TimeSpan.FromMilliseconds(30));
        }

        try
        {
            // Wait for all tasks to complete
            await Task.WhenAll(tasks);
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
            {
                _logger.LogError($"Error from batch operation: {ex.Message}");
            }
        }
    }
    #endregion

    public async Task BatchWriteItemsAsync(List<AssetTracking> itemBatch)
    {
        try
        {
            var batchWrite = _context.CreateBatchWrite<AssetTracking>();
            batchWrite.AddPutItems(itemBatch);
            // Ideal to retry with exponential backoff
            await batchWrite.ExecuteAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<AssetTracking>> GetEventsByAssetAndTimeRange(int asset, string startDateTime, string endDateTime)
    {
        var search = _context.QueryAsync<AssetTracking>(
            asset,
            QueryOperator.Between,
            new[] { startDateTime + "#", endDateTime + "#" + "~" }
        );

        var results = new List<AssetTracking>();
        do
        {
            var nextSet = await search.GetNextSetAsync();
            results.AddRange(nextSet);
        } while (!search.IsDone);

        return results;
    }

    public async Task<(List<AssetTracking>, string)> GetPagedEventsByAssetAndTimeRange(int asset, string startDateTime, string endDateTime, int limit, string nextToken)
    {
        var _table = Table.LoadTable(_client, Constants.DBConstants.TableName);
        var queryConfig = new QueryOperationConfig
        {
            KeyExpression = new Expression
            {
                ExpressionStatement = "Asset = :v_asset AND SortKey BETWEEN :v_start AND :v_end",
                ExpressionAttributeValues = new Dictionary<string, DynamoDBEntry>
                {
                    {":v_asset", new Primitive(asset.ToString(), true)},
                    {":v_start", new Primitive(startDateTime + "#")},
                    {":v_end", new Primitive(endDateTime + "~")}
                }
            },
            PaginationToken = nextToken,
            Limit = limit
        };

        Search search = _table.Query(queryConfig);
        // Get only the next set of items (one page)
        var documents = await search.GetNextSetAsync();
        var nextKey = search.IsDone ? string.Empty : search.PaginationToken;
        var data = documents.Select(_context.FromDocument<AssetTracking>).ToList();
        return (data, nextKey);
    }

    public async Task<AssetTracking> GetEventById(int asset, string sortKey)
    {
        return await _context.LoadAsync<AssetTracking>(asset, sortKey);
    }

    public async Task<List<AssetTracking>> GetEventByAssetAndTrip(int assetId, int tripId)
    {
        var config = new DynamoDBOperationConfig
        {
            IndexName = Constants.DBConstants.TripCreatedAtGsi
        };

        var search = _context.QueryAsync<AssetTracking>(assetId, QueryOperator.BeginsWith, [$"{tripId}#"], config);
        var events = await search.GetRemainingAsync();
        return events;
    }

    public async Task<List<AssetTracking>> GetLatestEvents()
    {
        var config = new DynamoDBOperationConfig
        {
            IndexName = Constants.DBConstants.LatestGSI
        };

        var search = _context.QueryAsync<AssetTracking>(Constants.DBConstants.LatestGsiHashKey, config);
        var events = await search.GetRemainingAsync();
        return events;
    }


}