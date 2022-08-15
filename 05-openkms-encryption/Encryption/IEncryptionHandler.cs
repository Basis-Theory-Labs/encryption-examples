using Encryption.Models;
using Encryption.Structs;

namespace Encryption;

public interface IEncryptionHandler
{
    Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);
    Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm, CancellationToken cancellationToken = default);
}
