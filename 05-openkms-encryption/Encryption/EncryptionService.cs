using Encryption.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenKms.Keys;

namespace Encryption;

public interface IEncryptionService
{
    Task<JsonWebEncryption> EncryptAsync(byte[] ciphertext, string? scheme, CancellationToken cancellationToken = default);
    Task<JsonWebEncryption> EncryptAsync(string ciphertext, string? scheme, CancellationToken cancellationToken = default);

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

    public async Task<JsonWebEncryption> EncryptAsync(byte[] ciphertext, string? scheme, CancellationToken cancellationToken = default)
    {
        var encryptionScheme = scheme != null
            ? await Schemes.GetSchemeAsync(scheme)
            : await Schemes.GetDefaultEncryptionSchemeAsync();

        if (encryptionScheme == null)
            throw new ArgumentNullException(nameof(encryptionScheme));

        var contentEncryptionHandler = await Handlers.GetContentEncryptionHandlerAsync(encryptionScheme.Name, cancellationToken);
        if (contentEncryptionHandler == null)
            throw new ArgumentNullException(nameof(contentEncryptionHandler));

        var encryptContentResult = await contentEncryptionHandler.EncryptAsync(ciphertext, cancellationToken);

        var keyEncryptionHandler = await Handlers.GetKeyEncryptionHandlerAsync(encryptionScheme.Name, cancellationToken);

        if (keyEncryptionHandler != null)
            await keyEncryptionHandler.EncryptAsync(encryptContentResult, cancellationToken);

        return new JsonWebEncryption
        {
            UnprotectedHeader = new JoseHeader
            {
                KeyId = encryptContentResult.KeyId,
                EncryptionAlgorithm = encryptContentResult.Algorithm,
            },
            Ciphertext = Base64UrlEncoder.Encode(encryptContentResult.Ciphertext)
        };
    }

    public Task<JsonWebEncryption> EncryptAsync(string ciphertext, string? scheme, CancellationToken cancellationToken = default)
    {
        return EncryptAsync(Base64UrlEncoder.DecodeBytes(ciphertext), scheme, cancellationToken);
    }

    public Task<byte[]> DecryptAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> DecryptStringAsync(JsonWebEncryption encryption, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
