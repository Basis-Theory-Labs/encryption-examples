using Encryption.Structs;

namespace Encryption;

public class EncryptionSchemeOptions
{
    public EncryptionAlgorithm DefaultEncryptionAlgorithm { get; set; } = EncryptionAlgorithm.RSA1_5;

    public string DefaultKeyName { get; set; } = ".default";
}
