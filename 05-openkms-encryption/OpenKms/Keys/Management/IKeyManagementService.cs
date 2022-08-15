using OpenKms.Keys.Cryptography;
using OpenKms.Keys.Cryptography.Operations;
using OpenKms.Keys.Management.Models;
using OpenKms.Keys.Models;
using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Management;

public interface IKeyManagementService : IKeyOperator
{
    JsonWebKey GenerateKey(string keyName, CancellationToken cancellationToken = default);
    JsonWebKey GenerateKey(string keyName, GenerateKeyRequest request, CancellationToken cancellationToken = default);

    JsonWebKey GetKey(string keyId, CancellationToken cancellationToken = default);

    JsonWebKey? FindKey(KeyType keyType, CancellationToken cancellationToken = default);
    JsonWebKey? FindKey(string keyName, CancellationToken cancellationToken = default);

    // TODO add interface definitions for key lookup
    // TODO add JsonWebKeySet model?
    // TODO define parameters that can be used to lookup keys
    // IList<JsonWebKey> GetKeys(GetKeysRequest request, CancellationToken cancellationToken = default);

    JsonWebKey RotateKey(string keyId, CancellationToken cancellationToken = default);
    JsonWebKey RotateKey(JsonWebKey key, CancellationToken cancellationToken = default);

    JsonWebKey ImportKey(JsonWebKey source, string? keyName = null, CancellationToken cancellationToken = default);

    JsonWebKey ExportKey(string keyId, CancellationToken cancellationToken = default);
    JsonWebKey ExportKey(JsonWebKey key, CancellationToken cancellationToken = default);

    KeyState EnableKey(string keyId, CancellationToken cancellationToken = default);
    KeyState EnableKey(JsonWebKey key, CancellationToken cancellationToken = default);

    KeyState DisableKey(string keyId, CancellationToken cancellationToken = default);
    KeyState DisableKey(JsonWebKey key, CancellationToken cancellationToken = default);

    bool SupportsKeyType(KeyType keyType, CancellationToken cancellationToken = default);

    bool SupportsKeyId(string keyId, CancellationToken cancellationToken = default);
}
