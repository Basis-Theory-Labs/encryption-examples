using Encryption.Models;
using Encryption.Structs;
using Microsoft.Extensions.Options;

namespace Encryption;

public interface IKeyNameProvider
{
    string GetKeyName();
}

public class GuidKeyNameProvider : IKeyNameProvider
{
    public string GetKeyName() => Guid.NewGuid().ToString();
}

public abstract class EncryptionHandler<TOptions, TKeyNameProvider> : EncryptionHandler<TOptions>
    where TOptions : EncryptionSchemeOptions, new()
    where TKeyNameProvider : IKeyNameProvider
{
    protected TKeyNameProvider KeyNameProvider { get; }
    protected EncryptionHandler(IOptionsMonitor<TOptions> options, TKeyNameProvider keyNameProvider) : base(options)
    {
        KeyNameProvider = keyNameProvider;
    }
}
/// <summary>
/// An opinionated abstraction for implementing <see cref="IEncryptionHandler"/>.
/// </summary>
/// <typeparam name="TOptions">The type for the options used to configure the encryption handler.</typeparam>
public abstract class EncryptionHandler<TOptions> : IEncryptionHandler where TOptions : EncryptionSchemeOptions, new()
{
    /// <summary>
    /// Gets or sets the <see cref="EncryptionScheme"/> associated with this encryption handler.
    /// </summary>
    public EncryptionScheme Scheme { get; private set; } = default!;

    /// <summary>
    /// Gets or sets the options associated with this encryption handler.
    /// </summary>
    public TOptions Options { get; private set; } = default!;

    public abstract Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);

    public abstract Task<EncryptResult> EncryptAsync(byte[] plaintext, string keyName,
        CancellationToken cancellationToken = default);

    public abstract Task<byte[]> DecryptAsync(string keyId, byte[] ciphertext, EncryptionAlgorithm algorithm,
        CancellationToken cancellationToken = default);

    public abstract Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, EncryptionAlgorithm algorithm,
        byte[]? iv = null, CancellationToken cancellationToken = default);

    public abstract Task<WrapKeyResult> WrapKeyAsync(JsonWebKey key, string keyName,
        CancellationToken cancellationToken = default);

    public abstract Task<byte[]> UnwrapKeyAsync(string keyId, byte[] encryptedKey, EncryptionAlgorithm algorithm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the <see cref="IOptionsMonitor{TOptions}"/> to detect changes to options.
    /// </summary>
    protected IOptionsMonitor<TOptions> OptionsMonitor { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EncryptionHandler{TOptions}"/>.
    /// </summary>
    /// <param name="options">The monitor for the options instance.</param>
    protected EncryptionHandler(IOptionsMonitor<TOptions> options)
    {
        OptionsMonitor = options;
    }

    /// <summary>
    /// Initialize the handler, resolve the options and validate them.
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync(EncryptionScheme scheme)
    {
        if (scheme == null)
        {
            throw new ArgumentNullException(nameof(scheme));
        }

        Scheme = scheme;
        Options = OptionsMonitor.Get(Scheme.Name);

        await InitializeHandlerAsync();
    }

    /// <summary>
    /// Called after options/events have been initialized for the handler to finish initializing itself.
    /// </summary>
    /// <returns>A task</returns>
    protected virtual Task InitializeHandlerAsync() => Task.CompletedTask;
}
