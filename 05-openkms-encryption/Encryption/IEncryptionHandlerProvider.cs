using OpenKms.Keys;

namespace Encryption;

public interface IEncryptionHandlerProvider
{
    Task<IEncryptionHandler> GetContentEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default);
    Task<IEncryptionHandler?> GetKeyEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default);
}
