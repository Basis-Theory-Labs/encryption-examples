using NoEncryption.Data;
using NoEncryption.Data.Entities;

namespace NoEncryption.Jobs;

public class ProcessBanks
{
    private readonly BankDbContext _dbContext;

    public ProcessBanks(BankDbContext dbContext) => _dbContext = dbContext;

    public async Task Run(CancellationToken cancellationToken)
    {
        var banks = _dbContext.Banks.Where(x => x.Status == ProcessStatus.PENDING);

        foreach (var bank in banks)
        {
            if (bank.RoutingNumber != "110000000") continue;

            bank.Status = ProcessStatus.PROCESSED;
        }   

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
