using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CustomerPortal.Data;

public class DatabaseMigrator
{
    private readonly BankDbContext _dbContext;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(BankDbContext dbContext, ILogger<DatabaseMigrator> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        await MigrateDatabase(_dbContext, 20, cancellationToken);
    }

    private async Task MigrateDatabase(DbContext dbContext, int retriesRemaining, CancellationToken cancellationToken)
    {
        try
        {
            dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
            _logger.LogInformation("Successfully migrated database!");
        }
        catch (SqlException e)
        {
            _logger.LogWarning(e,
                "Error running migrations on DB. Remaining Retries: {RetriesRemaining}",
                retriesRemaining);
            if (retriesRemaining == 0)
                throw;

            Thread.Sleep(3000);
            await MigrateDatabase(dbContext, retriesRemaining - 1, cancellationToken);
        }
    }
}
