public class DbInitializerService : IHostedService
{
    private readonly IDataAccess _dataAccess;

    public DbInitializerService(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine("StartAsync Prateeeek");
            var tableState = await _dataAccess.CreateTableIfNotExistAsync().ConfigureAwait(false);
            Console.WriteLine("tableState");
            Console.WriteLine(tableState);
            if (tableState == TableState.Created)
            {
                await _dataAccess.SeedDataAsync().ConfigureAwait(false);
                Console.WriteLine("End Prateeeek");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception Prateeeek");
            Console.Error.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            // Consider handling the exception, e.g., by logging or stopping the application.
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Handle cleanup if necessary
        return Task.CompletedTask;
    }
}