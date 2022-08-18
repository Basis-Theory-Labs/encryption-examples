using Azure.Core;
using Common.Constants;
using Common.Data;
using Common.Encryption;
using Encryption;
using Encryption.Extensions.DependencyInjection;
using Encryption.Structs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using OpenKms.AzureKeyVault;
using OpenKms.AzureKeyVault.Extensions;
using OpenKms.System.Security.Cryptography;
using OpenKms.System.Security.Cryptography.Extensions;

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
        services.AddHttpContextAccessor();

        services.AddSingleton<GuidKeyNameProvider>();
        services.AddEncryption(o =>
            {
                o.DefaultScheme = "default";
            })
            .AddScheme<AesEncryptionOptions, AesEncryptionHandler, AzureKeyVaultEncryptionOptions, AzureKeyVaultEncryptionHandler<GuidKeyNameProvider>>(EncryptionSchemes.BankAccountNumber, options =>
            {
                options.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
            }, options =>
            {
                options.KeySize = 4096;
                options.KeyType = KeyType.RSA;
                options.KeyName = EncryptionSchemes.BankAccountNumber;
                options.EncryptionAlgorithm = EncryptionAlgorithm.RSA_OAEP;
            })
            .AddScheme<AesEncryptionOptions, AesEncryptionHandler, AzureKeyVaultEncryptionOptions, AzureKeyVaultEncryptionHandler<GuidKeyNameProvider>>(EncryptionSchemes.BankRoutingNumber, options =>
            {
                options.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
            }, options =>
            {
                options.KeySize = 2048;
                options.KeyType = KeyType.RSA;
                options.KeyName = EncryptionSchemes.BankRoutingNumber;
                options.EncryptionAlgorithm = EncryptionAlgorithm.RSA_OAEP;
            })
            .AddScheme("builders that build builders", options =>
            {
                options.AddContentEncryption<AesEncryptionOptions, AesEncryptionHandler>(handlerOptions =>
                {
                    handlerOptions.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
                });

                options.AddKeyEncryption<AzureKeyVaultEncryptionOptions, AzureKeyVaultEncryptionHandler<GuidKeyNameProvider>>(
                handlerOptions =>
                {
                    handlerOptions.KeyName = configuration.GetValue<string>("Encryption:KeyName");
                    handlerOptions.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
                    handlerOptions.KeySize = 4096;
                });
            }).AddScheme("extension functions for builders to build other builders faster", builder =>
            {
                builder.AddAesContentEncryption(handlerOptions =>
                {
                    handlerOptions.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
                    handlerOptions.KeySize = 256;
                });

                builder.AddKeyVaultKeyEncryption(handlerOptions =>
                {
                    handlerOptions.KeyName = configuration.GetValue<string>("Encryption:KeyName");
                    handlerOptions.EncryptionAlgorithm = EncryptionAlgorithm.A256CBC_HS512;
                    handlerOptions.KeySize = 4096;
                });
            });


        services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Banks")));

        services.AddScoped<DatabaseMigrator>();

        return services;
    }
}
