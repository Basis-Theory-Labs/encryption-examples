using Encryption.Models;
using Microsoft.Extensions.Options;

namespace Encryption;

public interface IEncryptionSchemeProvider
{
    Task<IEnumerable<EncryptionScheme>> GetSchemesAsync();
    Task<EncryptionScheme?> GetSchemeAsync(string schemeName);
    Task<EncryptionScheme?> GetDefaultEncryptionSchemeAsync();

    void AddScheme(EncryptionScheme scheme);
    bool TryAddScheme(EncryptionScheme scheme);
    void RemoveScheme(string schemeName);
}

public class EncryptionSchemeProvider : IEncryptionSchemeProvider
{
    private readonly IDictionary<string, EncryptionScheme> _schemes;
    private readonly EncryptionOptions _options;

    /// <summary>
    /// Creates an instance of <see cref="EncryptionSchemeProvider"/>
    /// using the specified <paramref name="options"/>,
    /// </summary>
    /// <param name="options">The <see cref="EncryptionOptions"/> options.</param>
    public EncryptionSchemeProvider(IOptions<EncryptionOptions> options)
        : this(options, new Dictionary<string, EncryptionScheme>(StringComparer.Ordinal))
    {
    }

    /// <summary>
    /// Creates an instance of <see cref="EncryptionSchemeProvider"/>
    /// using the specified <paramref name="options"/> and <paramref name="schemes"/>.
    /// </summary>
    /// <param name="options">The <see cref="EncryptionOptions"/> options.</param>
    /// <param name="schemes">The dictionary used to store authentication schemes.</param>
    protected EncryptionSchemeProvider(IOptions<EncryptionOptions> options, IDictionary<string, EncryptionScheme> schemes)
    {
        _options = options.Value;

        _schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));

        foreach (var builder in _options.Schemes)
        {
            var scheme = builder.Build();
            AddScheme(scheme);
        }
    }

    private Task<EncryptionScheme?> GetDefaultSchemeAsync()
        => _options.DefaultScheme != null
            ? GetSchemeAsync(_options.DefaultScheme)
            : Task.FromResult<EncryptionScheme?>(null);

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

    public Task<EncryptionScheme?> GetDefaultEncryptionSchemeAsync()
    {
        return _options.DefaultEncryptionScheme != null
            ? GetSchemeAsync(_options.DefaultEncryptionScheme)
            : GetDefaultSchemeAsync();
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
