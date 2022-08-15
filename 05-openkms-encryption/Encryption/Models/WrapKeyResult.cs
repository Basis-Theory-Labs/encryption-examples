using Encryption.Structs;

namespace Encryption.Models;

public class WrapKeyResult : EncryptResult
{
    public WrapKeyResult(byte[] ciphertext,
        EncryptionAlgorithm algorithm,
        JsonWebKey? key = null,
        byte[]? iv = null,
        byte[]? authenticationTag = null,
        byte[]? additionalAuthenticatedData = null) : base(ciphertext, algorithm, key, iv, authenticationTag, additionalAuthenticatedData)
    {
    }
}
