using System.Text;
using Encryption.Extensions;
using Encryption.Models;
using Encryption.Structs;
using Microsoft.Extensions.Options;
using JsonWebKey = Encryption.Models.JsonWebKey;

namespace Encryption;

public interface IEncryptionService
{
    Task<JsonWebEncryption> EncryptAsync(byte[] plaintext, string? scheme, CancellationToken cancellationToken = default);
    Task<JsonWebEncryption> EncryptAsync(string plaintext, string? scheme, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default);
    Task<string> DecryptStringAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default);
}

public class EncryptionService : IEncryptionService
{
    public EncryptionService(IEncryptionSchemeProvider encryptionSchemeProvider,
        IEncryptionHandlerProvider encryptionHandlerProvider)
    {
        Schemes = encryptionSchemeProvider;
        Handlers = encryptionHandlerProvider;
    }

    /// <summary>
    /// Used to lookup EncryptionSchemes.
    /// </summary>
    public IEncryptionSchemeProvider Schemes { get; }

    /// <summary>
    /// Used to resolve IEncryptionHandler instances.
    /// </summary>
    public IEncryptionHandlerProvider Handlers { get; }

    public async Task<JsonWebEncryption> EncryptAsync(byte[] plaintext, string? scheme, CancellationToken cancellationToken = default)
    {
        var encryptionScheme = scheme != null
            ? await Schemes.GetSchemeAsync(scheme)
            : await Schemes.GetDefaultEncryptSchemeAsync();

        if (encryptionScheme == null)
            throw new ArgumentNullException(nameof(encryptionScheme));

        var contentEncryptionHandler = await Handlers.GetContentEncryptionHandlerAsync(encryptionScheme.Name, cancellationToken);
        if (contentEncryptionHandler == null)
            throw new ArgumentNullException(nameof(contentEncryptionHandler));

        var encryptContentResult = await contentEncryptionHandler.EncryptAsync(plaintext, cancellationToken);

        var keyEncryptionHandler = await Handlers.GetKeyEncryptionHandlerAsync(encryptionScheme.Name, cancellationToken);

        EncryptResult? encryptKeyResult = null;
        if (keyEncryptionHandler != null)
            encryptKeyResult = await keyEncryptionHandler.EncryptAsync(encryptContentResult.Key!.GetBytes(), cancellationToken);

        return new JsonWebEncryption
        {
            ProtectedHeader = new JoseHeader
            {
                KeyId = encryptKeyResult?.Key?.KeyId ?? encryptContentResult.Key?.KeyId,
                EncryptionAlgorithm = encryptContentResult.Algorithm,
                Algorithm = encryptKeyResult?.Algorithm,
            },
            EncryptedKey = encryptKeyResult?.Ciphertext != null ? Convert.ToBase64String(encryptKeyResult.Ciphertext) : null,
            Ciphertext = Convert.ToBase64String(encryptContentResult.Ciphertext),
            InitializationVector = encryptContentResult.Iv != null ? Convert.ToBase64String(encryptContentResult.Iv) : null,
        };
    }

    public Task<JsonWebEncryption> EncryptAsync(string plaintext, string? scheme, CancellationToken cancellationToken = default)
    {
        return EncryptAsync(Encoding.UTF8.GetBytes(plaintext), scheme, cancellationToken);
    }

    public async Task<byte[]> DecryptAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default)
    {
        var decryptScheme = await Schemes.GetDefaultEncryptSchemeAsync();

        if (decryptScheme == null)
            throw new ArgumentNullException(nameof(decryptScheme));

        var contentEncryptionHandler = await Handlers.GetContentEncryptionHandlerAsync(decryptScheme.Name, cancellationToken);
        if (contentEncryptionHandler == null)
            throw new ArgumentNullException(nameof(contentEncryptionHandler));

        var keyEncryptionHandler = await Handlers.GetKeyEncryptionHandlerAsync(decryptScheme.Name, cancellationToken);

        if (keyEncryptionHandler == null)
        {
            var key = new JsonWebKey
            {
                Algorithm = encryption.ProtectedHeader!.EncryptionAlgorithm,
                KeyId = encryption.ProtectedHeader.KeyId,
                KeyType = KeyType.OCT
            };

            return await contentEncryptionHandler.DecryptAsync(key, Convert.FromBase64String(encryption.Ciphertext!),
                cancellationToken: cancellationToken);
        }

        var keyEncryptionKey = new JsonWebKey
        {
            Algorithm = encryption.ProtectedHeader!.Algorithm,
            KeyId = encryption.ProtectedHeader.KeyId,
            KeyType = KeyType.OCT
        };

        var cekBytes = await keyEncryptionHandler.DecryptAsync(keyEncryptionKey,
            Convert.FromBase64String(encryption.EncryptedKey!), cancellationToken: cancellationToken);

        var cek = new JsonWebKey
        {
            Algorithm = encryption.ProtectedHeader.EncryptionAlgorithm,
            KeyType = KeyType.OCT,
            K = cekBytes
        };

        return await contentEncryptionHandler.DecryptAsync(cek, Convert.FromBase64String(encryption.Ciphertext!),
            Convert.FromBase64String(encryption.InitializationVector!), cancellationToken);
    }

    public async Task<string> DecryptStringAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default)
    {
        var plaintextBytes = await DecryptAsync(encryption, cancellationToken);

        return Encoding.UTF8.GetString(plaintextBytes);
    }
}
