using Encryption.Models;

namespace Encryption;

public interface IEncryptionSchemeProvider
{
    Task<IEnumerable<EncryptionScheme>> GetSchemesAsync();
    Task<EncryptionScheme?> GetSchemeAsync(string schemeName);
    Task<EncryptionScheme> GetDefaultEncryptionSchemeAsync();

    void AddScheme(EncryptionScheme scheme);
    bool TryAddScheme(EncryptionScheme scheme);
    void RemoveScheme(string schemeName);
}

public class EncryptionSchemeProvider : IEncryptionSchemeProvider
{
    private readonly Dictionary<string, EncryptionScheme> _schemes = new Dictionary<string, EncryptionScheme>();

    public Task<IEnumerable<EncryptionScheme>> GetSchemesAsync()
    {
        return Task.FromResult(_schemes.Select(s => s.Value));
    }

    public Task<EncryptionScheme?> GetSchemeAsync(string schemeName)
    {
        if (!_schemes.TryGetValue(schemeName, out var scheme))
            return Task.FromResult<EncryptionScheme?>(null);

        return Task.FromResult<EncryptionScheme?>(scheme);
    }

    public Task<EncryptionScheme> GetDefaultEncryptionSchemeAsync()
    {
        return Task.FromResult(_schemes["default"]); // TODO provide through options class
    }

    public void AddScheme(EncryptionScheme scheme)
    {
        _schemes.Add(scheme.Name, scheme);
    }

    public bool TryAddScheme(EncryptionScheme scheme)
    {
        throw new NotImplementedException();
    }

    public void RemoveScheme(string schemeName)
    {
        throw new NotImplementedException();
    }
}
