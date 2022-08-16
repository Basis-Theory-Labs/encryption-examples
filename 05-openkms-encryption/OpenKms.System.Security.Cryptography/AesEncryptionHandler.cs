using System.Security.Cryptography;
using Encryption;
using Encryption.Models;
using Encryption.Structs;
using Microsoft.Extensions.Options;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionHandler : EncryptionHandler<AesEncryptionOptions>, IEncryptionHandler
{
    private readonly IOptionsMonitor<AesEncryptionOptions> _options;

    public AesEncryptionHandler(IOptionsMonitor<AesEncryptionOptions> options)
    {
        _options = options;
    }

    public override Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        using var aes = Aes.Create();
        if(_options.CurrentValue.DefaultKeySize.HasValue)
            aes.KeySize = _options.CurrentValue.DefaultKeySize.Value;

        aes.GenerateKey();
        aes.GenerateIV();
        var key = aes.Key;
        var iv = aes.IV;

        var ciphertext = aes.EncryptCbc(plaintext, iv);

        return Task.FromResult(new EncryptResult(ciphertext, _options.CurrentValue.DefaultEncryptionAlgorithm,
            new JsonWebKey
            {
                KeyType = KeyType.OCT,
                K = key,
            }, iv));
    }

    public override Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override Task<WrapKeyResult> WrapKeyAsync(JsonWebKey key, string keyName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
