using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Common.Data;

public class DatabaseMigrator
{
    private readonly BankDbContext _dbContext;
    private readonly ILogger<DatabaseMigrator> _logger;

    public DatabaseMigrator(BankDbContext dbContext, ILogger<DatabaseMigrator> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public void Migrate() => MigrateDatabase(_dbContext, 20);

    private void MigrateDatabase(DbContext dbContext, int retriesRemaining)
    {
        try
        {
            dbContext.Database.SetCommandTimeout(TimeSpan.FromSeconds(5));
            dbContext.Database.Migrate();
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
            MigrateDatabase(dbContext, retriesRemaining - 1);
        }
    }
}
