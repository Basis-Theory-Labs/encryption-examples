using OpenKms.Keys.Cryptography.Structs;
using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Management.Models;

public class GenerateKeyRequest
{
    public KeyType KeyType { get; init; }

    public int? KeySize { get; init; }

    public EncryptionAlgorithm? Algorithm { get; init; }
}
