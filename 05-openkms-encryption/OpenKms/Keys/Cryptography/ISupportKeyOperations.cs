using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Cryptography;

public interface ISupportKeyOperations
{
    IList<KeyOperation> SupportedKeyOperations { get; }
    bool CanDo(KeyOperation operation);
}
