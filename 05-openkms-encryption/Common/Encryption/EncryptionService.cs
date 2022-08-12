using System.Text;
using Azure;
using LazyCache;
using Microsoft.Extensions.Configuration;
using OpenKms.Keys.Cryptography.Models;
using OpenKms.Keys.Management;
using JsonWebKey = OpenKms.Keys.Models.JsonWebKey;

namespace Common.Encryption;

public class EncryptionService
{
    private readonly IKeyManagementService _kms;
    private readonly IAppCache _cache;
    private readonly string _keyName;

    public EncryptionService(IKeyManagementService kms, IAppCache cache, IConfiguration configuration)
    {
        _kms = kms;
        _cache = cache;
        _keyName = configuration.GetValue<string>("Encryption:KeyName");
    }

    public async Task<string> Encrypt(string plaintext, CancellationToken cancellationToken = default)
    {
        var key = await GetOrCreateKey(cancellationToken);

        var encryptResult = _kms.Encrypt(new EncryptRequest(Encoding.UTF8.GetBytes(plaintext)), key, cancellationToken);

        return Convert.ToBase64String(encryptResult.Ciphertext);
    }

    public async Task<string> Decrypt(string ciphertext, CancellationToken cancellationToken = default)
    {
        var key = await GetOrCreateKey(cancellationToken);

        var decryptResult = _kms.Decrypt(new DecryptRequest(Convert.FromBase64String(ciphertext)), key, cancellationToken);

        return Encoding.UTF8.GetString(decryptResult.Plaintext);
    }

    private async Task<JsonWebKey> GetOrCreateKey(CancellationToken cancellationToken = default) =>
        await _cache.GetOrAddAsync(_keyName, () =>
        {
            try
            {
                var existing = _kms.FindKey(_keyName, cancellationToken);
                if (existing != null)
                    return Task.FromResult(existing);
            }
            catch (RequestFailedException ex)
            {
                // TODO logging
            }

            var key = _kms.GenerateKey(_keyName);
            return Task.FromResult(key);
        });
}
