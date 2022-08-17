using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Encryption;
using Encryption.Exceptions;
using Encryption.Extensions;
using Encryption.Models;
using Microsoft.Extensions.Options;
using OpenKms.AzureKeyVault.Extensions;
using EncryptionAlgorithm = Encryption.Structs.EncryptionAlgorithm;
using EncryptResult = Encryption.Models.EncryptResult;
using JsonWebKey = Encryption.Models.JsonWebKey;

namespace OpenKms.AzureKeyVault;

public class AzureKeyVaultEncryptionHandler<TKeyNameProvider> : EncryptionHandler<AzureKeyVaultEncryptionOptions, TKeyNameProvider>, IEncryptionHandler where TKeyNameProvider : IKeyNameProvider
{
    private readonly KeyClient _keyClient;

    public AzureKeyVaultEncryptionHandler(KeyClient keyClient, IOptionsMonitor<AzureKeyVaultEncryptionOptions> options, TKeyNameProvider keyNameProvider)
        : base(options, keyNameProvider)
    {
        _keyClient = keyClient;
    }

    public override Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        return EncryptAsync(plaintext, KeyNameProvider.GetKeyName(), cancellationToken);
    }

    public override async Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName,
        CancellationToken cancellationToken = default)
    {
        var key = await GetOrCreateKey(keyName, cancellationToken);

        var cryptoClient = new CryptographyClient(key.Key);
        var encryptResult =
            await cryptoClient.EncryptAsync(Options.DefaultEncryptionAlgorithm.ToString(), plaintext,
                cancellationToken);

        return new EncryptResult(encryptResult.Ciphertext, Options.DefaultEncryptionAlgorithm.ToString(), key.ToJsonWebKey());
    }

    public override async Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext,
        EncryptionAlgorithm algorithm, CancellationToken cancellationToken = default)
    {
        var (keyName, keyVersion) = ParseKeyId(keyId);
        var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);

        var decryptResult = await cryptoClient.DecryptAsync(algorithm.ToString(), ciphertext, cancellationToken);

        return decryptResult.Plaintext;
    }

    public override Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, EncryptionAlgorithm algorithm,
        byte[]? iv = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<WrapKeyResult> WrapKeyAsync(JsonWebKey key, string keyName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var keyVaultKey = await GetOrCreateKey(keyName, cancellationToken);
            var cryptoClient = new CryptographyClient(keyVaultKey.Key);

            var encryptResult =
                await cryptoClient.EncryptAsync(Options.DefaultEncryptionAlgorithm.ToString(),
                    key.GetBytes(),
                    cancellationToken);

            return new WrapKeyResult(encryptResult.Ciphertext, Options.DefaultEncryptionAlgorithm.ToString(),
                keyVaultKey.ToJsonWebKey());
        }
        catch (RequestFailedException ex)
        {
            // TODO logging
            throw new KeyNotFoundException();
        }
    }

    public override async Task<byte[]> UnwrapKeyAsync(string keyId, byte[] encryptedKey, EncryptionAlgorithm algorithm,
        CancellationToken cancellationToken = default)
    {
        var (keyName, keyVersion) = ParseKeyId(keyId);
        var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);

        var decryptResult = await cryptoClient.DecryptAsync(algorithm.ToString(), encryptedKey, cancellationToken);

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
        catch (RequestFailedException ex)
        {
            // TODO logging
            // TODO add options for default keytype, key size, key ops, etc
            var createKeyResponse =
                await _keyClient.CreateRsaKeyAsync(new CreateRsaKeyOptions(keyName, false)
                    {
                        KeySize = 2048,
                        KeyOperations = { KeyOperation.Decrypt, KeyOperation.Encrypt }
                    },
                    cancellationToken);

            key = createKeyResponse.Value;
        }

        return key;
    }

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
