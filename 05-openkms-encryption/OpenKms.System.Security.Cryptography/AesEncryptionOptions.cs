using Encryption;
using Encryption.Structs;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionOptions : EncryptionSchemeOptions
{
    public override EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.A256CBC_HS512;

    public override IList<KeyOperation> KeyOperations { get; set; } = new List<KeyOperation>
        { KeyOperation.Encrypt, KeyOperation.Decrypt };

    public int? DefaultKeySize { get; set; } = 256;
}
