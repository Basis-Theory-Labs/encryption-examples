using System.Security.Cryptography;
using Encryption;
using Encryption.Models;
using Encryption.Structs;
using Microsoft.Extensions.Options;

namespace OpenKms.System.Security.Cryptography;

public class AesEncryptionHandler : EncryptionHandler<AesEncryptionOptions>, IEncryptionHandler
{
    public AesEncryptionHandler(IOptionsMonitor<AesEncryptionOptions> options) : base(options)
    {
    }

    public override Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        using var aes = Aes.Create();
        aes.KeySize = Options.KeySize;

        aes.GenerateKey();
        aes.GenerateIV();
        var key = aes.Key;
        var iv = aes.IV;

        var ciphertext = aes.EncryptCbc(plaintext, iv);

        return Task.FromResult(new EncryptResult(ciphertext, Options.EncryptionAlgorithm,
            new JsonWebKey
            {
                KeyType = KeyType.OCT,
                K = key,
            }, iv));
    }

    public override Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, byte[]? iv = null,
        CancellationToken cancellationToken = default)
    {
        if (key.K == null)
            throw new ArgumentNullException(nameof(key.K));

        if (iv == null)
            throw new ArgumentNullException(nameof(iv));

        using var aes = Aes.Create();
        aes.Key = key.K!;
        aes.IV = iv;

        return Task.FromResult(aes.DecryptCbc(ciphertext, iv));
    }
}
