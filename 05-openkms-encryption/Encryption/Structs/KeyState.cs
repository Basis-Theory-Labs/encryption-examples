namespace Encryption.Structs;

public enum KeyState
{
    INITIAL,
    ENABLED,
    DISABLED,
    EXPIRED,
    DESTROYED,
    COMPROMISED,
}
