using OpenKms.Keys.Cryptography.Structs;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography.Operations.Models;

public class EncryptResult
{
    public EncryptResult(byte[] ciphertext,
        EncryptionAlgorithm algorithm,
        string? keyId = null,
        byte[]? iv = null,
        byte[]? authenticationTag = null,
        byte[]? additionalAuthenticatedData = null)
    {
        Ciphertext = ciphertext;
        Algorithm = algorithm;
        KeyId = keyId;
        Iv = iv;
        AuthenticationTag = authenticationTag;
        AdditionalAuthenticatedData = additionalAuthenticatedData;
    }

    /// <summary>
    /// Gets the ciphertext that is the result of the encryption.
    /// </summary>
    public byte[] Ciphertext { get; internal set; }

    /// <summary>
    /// Gets the <see cref="EncryptionAlgorithm"/> used for encryption. This must be stored alongside the <see cref="Ciphertext"/> as the same algorithm must be used to decrypt it.
    /// </summary>
    public EncryptionAlgorithm Algorithm { get; internal set; }

    /// <summary>
    /// Gets the key identifier of the <see cref="JsonWebKey"/> used to encrypt. This must be stored alongside the <see cref="Ciphertext"/> as the same key must be used to decrypt it.
    /// </summary>
    public string? KeyId { get; internal set; }

    /// <summary>
    /// Gets the initialization vector for encryption.
    /// </summary>
    public byte[]? Iv { get; internal set; }

    /// <summary>
    /// Gets the authentication tag resulting from encryption with a symmetric key including <see cref="EncryptionAlgorithm.A128Gcm"/>, <see cref="EncryptionAlgorithm.A192Gcm"/>, or <see cref="EncryptionAlgorithm.A256Gcm"/>.
    /// </summary>
    public byte[]? AuthenticationTag { get; internal set; }

    /// <summary>
    /// Gets additional data that is authenticated during decryption but not encrypted.
    /// </summary>
    public byte[]? AdditionalAuthenticatedData { get; internal set; }
}
