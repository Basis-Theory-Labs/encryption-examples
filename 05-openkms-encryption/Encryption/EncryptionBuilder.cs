using Encryption.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Encryption;

public class EncryptionBuilder
{
    public EncryptionBuilder(IServiceCollection services)
        => Services = services;

    /// <summary>
    /// The services being configured.
    /// </summary>
    public virtual IServiceCollection Services { get; }

    private EncryptionBuilder AddSchemeHelper<TOptions, TContentEncryptionHandler>(string authenticationScheme, string? displayName, Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : class, IEncryptionHandler
    {
        Services.Configure<EncryptionOptions>(o =>
        {
            o.AddScheme(authenticationScheme, scheme => {
                scheme.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
                scheme.DisplayName = displayName;
            });
        });
        if (configureOptions != null)
        {
            Services.Configure(authenticationScheme, configureOptions);
        }
        Services.AddOptions<TOptions>(authenticationScheme).Validate(o => {
            o.Validate(authenticationScheme);
            return true;
        });
        Services.AddSingleton<TContentEncryptionHandler>();
        return this;
    }

    private EncryptionBuilder AddSchemeHelper<TOptions, TContentEncryptionHandler, TKeyEncryptionHandler>(string authenticationScheme, string? displayName, Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : class, IEncryptionHandler
        where TKeyEncryptionHandler : class, IEncryptionHandler
    {
        Services.Configure<EncryptionOptions>(o =>
        {
            o.AddScheme(authenticationScheme, scheme => {
                scheme.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
                scheme.KeyEncryptionHandlerType = typeof(TKeyEncryptionHandler);
                scheme.DisplayName = displayName;
            });
        });
        if (configureOptions != null)
        {
            Services.Configure(authenticationScheme, configureOptions);
        }
        Services.AddOptions<TOptions>(authenticationScheme).Validate(o => {
            o.Validate(authenticationScheme);
            return true;
        });
        Services.AddSingleton<TContentEncryptionHandler>();
        Services.AddSingleton<TKeyEncryptionHandler>();
        return this;
    }

    /// <summary>
    /// Adds a <see cref="EncryptionScheme"/> which can be used by <see cref="IEncryptionService"/>.
    /// </summary>
    /// <typeparam name="TOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="displayName">The display name of this scheme.</param>
    /// <param name="configureOptions">Used to configure the scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TOptions, TContentEncryptionHandler>(string encryptionScheme, string? displayName,
        Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TOptions>
        => AddSchemeHelper<TOptions, TContentEncryptionHandler>(encryptionScheme, displayName, configureOptions);

    /// <summary>
    /// Adds a <see cref="EncryptionScheme"/> which can be used by <see cref="IEncryptionService"/>.
    /// </summary>
    /// <typeparam name="TOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="configureOptions">Used to configure the scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TOptions, TContentEncryptionHandler>(string encryptionScheme,
        Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TOptions>
        => AddScheme<TOptions, TContentEncryptionHandler>(encryptionScheme, displayName: null, configureOptions: configureOptions);

    /// <summary>
    /// Adds a <see cref="EncryptionScheme"/> which can be used by <see cref="IEncryptionService"/>.
    /// </summary>
    /// <typeparam name="TOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <typeparam name="TKeyEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="displayName">The display name of this scheme.</param>
    /// <param name="configureOptions">Used to configure the scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TOptions, TContentEncryptionHandler, TKeyEncryptionHandler>(string encryptionScheme, string? displayName,
        Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TOptions>
        where TKeyEncryptionHandler : EncryptionHandler<TOptions>
        => AddSchemeHelper<TOptions, TContentEncryptionHandler, TKeyEncryptionHandler>(encryptionScheme, displayName, configureOptions);

    /// <summary>
    /// Adds a <see cref="EncryptionScheme"/> which can be used by <see cref="IEncryptionService"/>.
    /// </summary>
    /// <typeparam name="TOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <typeparam name="TKeyEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="configureOptions">Used to configure the scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TOptions, TContentEncryptionHandler, TKeyEncryptionHandler>(string encryptionScheme,
        Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TOptions>
        where TKeyEncryptionHandler : EncryptionHandler<TOptions>
        => AddScheme<TOptions, TContentEncryptionHandler, TKeyEncryptionHandler>(encryptionScheme, displayName: null, configureOptions: configureOptions);
}
