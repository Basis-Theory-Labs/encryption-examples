using System.Collections.Immutable;
using Azure.Security.KeyVault.Keys;
using OpenKms.Keys.Structs;
using JsonWebKey = OpenKms.Keys.Models.JsonWebKey;
using KeyType = OpenKms.Keys.Structs.KeyType;
using KeyOperation = OpenKms.Keys.Structs.KeyOperation;

namespace OpenKms.AzureKeyVault.Extensions;

public static class KeyVaultKeyExtensions
{
    public static JsonWebKey ToJsonWebKey(this KeyVaultKey key)
    {
        var keyType = key.KeyType.ToString() switch
        {
            "EC" => KeyType.EC,
            "EC-HSM" => KeyType.EC,
            "RSA" => KeyType.RSA,
            "RSA-HSM" => KeyType.RSA,
            "oct" => KeyType.OCT,
            "oct-HSM" => KeyType.OCT,
            _ => throw new ArgumentOutOfRangeException(),
        };

        var keyOperations = key.KeyOperations.Select(keyOp =>
        {
            return keyOp.ToString() switch
            {
                "encrypt" => KeyOperation.Encrypt,
                "decrypt" => KeyOperation.Decrypt,
                "sign" => KeyOperation.Sign,
                "verify" => KeyOperation.Verify,
                "wrapKey" => KeyOperation.WrapKey,
                "unwrapKey" => KeyOperation.UnwrapKey,
                _ => throw new ArgumentOutOfRangeException(),
            };
        });

        KeyState keyState;
        if (key.Properties.ExpiresOn.HasValue && key.Properties.ExpiresOn.Value < DateTimeOffset.Now)
            keyState = KeyState.EXPIRED;
        else if (key.Properties.NotBefore.HasValue && key.Properties.ExpiresOn > DateTimeOffset.Now)
            keyState = KeyState.DISABLED;
        else if (key.Properties.Enabled ?? false)
            keyState = KeyState.ENABLED;
        else
            keyState = KeyState.DISABLED;

        return new JsonWebKey
        {
            KeyType = keyType,
            KeyId = key.Id.ToString(),
            KeyOperations = keyOperations.ToImmutableList(),
            State = keyState,
        };
    }
}
