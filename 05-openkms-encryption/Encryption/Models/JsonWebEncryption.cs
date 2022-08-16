using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace Encryption.Models;

public class JsonWebEncryption
{
    /// <summary>
    /// with the value BASE64URL(UTF8(JWE Protected Header))
    /// </summary>
    [JsonPropertyName("protected")]
    public JoseHeader? ProtectedHeader { get; set; }

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

    public string ToCompactSerializationFormat()
    {
        var protectedHeader = "";
        if (ProtectedHeader != null)
        {
            var serializedHeader = JsonSerializer.Serialize(ProtectedHeader, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
            protectedHeader = Base64UrlEncoder.Encode(serializedHeader);
        }

        var encryptedKey = EncryptedKey != null ? Base64UrlEncoder.Encode(EncryptedKey) : "";

        var iv = InitializationVector != null ? Base64UrlEncoder.Encode(InitializationVector) : "";

        var ciphertext = Ciphertext != null ? Base64UrlEncoder.Encode(Ciphertext) : "";

        var authenticationTag = AuthenticationTag != null ? Base64UrlEncoder.Encode(AuthenticationTag) : "";

        return protectedHeader + "." + encryptedKey + "." + iv + "." + ciphertext + "." + authenticationTag;
    }
}
