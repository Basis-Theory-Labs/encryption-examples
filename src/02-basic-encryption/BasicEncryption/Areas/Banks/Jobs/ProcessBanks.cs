using BasicEncryption.Data;
using BasicEncryption.Data.Entities;
using BasicEncryption.Services;

namespace BasicEncryption.Areas.Banks.Jobs;

public class ProcessBanks
{
    private readonly BankDbContext _dbContext;
    private readonly EncryptionService _encryptionService;

    public ProcessBanks(BankDbContext dbContext, EncryptionService encryptionService)
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
