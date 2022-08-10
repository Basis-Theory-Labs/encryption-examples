using Common.Data;
using Common.Data.Entities;
using OpenKMS.Abstractions;

namespace BankProcessor.Jobs;

public class ProcessBanks
{
    private readonly BankDbContext _dbContext;
    private readonly IEncryptionService _encryptionService;

    public ProcessBanks(BankDbContext dbContext, IEncryptionService encryptionService)
    {
        _dbContext = dbContext;
        _encryptionService = encryptionService;
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        var banks = _dbContext.Banks.Where(x => x.Status == ProcessStatus.PENDING);

        foreach (var bank in banks)
            bank.Status = ProcessStatus.PROCESSED;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
