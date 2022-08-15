using OpenKms.Keys.Cryptography.Operations.Models;
using OpenKms.Keys.Cryptography.Structs;

namespace OpenKms.Keys;

public interface IEncryptionHandler
{
    Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);
    Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName, CancellationToken cancellationToken = default);


    Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm, CancellationToken cancellationToken = default);
}
