using OpenKms.Keys.Cryptography.Operations.Models;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography.Operations;

public interface IEncryptDecrypt : ISupportKeyOperations
{
    EncryptResult Encrypt(EncryptRequest request, string keyId, CancellationToken cancellationToken = default);
    EncryptResult Encrypt(EncryptRequest request, JsonWebKey key, CancellationToken cancellationToken = default);

    DecryptResult Decrypt(DecryptRequest request, string keyId, CancellationToken cancellationToken = default);
    DecryptResult Decrypt(DecryptRequest request, JsonWebKey key, CancellationToken cancellationToken = default);
}
