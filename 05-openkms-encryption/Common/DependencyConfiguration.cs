using Azure.Core;
using Common.Data;
using Common.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenKms.AzureKeyVault;
using OpenKms.Keys.Management;
using OpenKms.Keys.Management.Models;
using OpenKms.Keys.Structs;
using EncryptionAlgorithm = OpenKms.Keys.Cryptography.Structs.EncryptionAlgorithm;

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

        services.Configure<KeyVaultKeyManagementServiceOptions>(o =>
        {
            o.DefaultGenerateKeyRequest = new GenerateKeyRequest()
            {
                Algorithm = EncryptionAlgorithm.RSA1_5,
                KeySize = 2048,
                KeyType = KeyType.RSA
            };
        });
        services.AddSingleton<IKeyManagementService, KeyVaultKeyManagementService>();

        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Banks")));

        services.AddSingleton<EncryptionService>();
        services.AddScoped<DatabaseMigrator>();

        return services;
    }
}
