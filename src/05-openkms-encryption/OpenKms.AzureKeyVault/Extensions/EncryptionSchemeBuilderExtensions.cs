using System.Runtime.Serialization;
using Encryption;
using Encryption.Builders;

namespace OpenKms.AzureKeyVault.Extensions;

public static class EncryptionSchemeBuilderExtensions
{
    public static EncryptionSchemeBuilder AddKeyVaultKeyEncryption(this EncryptionSchemeBuilder schemeBuilder,
        Action<AzureKeyVaultEncryptionOptions> configureOptions)
    {
        schemeBuilder.AddKeyEncryption<AzureKeyVaultEncryptionOptions,
            AzureKeyVaultEncryptionHandler<GuidKeyNameProvider>>(configureOptions);
        return schemeBuilder;
    }

    public static EncryptionSchemeBuilder AddKeyVaultContentEncryption(this EncryptionSchemeBuilder schemeBuilder,
        Action<AzureKeyVaultEncryptionOptions> configureOptions)
    {
        schemeBuilder.AddContentEncryption<AzureKeyVaultEncryptionOptions,
            AzureKeyVaultEncryptionHandler<GuidKeyNameProvider>>(configureOptions);
        return schemeBuilder;
    }

    public static EncryptionSchemeBuilder AddKeyVaultKeyEncryption<TKeyNameProvider>(
        this EncryptionSchemeBuilder schemeBuilder,
        Action<AzureKeyVaultEncryptionOptions> configureOptions)
        where TKeyNameProvider : IKeyNameProvider
    {
        schemeBuilder.AddKeyEncryption<AzureKeyVaultEncryptionOptions,
            AzureKeyVaultEncryptionHandler<TKeyNameProvider>>(configureOptions);

        return schemeBuilder;
    }

    public static EncryptionSchemeBuilder AddKeyVaultContentEncryption<TKeyNameProvider>(this EncryptionSchemeBuilder schemeBuilder,
        Action<AzureKeyVaultEncryptionOptions> configureOptions)
        where TKeyNameProvider : IKeyNameProvider
    {
        schemeBuilder.AddContentEncryption<AzureKeyVaultEncryptionOptions,
            AzureKeyVaultEncryptionHandler<TKeyNameProvider>>(configureOptions);
        return schemeBuilder;
    }
}
