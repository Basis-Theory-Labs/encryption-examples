using OpenKms.Keys.Cryptography.Structs;

namespace OpenKms.Keys.Cryptography.Models;

public class DecryptRequest
{
    public DecryptRequest(byte[] ciphertext,
        EncryptionAlgorithm? algorithm = null,
        byte[]? iv = null,
        byte[]? authenticationTag = null,
        byte[]? additionalAuthenticatedData = null)
    {
        Ciphertext = ciphertext;
        Algorithm = algorithm;
        Iv = iv;
        AuthenticationTag = authenticationTag;
        AdditionalAuthenticatedData = additionalAuthenticatedData;
    }

    /// <summary>
    /// Gets the ciphertext to decrypt.
    /// </summary>
    public byte[] Ciphertext { get; }

    /// <summary>
    /// Gets or sets the <see cref="EncryptionAlgorithm"/>.
    /// </summary>
    public EncryptionAlgorithm? Algorithm { get; }

    /// <summary>
    /// Gets the initialization vector for decryption.
    /// </summary>
    public byte[]? Iv { get; private set; }

    /// <summary>
    /// Gets the authenticated tag resulting from encryption with a symmetric key using AES.
    /// </summary>
    public byte[]? AuthenticationTag { get; private set; }

    /// <summary>
    /// Gets additional data that is authenticated during decryption but not encrypted.
    /// </summary>
    public byte[]? AdditionalAuthenticatedData { get; private set; }
}
