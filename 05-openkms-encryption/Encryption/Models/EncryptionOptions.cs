using Microsoft.Extensions.DependencyInjection;

namespace Encryption.Models;

/// <summary>
/// Options to configure Encryption
/// </summary>
public class EncryptionOptions
{
        private readonly IList<EncryptionSchemeBuilder> _schemes = new List<EncryptionSchemeBuilder>();

        /// <summary>
        /// Returns the schemes in the order they were added (important for request handling priority)
        /// </summary>
        public IEnumerable<EncryptionSchemeBuilder> Schemes => _schemes;

        /// <summary>
        /// Maps schemes by name.
        /// </summary>
        public IDictionary<string, EncryptionSchemeBuilder> SchemeMap { get; } = new Dictionary<string, EncryptionSchemeBuilder>(StringComparer.Ordinal);

        /// <summary>
        /// Adds an <see cref="EncryptionScheme"/>.
        /// </summary>
        /// <param name="builder">The name of the scheme being added.</param>
        /// <param name="configureBuilder">Configures the scheme.</param>
        public void AddScheme(EncryptionSchemeBuilder builder)
        {
            if (builder.Name == null)
                throw new ArgumentNullException(nameof(builder.Name));
            if (SchemeMap.ContainsKey(builder.Name))
                throw new InvalidOperationException($"Scheme already exists: {builder.Name}");

            _schemes.Add(builder);
            SchemeMap[builder.Name] = builder;
        }

        // /// <summary>
        // /// Adds an <see cref="EncryptionScheme"/>.
        // /// </summary>
        // /// <typeparam name="TContentEncryptionHandler">The <see cref="IEncryptionHandler"/> responsible for content encryption.</typeparam>
        // /// <param name="name">The name of the scheme being added.</param>
        // /// <param name="displayName">The display name for the scheme.</param>
        // public void AddScheme<TContentEncryptionHandler>(string name, IServiceCollection services) where TContentEncryptionHandler : IEncryptionHandler
        //     => AddScheme(name, services, b =>
        //     {
        //         b.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
        //     });
        //
        // /// <summary>
        // /// Adds an <see cref="EncryptionScheme"/>.
        // /// </summary>
        // /// <typeparam name="TContentEncryptionHandler">The <see cref="IEncryptionHandler"/> responsible for content encryption.</typeparam>
        // /// <typeparam name="TKeyEncryptionHandler">The <see cref="IEncryptionHandler"/> responsible for key encryption.</typeparam>
        // /// <param name="name">The name of the scheme being added.</param>
        // /// <param name="displayName">The display name for the scheme.</param>
        // public void AddScheme<TContentEncryptionHandler, TKeyEncryptionHandler>(string name, IServiceCollection services)
        //     where TContentEncryptionHandler : IEncryptionHandler
        //     where TKeyEncryptionHandler : IEncryptionHandler
        //     => AddScheme(name, services, b =>
        //     {
        //         b.ContentEncryptionHandlerType = typeof(TContentEncryptionHandler);
        //         b.KeyEncryptionHandlerType = typeof(TKeyEncryptionHandler);
        //     });

        /// <summary>
        /// Used as the fallback default scheme for all the other defaults.
        /// </summary>
        public string? DefaultScheme { get; set; }

        /// <summary>
        /// Used as the default scheme.
        /// </summary>
        public string? DefaultEncryptScheme { get; set; }
}
