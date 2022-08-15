using Encryption.Models;
using Encryption.Structs;

namespace Encryption;

/// <summary>
/// An opinionated abstraction for implementing <see cref="IEncryptionHandler"/>.
/// </summary>
/// <typeparam name="TOptions">The type for the options used to configure the encryption handler.</typeparam>
public abstract class EncryptionHandler<TOptions> : IEncryptionHandler where TOptions : EncryptionSchemeOptions, new()
{
    public abstract Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);

    public abstract Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName,
        CancellationToken cancellationToken = default);

    public abstract Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm,
        CancellationToken cancellationToken = default);

    public abstract Task<WrapKeyResult> WrapKeyAsync(JsonWebKey key, string keyName,
        CancellationToken cancellationToken = default);
}
