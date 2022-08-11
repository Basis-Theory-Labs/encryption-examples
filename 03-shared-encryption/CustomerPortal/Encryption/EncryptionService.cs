using System.Security.Cryptography;
using System.Text;
using Azure;
using Azure.Core;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using LazyCache;

namespace CustomerPortal.Encryption;

public class EncryptionService
{
    private readonly KeyClient _keyClient;
    private readonly IAppCache _cache;
    private readonly TokenCredential _tokenCredential;
    private readonly string _keyName;

    public EncryptionService(KeyClient keyClient, IConfiguration configuration, IAppCache cache,
        TokenCredential tokenCredential)
    {
        _keyClient = keyClient;
        _cache = cache;
        _tokenCredential = tokenCredential;
        _keyName = configuration.GetValue<string>("Encryption:KeyName");
    }

    public async Task<string> Encrypt(string plaintext)
    {
        var key = await GetOrCreateKey();
        using var rsaKey = RSA.Create(new RSAParameters
        {
            Exponent = key.Key.E,
            Modulus = key.Key.N
        });

        var cipherText = rsaKey.Encrypt(Encoding.UTF8.GetBytes(plaintext), RSAEncryptionPadding.Pkcs1);

        return Convert.ToBase64String(cipherText);
    }

    public async Task<string> Decrypt(string ciphertext)
    {
        var key = await GetOrCreateKey();

        var cryptoClient = new CryptographyClient(key.Id, _tokenCredential);

        var decryptResult = await cryptoClient.DecryptAsync(EncryptionAlgorithm.Rsa15,
            Convert.FromBase64String(ciphertext));

        return Encoding.UTF8.GetString(decryptResult.Plaintext);
    }

    private async Task<KeyVaultKey> GetOrCreateKey() =>
        await _cache.GetOrAddAsync(_keyName, async () =>
        {
            try
            {
                var existing = await _keyClient.GetKeyAsync(_keyName);
                if (existing?.Value != null) return existing.Value;

                return await CreateKey();
            }
            catch (RequestFailedException)
            {
                return await CreateKey();
            }
        });

    private async Task<KeyVaultKey> CreateKey()
    {
        var key = await _keyClient.CreateRsaKeyAsync(new CreateRsaKeyOptions(_keyName)
        {
            Enabled = true,
            KeyOperations = { KeyOperation.Decrypt, KeyOperation.Encrypt },
            KeySize = 2048
        });
        return key.Value;
    }
}
