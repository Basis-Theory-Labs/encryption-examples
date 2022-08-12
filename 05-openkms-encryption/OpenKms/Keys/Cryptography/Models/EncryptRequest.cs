using OpenKms.Keys.Cryptography.Structs;

namespace OpenKms.Keys.Cryptography.Models;

public class EncryptRequest
{
    public EncryptRequest(byte[] plaintext, EncryptionAlgorithm? algorithm = null, byte[]? iv = null, byte[]? additionalAuthenticatedData = null)
    {
        Plaintext = plaintext;
        Algorithm = algorithm;
        Iv = iv;
        AdditionalAuthenticatedData = additionalAuthenticatedData;
    }

    /// <summary>
    /// Gets the plaintext to encrypt.
    /// </summary>
    public byte[] Plaintext { get; } = default!;

    /// <summary>
    /// Gets the <see cref="EncryptionAlgorithm"/>.
    /// </summary>
    public EncryptionAlgorithm? Algorithm { get; }

    /// <summary>
    /// Gets the initialization vector for encryption.
    /// </summary>
    public byte[]? Iv { get; private set; }

    /// <summary>
    /// Gets additional data that is authenticated during decryption but not encrypted.
    /// </summary>
    public byte[]? AdditionalAuthenticatedData { get; }
}
