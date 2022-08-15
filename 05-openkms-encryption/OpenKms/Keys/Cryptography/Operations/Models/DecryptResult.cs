using OpenKms.Keys.Cryptography.Structs;

namespace OpenKms.Keys.Cryptography.Operations.Models;

public class DecryptResult
{
    public DecryptResult(byte[] plaintext, EncryptionAlgorithm algorithm, string? keyId)
    {
        Plaintext = plaintext;
        Algorithm = algorithm;
        KeyId = keyId;
    }

    /// <summary>
    /// Gets the decrypted data.
    /// </summary>
    public byte[] Plaintext { get; private set; }

    /// <summary>
    /// Gets the <see cref="EncryptionAlgorithm"/> used for the decryption.
    /// </summary>
    public EncryptionAlgorithm Algorithm { get; private set; }

    /// <summary>
    /// Gets the key identifier of the <see cref="KeyVaultKey"/> used to decrypt.
    /// </summary>
    public string? KeyId { get; private set; }
}
