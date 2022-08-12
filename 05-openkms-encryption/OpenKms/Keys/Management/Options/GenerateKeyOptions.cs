using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Management.Options;

public class GenerateKeyOptions
{
    public KeyType KeyType { get; init; }
    public int? KeySize { get; set; }
}
