using Azure.Core;
using Common.Data;
using Common.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static class DependencyConfiguration
{
    public static IServiceCollection RegisterCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLazyCache();

        var tokenCredential = new LocalTokenCredential();

        services.AddSingleton<TokenCredential>(tokenCredential);

        services.AddAzureClients(b =>
        {
            var keyVaultUri = new Uri(configuration.GetValue<string>("Encryption:ProviderUri"));
            b.AddKeyClient(keyVaultUri);
            b.UseCredential(tokenCredential);
        });

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Banks")));

        services.AddSingleton<EncryptionService>();
        services.AddScoped<DatabaseMigrator>();

        return services;
    }
}
