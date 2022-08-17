using Encryption.Models;

namespace Encryption;

public class EncryptionSchemeBuilder
{
    public EncryptionSchemeBuilder(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of the scheme being built.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the display name for the scheme being built.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the content encryption <see cref="IEncryptionHandler"/> type responsible for this scheme.
    /// </summary>
    public Type? ContentEncryptionHandlerType { get; set; }

    /// <summary>
    /// Gets or sets the key encryption <see cref="IEncryptionHandler"/> type responsible for this scheme.
    /// </summary>
    public Type? KeyEncryptionHandlerType { get; set; }

    /// <summary>
    /// Builds the <see cref="EncryptionScheme"/> instance.
    /// </summary>
    /// <returns>The <see cref="EncryptionScheme"/>.</returns>
    public EncryptionScheme Build()
    {
        if (ContentEncryptionHandlerType is null)
        {
            throw new InvalidOperationException($"{nameof(ContentEncryptionHandlerType)} must be configured to build an {nameof(EncryptionScheme)}.");
        }

        return new EncryptionScheme(Name, DisplayName, ContentEncryptionHandlerType, null, KeyEncryptionHandlerType);
    }
}
