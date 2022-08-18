using Encryption.Models;
using Encryption.Options;
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
    where TOptions : EncryptionHandlerOptions, new()
    where TKeyNameProvider : IKeyNameProvider
{
    protected TKeyNameProvider KeyNameProvider { get; }
    protected EncryptionHandler(IOptionsMonitor<TOptions> options, TKeyNameProvider keyNameProvider) : base(options)
    {
        KeyNameProvider = keyNameProvider;
    }
}

public interface IEncryptionHandler
{
    /// <summary>
    /// Initialize the encryption handler. The handler should initialize anything it needs from the scheme as part of this method.
    /// </summary>
    /// <param name="scheme">The <see cref="EncryptionScheme"/> scheme.</param>
    Task InitializeAsync(EncryptionScheme scheme);

    Task<EncryptResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, byte[]? iv = null,
        CancellationToken cancellationToken = default);

    bool CanDecrypt(JsonWebKey key);
}

/// <summary>
/// An opinionated abstraction for implementing <see cref="IEncryptionHandler"/>.
/// </summary>
/// <typeparam name="TOptions">The type for the options used to configure the encryption handler.</typeparam>
public abstract class EncryptionHandler<TOptions> : IEncryptionHandler where TOptions : EncryptionHandlerOptions, new()
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

    public abstract Task<byte[]> DecryptAsync(JsonWebKey key, byte[] ciphertext, byte[]? iv = null,
        CancellationToken cancellationToken = default);

    public abstract bool CanDecrypt(JsonWebKey key);

    /// <summary>
    /// Gets the <see cref="IOptionsMonitor{TOptions}"/> to detect changes to options.
    /// </summary>
    protected IOptionsMonitor<TOptions> OptionsMonitor { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="EncryptionHandler{TOptions}"/>.
    /// </summary>
    /// <param name="options">The monitor for the options instance.</param>
    protected EncryptionHandler(IOptionsMonitor<TOptions> options) => OptionsMonitor = options;

    /// <summary>
    /// Initialize the handler, resolve the options and validate them.
    /// </summary>
    /// <returns></returns>
    public async Task InitializeAsync(EncryptionScheme scheme)
    {
        Scheme = scheme ?? throw new ArgumentNullException(nameof(scheme));
        Options = OptionsMonitor.Get(Scheme.Name);

        await InitializeHandlerAsync();
    }

    /// <summary>
    /// Called after options/events have been initialized for the handler to finish initializing itself.
    /// </summary>
    /// <returns>A task</returns>
    protected virtual Task InitializeHandlerAsync() => Task.CompletedTask;
}
