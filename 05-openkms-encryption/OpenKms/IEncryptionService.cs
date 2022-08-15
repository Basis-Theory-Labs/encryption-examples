using OpenKms.Keys.Cryptography.Models;

namespace OpenKms;

public interface IEncryptionService
{
    // JsonWebKey GenerateKey(string keyName, string? scheme, CancellationToken? cancellationToken = default);

    Task<JsonWebEncryption> EncryptAsync(byte[] ciphertext, string? scheme, CancellationToken cancellationToken = default);
    Task<JsonWebEncryption> EncryptAsync(string ciphertext, string? scheme, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default);
    Task<string> DecryptStringAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default);
}
