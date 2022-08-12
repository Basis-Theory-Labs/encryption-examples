using OpenKms.Keys.Cryptography.Models;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography;

public interface ISignVerify : ISupportKeyOperations
{
    SignResult Sign(SignRequest request, string keyId, CancellationToken cancellationToken = default);
    SignResult Sign(SignRequest request, JsonWebKey key, CancellationToken cancellationToken = default);

    VerifyRequest Verify(VerifyRequest request, string keyId, CancellationToken cancellationToken = default);
    VerifyRequest Verify(VerifyRequest request, JsonWebKey key, CancellationToken cancellationToken = default);
}
