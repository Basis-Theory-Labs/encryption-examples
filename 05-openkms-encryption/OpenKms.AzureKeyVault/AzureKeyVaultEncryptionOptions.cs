
using Encryption;
using Encryption.Structs;

namespace OpenKms.AzureKeyVault;

public class AzureKeyVaultEncryptionOptions : EncryptionHandlerOptions
{
    public override int KeySize { get; set; } = 2048;

    public override EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.RSA1_5;

    public override KeyType KeyType { get; set; } = KeyType.RSA;

    public string KeyName { get; set; } = default!;
}
