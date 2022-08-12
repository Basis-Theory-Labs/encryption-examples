using OpenKms.Keys.Cryptography.Models;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography;

public interface IDeriveKeyDeriveBits : ISupportKeyOperations
{
    DeriveKeyResult DeriveKey(DeriveKeyRequest request, string keyId, CancellationToken cancellationToken = default);
    DeriveKeyResult DeriveKey(DeriveKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default);

    DeriveBitsRequest DeriveBits(DeriveBitsRequest request, string keyId, CancellationToken cancellationToken = default);
    DeriveBitsRequest DeriveBits(DeriveBitsRequest request, JsonWebKey key, CancellationToken cancellationToken = default);
}
