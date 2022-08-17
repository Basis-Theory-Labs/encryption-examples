using Microsoft.Extensions.DependencyInjection;

namespace Encryption;

public interface IEncryptionHandlerProvider
{
    Task<IEncryptionHandler> GetContentEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default);
    Task<IEncryptionHandler?> GetKeyEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default);
}

public class EncryptionHandlerProvider : IEncryptionHandlerProvider
{
    private readonly IEncryptionSchemeProvider _schemeProvider;
    private readonly IServiceProvider _serviceProvider;

    public EncryptionHandlerProvider(IEncryptionSchemeProvider schemeProvider, IServiceProvider serviceProvider)
    {
        _schemeProvider = schemeProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<IEncryptionHandler> GetContentEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default)
    {
        var encryptionScheme = await _schemeProvider.GetSchemeAsync(scheme);

        var handler = _serviceProvider.GetRequiredService(encryptionScheme!.ContentEncryptionHandlerType) as IEncryptionHandler;

        await handler!.InitializeAsync(encryptionScheme);

        return handler;
    }

    public async Task<IEncryptionHandler?> GetKeyEncryptionHandlerAsync(string scheme, CancellationToken cancellationToken = default)
    {
        var encryptionScheme = await _schemeProvider.GetSchemeAsync(scheme);

        if (encryptionScheme!.KeyEncryptionHandlerType == null) return null;

        var handler = _serviceProvider.GetRequiredService(encryptionScheme.KeyEncryptionHandlerType) as IEncryptionHandler;

        if (handler != null)
            await handler.InitializeAsync(encryptionScheme);

        return handler;
    }
}
