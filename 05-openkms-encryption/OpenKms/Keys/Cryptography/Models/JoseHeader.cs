using System.Text.Json.Serialization;
using OpenKms.Keys.Cryptography.Structs;
using OpenKms.Keys.Models;

namespace OpenKms.Keys.Cryptography.Models;

public class JoseHeader
{
    [JsonPropertyName("alg")]
    public EncryptionAlgorithm Algorithm { get; set; }

    [JsonPropertyName("enc")]
    public EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    [JsonPropertyName("zip")]
    public CompressionAlgorithm? CompressionAlgorithm { get; set; }

    [JsonPropertyName("jku")]
    public Uri? JwkSetUrl { get; set; }

    [JsonPropertyName("jwk")]
    public JsonWebKey? JsonWebKey { get; set; }

    [JsonPropertyName("kid")]
    public string? KeyId { get; set; }

    [JsonPropertyName("x5u")]
    public Uri? X509Url { get; set; }

    [JsonPropertyName("x5c")]
    public string? X509CertificateChain { get; set; }

    [JsonPropertyName("x5t")]
    public string? X509CertificateSha1Thumbprint { get; set; }

    [JsonPropertyName("x5t#S256")]
    public string? X509CertificateSha256Thumbprint { get; set; }

    [JsonPropertyName("typ")]
    public string? Type { get; set; }

    [JsonPropertyName("cty")]
    public string? ContentType { get; set; }

    [JsonPropertyName("crit")]
    public IList<string>? CriticalHeaderParameters { get; set; }


}
