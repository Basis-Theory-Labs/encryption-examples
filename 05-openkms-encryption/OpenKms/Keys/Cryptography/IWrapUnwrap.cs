using OpenKms.Keys.Cryptography.Models;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography;

public interface IWrapUnwrap : ISupportKeyOperations
{
    WrapKeyResult WrapKey(WrapKeyRequest request, string keyId, CancellationToken cancellationToken = default);
    WrapKeyResult WrapKey(WrapKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default);

    UnwrapKeyRequest UnwrapKey(UnwrapKeyRequest request, string keyId, CancellationToken cancellationToken = default);
    UnwrapKeyRequest UnwrapKey(UnwrapKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default);
}
