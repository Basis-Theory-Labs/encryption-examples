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
        IEncryptionHandlerProvider encryptionHandlerProvider,
        IOptions<EncryptionOptions> options)
    {
        Schemes = encryptionSchemeProvider;
        Handlers = encryptionHandlerProvider;
        Options = options.Value;
    }

    /// <summary>
    /// Used to lookup EncryptionSchemes.
    /// </summary>
    public IEncryptionSchemeProvider Schemes { get; }

    /// <summary>
    /// Used to resolve IEncryptionHandler instances.
    /// </summary>
    public IEncryptionHandlerProvider Handlers { get; }

    /// <summary>
    /// The <see cref="EncryptionOptions"/>.
    /// </summary>
    public EncryptionOptions Options { get; }

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

        EncryptResult? wrapKeyResult = null;
        if (keyEncryptionHandler != null)
            wrapKeyResult = await keyEncryptionHandler.EncryptAsync(encryptContentResult.Key!.GetBytes(), cancellationToken);

        return new JsonWebEncryption
        {
            ProtectedHeader = new JoseHeader
            {
                KeyId = wrapKeyResult?.Key?.KeyId ?? encryptContentResult.Key?.KeyId,
                EncryptionAlgorithm = encryptContentResult.Algorithm,
                Algorithm = wrapKeyResult?.Algorithm,
            },
            EncryptedKey = wrapKeyResult?.Ciphertext != null ? Convert.ToBase64String(wrapKeyResult.Ciphertext) : null,
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
            return await contentEncryptionHandler.DecryptAsync(encryption.ProtectedHeader!.KeyId!, Convert.FromBase64String(encryption.Ciphertext!),
            encryption.ProtectedHeader.EncryptionAlgorithm, cancellationToken);

        var cekBytes = await keyEncryptionHandler.DecryptAsync(encryption.ProtectedHeader!.KeyId!,
            Convert.FromBase64String(encryption.EncryptedKey!), encryption.ProtectedHeader.Algorithm!.Value,
            cancellationToken);

        var cek = new JsonWebKey()
        {
            Algorithm = encryption.ProtectedHeader.EncryptionAlgorithm,
            KeyType = KeyType.OCT,
            K = cekBytes
        };

        return await contentEncryptionHandler.DecryptAsync(cek, Convert.FromBase64String(encryption.Ciphertext!),
            encryption.ProtectedHeader.EncryptionAlgorithm, Convert.FromBase64String(encryption.InitializationVector!), cancellationToken);

    }

    public async Task<string> DecryptStringAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default)
    {
        var plaintextBytes = await DecryptAsync(encryption, cancellationToken);

        return Encoding.UTF8.GetString(plaintextBytes);
    }
}
