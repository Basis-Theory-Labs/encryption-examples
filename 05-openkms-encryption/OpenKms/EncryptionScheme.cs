using OpenKms.Keys;

namespace OpenKms;

public class EncryptionScheme
{
    public EncryptionScheme(string name, string? displayName, Type handlerType)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (handlerType == null)
        {
            throw new ArgumentNullException(nameof(handlerType));
        }

        if (!typeof(IEncryptionHandler).IsAssignableFrom(handlerType))
        {
            throw new ArgumentException("handlerType must implement IAuthenticationHandler.");
        }

        Name = name;
        HandlerType = handlerType;
        DisplayName = displayName;
    }

    public string Name { get; }

    public string? DisplayName { get; }

    public Type HandlerType { get; }
}
