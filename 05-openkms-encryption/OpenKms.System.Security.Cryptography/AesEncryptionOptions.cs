using Encryption;
using Encryption.Options;
using Encryption.Structs;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionOptions : EncryptionHandlerOptions
{
    public override IList<EncryptionAlgorithm> ValidEncryptionAlgorithms { get; } = new[]
    {
        EncryptionAlgorithm.A128GCM,
        EncryptionAlgorithm.A192GCM,
        EncryptionAlgorithm.A256GCM,
        EncryptionAlgorithm.A256CBC_HS512
    };

    public override Dictionary<KeyType, int?[]> ValidKeyTypeSizes { get; } = new()
    {
        { KeyType.OCT, new[] { (int?)null, 128, 192, 256 } }
    };

    public override EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.A256CBC_HS512;

    public override KeyType KeyType { get; set; } = KeyType.OCT;

    public override int? KeySize { get; set; } = 256;
}
