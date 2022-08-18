using Encryption.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Encryption;

public class EncryptionSchemeBuilder
{
    public EncryptionSchemeBuilder(string name, IServiceCollection services)
    {
        Name = name;
        Services = services;
    }

    /// <summary>
    /// The services being configured.
    /// </summary>
    public virtual IServiceCollection Services { get; }

    /// <summary>
    /// Gets the name of the scheme being built.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the content encryption <see cref="IEncryptionHandler"/> type responsible for this scheme.
    /// </summary>
    public Type? ContentEncryptionHandlerType { get; set; }

    /// <summary>
    /// Gets or sets the key encryption <see cref="IEncryptionHandler"/> type responsible for this scheme.
    /// </summary>
    public Type? KeyEncryptionHandlerType { get; set; }

    public EncryptionSchemeBuilder AddContentEncryption<THandlerOptions, TEncryptionHandler>(Action<THandlerOptions> configureOptions)
        where THandlerOptions : EncryptionHandlerOptions, new()
        where TEncryptionHandler : class, IEncryptionHandler
    {
        ContentEncryptionHandlerType = typeof(TEncryptionHandler);
        AddHandlerCore<THandlerOptions, TEncryptionHandler>(configureOptions);

        return this;
    }

    public EncryptionSchemeBuilder AddKeyEncryption<THandlerOptions, TEncryptionHandler>(Action<THandlerOptions> configureOptions)
        where THandlerOptions : EncryptionHandlerOptions, new()
        where TEncryptionHandler : class, IEncryptionHandler
    {
        KeyEncryptionHandlerType = typeof(TEncryptionHandler);
        AddHandlerCore<THandlerOptions, TEncryptionHandler>(configureOptions);

        return this;
    }

    private void AddHandlerCore<THandlerOptions, TEncryptionHandler>(Action<THandlerOptions> configureOptions)
        where THandlerOptions : EncryptionHandlerOptions, new()
        where TEncryptionHandler : class, IEncryptionHandler
    {
        Services.AddOptions<THandlerOptions>(Name).Configure(configureOptions);
        Services.TryAddTransient<TEncryptionHandler>();
    }

    /// <summary>
    /// Builds the <see cref="EncryptionScheme"/> instance.
    /// </summary>
    /// <returns>The <see cref="EncryptionScheme"/>.</returns>
    public EncryptionScheme Build()
    {
        if (ContentEncryptionHandlerType is null)
            throw new InvalidOperationException($"{nameof(ContentEncryptionHandlerType)} must be configured to build an {nameof(EncryptionScheme)}.");

        return new EncryptionScheme(Name, ContentEncryptionHandlerType, KeyEncryptionHandlerType);
    }
}
