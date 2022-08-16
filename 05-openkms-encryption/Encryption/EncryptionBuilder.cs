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

    private EncryptionBuilder AddSchemeHelper<TOptions, TContentEncryptionHandler>(string encryptionScheme, string? displayName, Action<TOptions>? configureOptions)
        where TOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : class, IEncryptionHandler
    {
        Services.Configure<EncryptionOptions>(o =>
        {
            o.AddScheme(encryptionScheme, scheme => {
                scheme.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
                scheme.DisplayName = displayName;
            });
        });
        if (configureOptions != null)
        {
            Services.Configure(encryptionScheme, configureOptions);
        }
        Services.AddOptions<TOptions>(encryptionScheme).Validate(o => {
            o.Validate(encryptionScheme);
            return true;
        });
        Services.AddSingleton<TContentEncryptionHandler>();
        return this;
    }

    private EncryptionBuilder AddSchemeHelper<TContentEncryptionOptions, TContentEncryptionHandler, TKeyEncryptionOptions, TKeyEncryptionHandler>(string encryptionScheme, string? displayName, Action<TContentEncryptionOptions>? configureContentEncryptionOptions, Action<TKeyEncryptionOptions>? configureKeyEncryptionOptions)
        where TContentEncryptionOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : class, IEncryptionHandler
        where TKeyEncryptionOptions : EncryptionSchemeOptions, new()
        where TKeyEncryptionHandler : class, IEncryptionHandler
    {
        Services.Configure<EncryptionOptions>(o =>
        {
            o.AddScheme(encryptionScheme, scheme => {
                scheme.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
                scheme.KeyEncryptionHandlerType = typeof(TKeyEncryptionHandler);
                scheme.DisplayName = displayName;
            });
        });
        if (configureContentEncryptionOptions != null)
        {
            Services.Configure(encryptionScheme, configureContentEncryptionOptions);
        }
        Services.AddOptions<TContentEncryptionOptions>(encryptionScheme).Validate(o => {
            o.Validate(encryptionScheme);
            return true;
        });

        if (configureKeyEncryptionOptions != null)
        {
            Services.Configure(encryptionScheme, configureKeyEncryptionOptions);
        }
        Services.AddOptions<TKeyEncryptionOptions>(encryptionScheme).Validate(o => {
            o.Validate(encryptionScheme);
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
    /// <typeparam name="TContentEncryptionOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the content encryptionhandler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <typeparam name="TKeyEncryptionOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the key encryption handler."/>.</typeparam>
    /// <typeparam name="TKeyEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="displayName">The display name of this scheme.</param>
    /// <param name="configureContentEncryptionOptions">Used to configure the content encryption scheme options.</param>
    /// <param name="configureKeyEncryptionOptions">Used to configure the key encryption scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TContentEncryptionOptions, TContentEncryptionHandler, TKeyEncryptionOptions, TKeyEncryptionHandler>(string encryptionScheme, string? displayName,
        Action<TContentEncryptionOptions>? configureContentEncryptionOptions, Action<TKeyEncryptionOptions>? configureKeyEncryptionOptions)
        where TContentEncryptionOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TContentEncryptionOptions>
        where TKeyEncryptionOptions : EncryptionSchemeOptions, new()
        where TKeyEncryptionHandler : EncryptionHandler<TKeyEncryptionOptions>
        => AddSchemeHelper<TContentEncryptionOptions, TContentEncryptionHandler, TKeyEncryptionOptions, TKeyEncryptionHandler>(encryptionScheme, displayName, configureContentEncryptionOptions, configureKeyEncryptionOptions);

    /// <summary>
    /// Adds a <see cref="EncryptionScheme"/> which can be used by <see cref="IEncryptionService"/>.
    /// </summary>
    /// <typeparam name="TContentEncryptionOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TContentEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <typeparam name="TKeyEncryptionOptions">The <see cref="EncryptionSchemeOptions"/> type to configure the handler."/>.</typeparam>
    /// <typeparam name="TKeyEncryptionHandler">The <see cref="EncryptionHandler{TOptions}"/> used to handle this scheme.</typeparam>
    /// <param name="encryptionScheme">The name of this scheme.</param>
    /// <param name="configureContentEncryptionOptions">Used to configure the scheme options.</param>
    /// <param name="configureKeyEncryptionOptions">Used to configure the scheme options.</param>
    /// <returns>The builder.</returns>
    public virtual EncryptionBuilder AddScheme<TContentEncryptionOptions, TContentEncryptionHandler, TKeyEncryptionOptions, TKeyEncryptionHandler>(string encryptionScheme,
        Action<TContentEncryptionOptions>? configureContentEncryptionOptions, Action<TKeyEncryptionOptions>? configureKeyEncryptionOptions)
        where TContentEncryptionOptions : EncryptionSchemeOptions, new()
        where TContentEncryptionHandler : EncryptionHandler<TContentEncryptionOptions>
        where TKeyEncryptionOptions : EncryptionSchemeOptions, new()
        where TKeyEncryptionHandler : EncryptionHandler<TKeyEncryptionOptions>
        => AddScheme<TContentEncryptionOptions, TContentEncryptionHandler, TKeyEncryptionOptions, TKeyEncryptionHandler>(encryptionScheme, displayName: null, configureContentEncryptionOptions: configureContentEncryptionOptions, configureKeyEncryptionOptions: configureKeyEncryptionOptions);
}
