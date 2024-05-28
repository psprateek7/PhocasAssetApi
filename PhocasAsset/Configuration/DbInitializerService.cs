public class DbInitializerService : IHostedService
{
    private readonly IDataAccess _dataAccess;
    private readonly ILogger<DataAccess> _logger;

    public DbInitializerService(IDataAccess dataAccess, ILogger<DataAccess> logger)
    {
        _dataAccess = dataAccess;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Checking Table Status");
            var tableState = await _dataAccess.CreateTableIfNotExistAsync().ConfigureAwait(false);
            if (tableState == TableState.Created)
            {
                await _dataAccess.SeedDataAsync().ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"An error occurred while seeding the database: {ex.Message}");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Handle cleanup if necessary
        return Task.CompletedTask;
    }
}