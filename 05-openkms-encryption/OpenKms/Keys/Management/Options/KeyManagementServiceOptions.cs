using OpenKms.Keys.Cryptography.Structs;
using OpenKms.Keys.Management.Models;
using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Management.Options;

public abstract class KeyManagementServiceOptions
{
    public GenerateKeyRequest? DefaultGenerateKeyRequest { get; set; }

    public virtual Dictionary<KeyType, EncryptionAlgorithm> DefaultEncryptionAlgorithms { get; set; } =
        new Dictionary<KeyType, EncryptionAlgorithm>
        {
            { KeyType.RSA, EncryptionAlgorithm.RSA1_5 },
            { KeyType.OCT, EncryptionAlgorithm.A256GCM },
        };
}
