using System.Text.Json.Serialization;

namespace Encryption.Models;

public class JsonWebEncryption
{
    /// <summary>
    /// with the value BASE64URL(UTF8(JWE Protected Header))
    /// </summary>
    [JsonPropertyName("protected")]
    public string? ProtectedHeader { get; set; }

    /// <summary>
    /// with the value JWE Shared Unprotected Header
    /// </summary>
    [JsonPropertyName("unprotected")]
    public JoseHeader? UnprotectedHeader { get; set; }

    /// <summary>
    /// with the value JWE Per-Recipient Unprotected Header
    /// </summary>
    [JsonPropertyName("header")]
    public JoseHeader? Header { get; set; }

    [JsonPropertyName("encrypted_key")]
    public string? EncryptedKey { get; set; }

    [JsonPropertyName("iv")]
    public string? InitializationVector { get; set; }

    [JsonPropertyName("ciphertext")]
    public string? Ciphertext { get; set; }

    [JsonPropertyName("tag")]
    public string? AuthenticationTag { get; set; }

    [JsonPropertyName("aad")]
    public string? AdditionalAuthenticatedData { get; set; }
}
