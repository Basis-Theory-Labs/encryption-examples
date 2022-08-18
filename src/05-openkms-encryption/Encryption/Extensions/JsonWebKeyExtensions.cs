using Encryption.Models;
using Encryption.Structs;

namespace Encryption.Extensions;

public static class JsonWebKeyExtensions
{
    public static byte[] GetBytes(this JsonWebKey key)
    {
        return key.KeyType.ToString() switch
        {
            KeyType.OctValue => key.K!,
            _ => throw new ArgumentOutOfRangeException(nameof(key.KeyType))
        };
    }
}
