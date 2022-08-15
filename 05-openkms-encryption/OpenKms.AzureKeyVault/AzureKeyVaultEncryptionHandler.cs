using Azure;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Microsoft.Extensions.Options;
using OpenKms.Keys;
using OpenKms.Keys.Exceptions;
using EncryptResult = OpenKms.Keys.Cryptography.Operations.Models.EncryptResult;

namespace OpenKms.AzureKeyVault;

public class AzureKeyVaultEncryptionHandler : IEncryptionHandler
{
    private readonly KeyClient _keyClient;
    private readonly IOptionsMonitor<AzureKeyVaultEncryptionOptions> _options;

    public AzureKeyVaultEncryptionHandler(KeyClient keyClient, IOptionsMonitor<AzureKeyVaultEncryptionOptions> options)
    {
        _keyClient = keyClient;
        _options = options;
    }

    public Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName, CancellationToken cancellationToken = default)
    {
        try
        {
            var keyResponse = await _keyClient.GetKeyAsync(keyName, cancellationToken: cancellationToken);
            var cryptoClient = new CryptographyClient(keyResponse.Value.Key);

            var encryptResult =
                await cryptoClient.EncryptAsync(_options.CurrentValue.DefaultEncryptionAlgorithm.ToString(), plaintext,
                    cancellationToken);

            return new EncryptResult(encryptResult.Ciphertext, encryptResult.Algorithm.ToString(),
                keyResponse.Value.Id.ToString());
        }
        catch (RequestFailedException ex)
        {
            // TODO logging
            throw new KeyNotFoundException();
        }
    }

    public async Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, Keys.Cryptography.Structs.EncryptionAlgorithm algorithm, CancellationToken cancellationToken = default)
    {
        var (keyName, keyVersion) = ParseKeyId(keyId);
        var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);

        var decryptResult = await cryptoClient.DecryptAsync(algorithm.ToString(), ciphertext, cancellationToken);

        return decryptResult.Plaintext;
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
