using Encryption.Models;
using Encryption.Structs;

namespace Encryption;

public interface IEncryptionHandler
{
    /// <summary>
    /// Initialize the encryption handler. The handler should initialize anything it needs from the scheme as part of this method.
    /// </summary>
    /// <param name="scheme">The <see cref="EncryptionScheme"/> scheme.</param>
    Task InitializeAsync(EncryptionScheme scheme);

    Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);
    Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm, CancellationToken cancellationToken = default);

    Task<WrapKeyResult> WrapKeyAsync(JsonWebKey key, string keyName, CancellationToken cancellationToken = default);
}
