using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Encryption;
using Encryption.Exceptions;
using Microsoft.Extensions.Options;
using OpenKms.AzureKeyVault.Extensions;
using EncryptResult = Encryption.Models.EncryptResult;
using JsonWebKey = Encryption.Models.JsonWebKey;
using KeyType = Encryption.Structs.KeyType;

namespace OpenKms.AzureKeyVault;

public class AzureKeyVaultEncryptionHandler<TKeyNameProvider> :
    EncryptionHandler<AzureKeyVaultEncryptionOptions, TKeyNameProvider>, IEncryptionHandler
    where TKeyNameProvider : IKeyNameProvider
{
    private readonly KeyClient _keyClient;

    public AzureKeyVaultEncryptionHandler(KeyClient keyClient, IOptionsMonitor<AzureKeyVaultEncryptionOptions> options,
        TKeyNameProvider keyNameProvider)
        : base(options, keyNameProvider) => _keyClient = keyClient;

    public override async Task<EncryptResult> EncryptAsync(byte[] plaintext,
        CancellationToken cancellationToken = default)
    {
        var key = await GetOrCreateKey(KeyNameProvider.GetKeyName(), cancellationToken);

        var cryptoClient = new CryptographyClient(key.Key);
        var encryptResult = await cryptoClient.EncryptAsync(Options.EncryptionAlgorithm.ToString(), plaintext,
                cancellationToken);

        return new EncryptResult(encryptResult.Ciphertext, Options.EncryptionAlgorithm.ToString(), key.ToJsonWebKey());
    }

    public override async Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, byte[]? iv = null,
        CancellationToken cancellationToken = default)
    {
        var (keyName, keyVersion) = ParseKeyId(key.KeyId);
        var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);

        var decryptResult = await cryptoClient.DecryptAsync(key.Algorithm.ToString(), ciphertext, cancellationToken);

        return decryptResult.Plaintext;
    }

    private async Task<KeyVaultKey> GetOrCreateKey(string keyName, CancellationToken cancellationToken = default)
    {
        KeyVaultKey key;
        try
        {
            var keyResponse = await _keyClient.GetKeyAsync(keyName, cancellationToken: cancellationToken);

            key = keyResponse.Value;
        }
        catch (RequestFailedException)
        {
            var createKeyTask = Options.KeyType.ToString() switch
            {
                KeyType.EcValue => CreateEcKey(keyName, cancellationToken),
                KeyType.RsaValue => CreateRsaKey(keyName, Options.KeySize, cancellationToken),
                KeyType.OctValue => CreateOctKey(keyName, Options.KeySize, cancellationToken),
                _ => throw new ArgumentOutOfRangeException()
            };

            var createKeyResponse = await createKeyTask;

            key = createKeyResponse.Value;
        }

        return key;
    }

    private Task<Response<KeyVaultKey>> CreateRsaKey(string keyName, int? keySize, CancellationToken cancellationToken = default)
     {
         var createKeyOptions = new CreateRsaKeyOptions(keyName)
         {
             KeySize = keySize,
         };

         _setSharedCreateKeyOptions(createKeyOptions, Options);

         return _keyClient.CreateRsaKeyAsync(createKeyOptions, cancellationToken);
     }

    private Task<Response<KeyVaultKey>> CreateEcKey(string keyName, CancellationToken cancellationToken = default)
    {
        var createKeyOptions = new CreateEcKeyOptions(keyName)
        {
            CurveName = Options.EcCurveName
        };

        _setSharedCreateKeyOptions(createKeyOptions, Options);

        return _keyClient.CreateEcKeyAsync(createKeyOptions, cancellationToken);
    }

    private Task<Response<KeyVaultKey>> CreateOctKey(string keyName, int? keySize, CancellationToken cancellationToken = default)
    {
        var createKeyOptions = new CreateOctKeyOptions(keyName)
        {
            KeySize = keySize,
        };

        _setSharedCreateKeyOptions(createKeyOptions, Options);

        return _keyClient.CreateOctKeyAsync(createKeyOptions, cancellationToken);
    }

    private readonly Action<CreateKeyOptions, AzureKeyVaultEncryptionOptions> _setSharedCreateKeyOptions = (createKeyOptions, handlerOptions) =>
    {
        createKeyOptions.ExpiresOn = handlerOptions.KeyRotationInterval.HasValue
            ? DateTimeOffset.Now.Add(handlerOptions.KeyRotationInterval.Value)
            : null;
        foreach (var keyOp in handlerOptions.KeyOperations)
        {
            createKeyOptions.KeyOperations.Add(keyOp.ToString());
        }
    };

    private static (string, string?) ParseKeyId(string keyId)
    {
        var splitKeyUri = keyId.Split("/keys/");
        if (splitKeyUri.Length != 2)
            throw new KeyIdNotSupportedException();

        var keyIdParts = splitKeyUri[1].Split("/");

        return keyIdParts.Length switch
        {
            2 => (keyIdParts[0], keyIdParts[1]),
            1 => (keyIdParts[0], null),
            _ => throw new KeyIdNotSupportedException()
        };
    }
}
