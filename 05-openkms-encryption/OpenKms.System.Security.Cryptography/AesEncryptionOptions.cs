using Encryption;
using Encryption.Structs;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionOptions : EncryptionSchemeOptions
{
    public override EncryptionAlgorithm DefaultEncryptionAlgorithm { get; set; } = EncryptionAlgorithm.A256CBC_HS512;

    public int? DefaultKeySize { get; set; } = 256;
}
