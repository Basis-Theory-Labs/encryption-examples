namespace OpenKms.Keys;

/// <summary>
/// An opinionated abstraction for implementing <see cref="IEncryptionHandler"/>.
/// </summary>
/// <typeparam name="TOptions">The type for the options used to configure the encryption handler.</typeparam>
public abstract class EncryptionHandler<TOptions> where TOptions : EncryptionSchemeOptions, new()
{

}
