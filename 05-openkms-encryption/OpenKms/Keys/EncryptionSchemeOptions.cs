using OpenKms.Keys.Cryptography.Structs;

namespace OpenKms.Keys;

public class EncryptionSchemeOptions
{
    public EncryptionAlgorithm DefaultEncryptionAlgorithm { get; set; } = EncryptionAlgorithm.RSA1_5;

    public string DefaultKeyName { get; set; } = ".default";
}
