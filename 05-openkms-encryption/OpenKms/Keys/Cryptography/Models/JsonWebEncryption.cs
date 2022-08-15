using System.Text.Json.Serialization;

namespace OpenKms.Keys.Cryptography.Models;

public class JsonWebEncryption
{
    [JsonPropertyName("protected")]
    public string? ProtectedHeader { get; set; }

    [JsonPropertyName("unprotected")]
    public JoseHeader? UnprotectedHeader { get; set; }

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
