using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Encryption.Extensions.DependencyInjection;

public static class EncryptionServiceCollectionExtensions
{
    public static EncryptionBuilder AddEncryption(this IServiceCollection services)
    {
        services.TryAddScoped<IEncryptionService, EncryptionService>();
        services.TryAddScoped<IEncryptionHandlerProvider, EncryptionHandlerProvider>();
        services.TryAddSingleton<IEncryptionSchemeProvider, EncryptionSchemeProvider>();

        return new EncryptionBuilder(services);
    }
}
