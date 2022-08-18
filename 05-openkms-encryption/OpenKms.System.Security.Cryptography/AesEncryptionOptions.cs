using Encryption;
using Encryption.Structs;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionOptions : EncryptionHandlerOptions
{
    public override EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.A256CBC_HS512;

    public override KeyType KeyType { get; set; } = KeyType.OCT;

    public override int KeySize { get; set; } = 256;
}
