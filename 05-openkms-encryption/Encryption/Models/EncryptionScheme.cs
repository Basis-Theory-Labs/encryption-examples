namespace Encryption.Models;

public class EncryptionScheme
{
    public EncryptionScheme(string name, string? displayName, Type contentEncryptionHandlerType, Type?  contentEncryptionKeyNameProviderType = null, Type? keyEncryptionHandlerType = null,
        Type? keyEncryptionKeyNameProviderType = null)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (contentEncryptionHandlerType == null)
        {
            throw new ArgumentNullException(nameof(contentEncryptionHandlerType));
        }

        if (!typeof(IEncryptionHandler).IsAssignableFrom(contentEncryptionHandlerType))
        {
            throw new ArgumentException("contentEncryptionHandlerType must implement IEncryptionHandler.");
        }

        if (keyEncryptionHandlerType != null && !typeof(IEncryptionHandler).IsAssignableFrom(keyEncryptionHandlerType))
        {
            throw new ArgumentException("keyEncryptionHandlerType must implement IEncryptionHandler.");
        }

        Name = name;
        ContentEncryptionHandlerType = contentEncryptionHandlerType;
        KeyEncryptionHandlerType = keyEncryptionHandlerType;
        DisplayName = displayName;
    }

    public string Name { get; }

    public string? DisplayName { get; }

    public Type ContentEncryptionHandlerType { get; }

    public Type? ContentEncryptionKeyNameProviderType { get; }
    public Type? KeyEncryptionHandlerType { get; }

    public Type? KeyEncryptionKeyNameProviderType { get; }
}
