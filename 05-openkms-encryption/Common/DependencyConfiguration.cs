using Azure.Core;
using Common.Data;
using Common.Encryption;
using Encryption.Extensions.DependencyInjection;
using Encryption.Structs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenKms.AzureKeyVault;
using OpenKms.System.Security.Cryptography;

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

        services.AddEncryption(o =>
            {
                o.DefaultScheme = "default";
            })
            .AddScheme<AesEncryptionOptions,AesEncryptionHandler, AzureKeyVaultEncryptionOptions, AzureKeyVaultEncryptionHandler>("default", "DEFAULT", options =>
            {
                options.DefaultKeyName = configuration.GetValue<string>("Encryption:KeyName");
                options.DefaultEncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
                options.DefaultKeySize = 256;
            }, options =>
            {
                options.DefaultKeyName = configuration.GetValue<string>("Encryption:KeyName");
                options.DefaultEncryptionAlgorithm = EncryptionAlgorithm.RSA_OAEP;
            });

        // services.Configure<AzureKeyVaultEncryptionOptions>(o =>
        // {
        //     o.DefaultKeyName = configuration.GetValue<string>("Encryption:KeyName");
        // });
        // services.AddSingleton<IEncryptionHandler, AzureKeyVaultEncryptionHandler>();
        // services.AddSingleton<AzureKeyVaultEncryptionHandler>();

        // services.AddSingleton<IKeyManagementService, KeyVaultKeyManagementService>();

        // services.AddSingleton<IEncryptionService, EncryptionService>();
        // services.AddSingleton<IEncryptionSchemeProvider, EncryptionSchemeProvider>((_) =>
        // {
        //     var provider = new EncryptionSchemeProvider();
        //     provider.AddScheme(new EncryptionScheme("default", "DEFAULT", typeof(AzureKeyVaultEncryptionHandler), null));
        //
        //     return provider;
        // });
        // services.AddSingleton<IEncryptionHandlerProvider, EncryptionHandlerProvider>();
        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Banks")));

        // services.AddSingleton<CommonEncryptionService>();
        services.AddScoped<DatabaseMigrator>();

        return services;
    }
}
