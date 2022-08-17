
using Encryption;
using Encryption.Structs;

namespace OpenKms.AzureKeyVault;

public class AzureKeyVaultEncryptionOptions : EncryptionSchemeOptions
{
    public override int? KeySize { get; set; } = 2048;

    public override IList<KeyOperation> KeyOperations { get; set; } = new List<KeyOperation>
        { KeyOperation.Decrypt, KeyOperation.Encrypt };

    public override KeyType KeyType { get; set; } = KeyType.RSA;
}
