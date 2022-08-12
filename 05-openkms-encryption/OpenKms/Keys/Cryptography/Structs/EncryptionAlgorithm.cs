using OpenKms.Keys.Models;
using OpenKms.Keys.Structs;

namespace OpenKms.Keys.Cryptography.Structs;

/// <summary>
/// <see cref="JsonWebKey"/> encryption algorithm.
/// </summary>
public readonly struct EncryptionAlgorithm : IEquatable<EncryptionAlgorithm>
{
    public const string HS256Value = "HS256";
    public const string HS384Value = "HS384";
    public const string HS512Value = "HS512";
    public const string RS256Value = "RS256";
    public const string RS384Value = "RS384";
    public const string RS512Value = "RS512";
    public const string ES256Value = "ES256";
    public const string ES384Value = "ES384";
    public const string ES512Value = "ES512";
    public const string PS256Value = "PS256";
    public const string PS384Value = "PS384";
    public const string PS512Value = "PS512";
    public const string NoneValue = "none";
    public const string RSA1_5Value = "RSA1_5";
    public const string RSA_OAEPValue = "RSA-OAEP";
    public const string RSA_OAEP_256Value = "RSA-OAEP-256";
    public const string A128KWValue = "A128KW";
    public const string A192KWValue = "A192KW";
    public const string A256KWValue = "A256KW";
    public const string DirValue = "dir";
    public const string ECDH_ESValue = "ECDH-ES";
    public const string ECDH_ES_A128KWValue = "ECDH-ES+A128KW";
    public const string ECDH_ES_A256KWValue = "ECDH-ES+A256KW";
    public const string A128GCMKWValue = "A128GCMKW";
    public const string A192GCMKWValue = "A192GCMKW";
    public const string A256GCMKWValue = "A256GCMKW";
    public const string PBES2_HS256_A128KWValue = "PBES2-HS256+A128KW";
    public const string PBES2_HS384_A192KWValue = "PBES2-HS384+A192KW";
    public const string PBES2_HS512_A256KWValue = "PBES2-HS512+A256KW";
    public const string A128CBC_HS256Value = "A128CBC-HS256";
    public const string A192CBC_HS384Value = "A192CBC-HS384";
    public const string A256CBC_HS512Value = "A256CBC-HS512";
    public const string A128GCMValue = "A128GCM";
    public const string A192GCMValue = "A192GCM";
    public const string A256GCMValue = "A256GCM";

    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="EncryptionAlgorithm"/> structure.
    /// </summary>
    /// <param name="value">The string value of the instance.</param>
    public EncryptionAlgorithm(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// HMAC using SHA-256
    /// </summary>
    public static EncryptionAlgorithm HS256 { get; } = new EncryptionAlgorithm(HS256Value);

    /// <summary>
    /// HMAC using SHA-384
    /// </summary>
    public static EncryptionAlgorithm HS384 { get; } = new EncryptionAlgorithm(HS384Value);

    /// <summary>
    /// HMAC using SHA-512
    /// </summary>
    public static EncryptionAlgorithm HS512 { get; } = new EncryptionAlgorithm(HS512Value);

    /// <summary>
    /// RSASSA-PKCS1-v1_5 using SHA-256
    /// </summary>
    public static EncryptionAlgorithm RS256 { get; } = new EncryptionAlgorithm(RS256Value);

    /// <summary>
    /// RSASSA-PKCS1-v1_5 using SHA-384
    /// </summary>
    public static EncryptionAlgorithm RS384 { get; } = new EncryptionAlgorithm(RS384Value);

    /// <summary>
    /// RSASSA-PKCS1-v1_5 using SHA-512
    /// </summary>
    public static EncryptionAlgorithm RS512 { get; } = new EncryptionAlgorithm(RS512Value);

    /// <summary>
    /// ECDSA using P-256 and SHA-256
    /// </summary>
    public static EncryptionAlgorithm ES256 { get; } = new EncryptionAlgorithm(ES256Value);

    /// <summary>
    /// ECDSA using P-384 and SHA-384
    /// </summary>
    public static EncryptionAlgorithm ES384 { get; } = new EncryptionAlgorithm(ES384Value);

    /// <summary>
    /// ECDSA using P-521 and SHA-512
    /// </summary>
    public static EncryptionAlgorithm ES512 { get; } = new EncryptionAlgorithm(ES512Value);

    /// <summary>
    /// RSASSA-PSS using SHA-256 and MGF1 with
    /// </summary>
    public static EncryptionAlgorithm PS256 { get; } = new EncryptionAlgorithm(PS256Value);

    /// <summary>
    /// RSASSA-PSS using SHA-384 and MGF1 with
    /// </summary>
    public static EncryptionAlgorithm PS384 { get; } = new EncryptionAlgorithm(PS384Value);

    /// <summary>
    /// RSASSA-PSS using SHA-512 and MGF1 with
    /// </summary>
    public static EncryptionAlgorithm PS512 { get; } = new EncryptionAlgorithm(PS512Value);

    /// <summary>
    /// No digital signature or MAC performed
    /// </summary>
    public static EncryptionAlgorithm None { get; } = new EncryptionAlgorithm(NoneValue);

    /// <summary>
    /// RSAES-PKCS1-v1_5
    /// </summary>
    public static EncryptionAlgorithm RSA1_5 { get; } = new EncryptionAlgorithm(RSA1_5Value);

    /// <summary>
    /// RSAES OAEP using default parameters
    /// </summary>
    public static EncryptionAlgorithm RSA_OAEP { get; } = new EncryptionAlgorithm(RSA_OAEPValue);

    /// <summary>
    /// RSAES OAEP using SHA-256 and MGF1 with
    /// </summary>
    public static EncryptionAlgorithm RSA_OAEP_256 { get; } = new EncryptionAlgorithm(RSA_OAEP_256Value);

    /// <summary>
    /// AES Key Wrap using 128-bit key
    /// </summary>
    public static EncryptionAlgorithm A128KW { get; } = new EncryptionAlgorithm(A128KWValue);

    /// <summary>
    /// AES Key Wrap using 192-bit key
    /// </summary>
    public static EncryptionAlgorithm A192KW { get; } = new EncryptionAlgorithm(A192KWValue);

    /// <summary>
    /// AES Key Wrap using 256-bit key
    /// </summary>
    public static EncryptionAlgorithm A256KW { get; } = new EncryptionAlgorithm(A256KWValue);

    /// <summary>
    /// Direct use of a shared symmetric key
    /// </summary>
    public static EncryptionAlgorithm Dir { get; } = new EncryptionAlgorithm(DirValue);

    /// <summary>
    /// ECDH-ES using Concat KDF
    /// </summary>
    public static EncryptionAlgorithm ECDH_ES { get; } = new EncryptionAlgorithm(ECDH_ESValue);

    /// <summary>
    /// ECDH-ES using Concat KDF and "A128KW"
    /// </summary>
    public static EncryptionAlgorithm ECDH_ES_A128KW { get; } = new EncryptionAlgorithm(ECDH_ES_A128KWValue);

    /// <summary>
    /// ECDH-ES using Concat KDF and "A256KW"
    /// </summary>
    public static EncryptionAlgorithm ECDH_ES_A256KW { get; } = new EncryptionAlgorithm(ECDH_ES_A256KWValue);

    /// <summary>
    /// Key wrapping with AES GCM using 128-bit key
    /// </summary>
    public static EncryptionAlgorithm A128GCMKW { get; } = new EncryptionAlgorithm(A128GCMKWValue);

    /// <summary>
    /// Key wrapping with AES GCM using 192-bit key
    /// </summary>
    public static EncryptionAlgorithm A192GCMKW { get; } = new EncryptionAlgorithm(A192GCMKWValue);

    /// <summary>
    /// Key wrapping with AES GCM using 256-bit key
    /// </summary>
    public static EncryptionAlgorithm A256GCMKW { get; } = new EncryptionAlgorithm(A256GCMKWValue);

    /// <summary>
    /// PBES2 with HMAC SHA-256 and "A128KW"
    /// </summary>
    public static EncryptionAlgorithm PBES2_HS256_A128KW { get; } = new EncryptionAlgorithm(PBES2_HS256_A128KWValue);

    /// <summary>
    /// PBES2 with HMAC SHA-384 and "A192KW"
    /// </summary>
    public static EncryptionAlgorithm PBES2_HS384_A192KW { get; } = new EncryptionAlgorithm(PBES2_HS384_A192KWValue);

    /// <summary>
    /// PBES2 with HMAC SHA-512 and "A256KW"
    /// </summary>
    public static EncryptionAlgorithm PBES2_HS512_A256KW { get; } = new EncryptionAlgorithm(PBES2_HS512_A256KWValue);

    /// <summary>
    /// AES_128_CBC_HMAC_SHA_256 authenticated
    /// </summary>
    public static EncryptionAlgorithm A128CBC_HS256 { get; } = new EncryptionAlgorithm(A128CBC_HS256Value);

    /// <summary>
    /// AES_192_CBC_HMAC_SHA_384 authenticated
    /// </summary>
    public static EncryptionAlgorithm A192CBC_HS384 { get; } = new EncryptionAlgorithm(A192CBC_HS384Value);

    /// <summary>
    /// AES_256_CBC_HMAC_SHA_512 authenticated
    /// </summary>
    public static EncryptionAlgorithm A256CBC_HS512 { get; } = new EncryptionAlgorithm(A256CBC_HS512Value);

    /// <summary>
    /// AES GCM using 128-bit key
    /// </summary>
    public static EncryptionAlgorithm A128GCM { get; } = new EncryptionAlgorithm(A128GCMValue);

    /// <summary>
    /// AES GCM using 192-bit key
    /// </summary>
    public static EncryptionAlgorithm A192GCM { get; } = new EncryptionAlgorithm(A192GCMValue);

    /// <summary>
    /// AES GCM using 256-bit key
    /// </summary>
    public static EncryptionAlgorithm A256GCM { get; } = new EncryptionAlgorithm(A256GCMValue);

    /// <summary>
    /// Determines if two <see cref="EncryptionAlgorithm"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="EncryptionAlgorithm"/> to compare.</param>
    /// <param name="right">The second <see cref="EncryptionAlgorithm"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are the same; otherwise, false.</returns>
    public static bool operator ==(EncryptionAlgorithm left, EncryptionAlgorithm right) => left.Equals(right);

    /// <summary>
    /// Determines if two <see cref="EncryptionAlgorithm"/> values are different.
    /// </summary>
    /// <param name="left">The first <see cref="EncryptionAlgorithm"/> to compare.</param>
    /// <param name="right">The second <see cref="EncryptionAlgorithm"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are different; otherwise, false.</returns>
    public static bool operator !=(EncryptionAlgorithm left, EncryptionAlgorithm right) => !left.Equals(right);

    /// <summary>
    /// Converts a string to a <see cref="KeyType"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    public static implicit operator EncryptionAlgorithm(string value) => new EncryptionAlgorithm(value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is KeyType other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(EncryptionAlgorithm other) => string.Equals(_value, other._value, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    /// <inheritdoc/>
    public override string ToString() => _value;
}
