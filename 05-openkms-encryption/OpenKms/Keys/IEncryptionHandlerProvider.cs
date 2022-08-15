namespace OpenKms.Keys;

public interface IEncryptionHandlerProvider
{
    Task<IEncryptionHandler> GetHandlerAsync(string kmsScheme, CancellationToken cancellationToken = default);
}
