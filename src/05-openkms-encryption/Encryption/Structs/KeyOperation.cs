using Encryption.Models;

namespace Encryption.Structs;

/// <summary>
/// <see cref="JsonWebKey"/> key_ops.
/// </summary>
public readonly struct KeyOperation
{
    public const string SignValue = "sign";
    public const string VerifyValue = "verify";
    public const string EncryptValue = "encrypt";
    public const string DecryptValue = "decrypt";
    public const string WrapKeyValue = "wrapKey";
    public const string UnwrapKeyValue = "unwrapKey";
    public const string DeriveKeyValue = "deriveKey";
    public const string DeriveBitsValue = "deriveBits";

    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyOperation"/> structure.
    /// </summary>
    /// <param name="value">The string value of the instance.</param>
    public KeyOperation(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Compute digital signature or MAC
    /// </summary>
    public static KeyOperation Sign { get; } = new KeyOperation(SignValue);

    /// <summary>
    /// Verify digital signature or MAC
    /// </summary>
    public static KeyOperation Verify { get; } = new KeyOperation(VerifyValue);

    /// <summary>
    /// Encrypt content
    /// </summary>
    public static KeyOperation Encrypt { get; } = new KeyOperation(EncryptValue);

    /// <summary>
    /// Decrypt content and validate decryption, if applicable
    /// </summary>
    public static KeyOperation Decrypt { get; } = new KeyOperation(DecryptValue);

    /// <summary>
    /// Encrypt key
    /// </summary>
    public static KeyOperation WrapKey { get; } = new KeyOperation(WrapKeyValue);

    /// <summary>
    /// Decrypt key and validate decryption, if applicable
    /// </summary>
    public static KeyOperation UnwrapKey { get; } = new KeyOperation(UnwrapKeyValue);

    /// <summary>
    /// Derive key
    /// </summary>
    public static KeyOperation DeriveKey { get; } = new KeyOperation(DeriveKeyValue);

    /// <summary>
    /// Derive bits not to be used as a key
    /// </summary>
    public static KeyOperation DeriveBits { get; } = new KeyOperation(DeriveBitsValue);

    /// <summary>
    /// Determines if two <see cref="KeyOperation"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="KeyOperation"/> to compare.</param>
    /// <param name="right">The second <see cref="KeyOperation"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are the same; otherwise, false.</returns>
    public static bool operator ==(KeyOperation left, KeyOperation right) => left.Equals(right);

    /// <summary>
    /// Determines if two <see cref="KeyOperation"/> values are different.
    /// </summary>
    /// <param name="left">The first <see cref="KeyOperation"/> to compare.</param>
    /// <param name="right">The second <see cref="KeyOperation"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are different; otherwise, false.</returns>
    public static bool operator !=(KeyOperation left, KeyOperation right) => !left.Equals(right);

    /// <summary>
    /// Converts a string to a <see cref="KeyOperation"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    public static implicit operator KeyOperation(string value) => new KeyOperation(value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is KeyOperation other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(KeyOperation other) => string.Equals(_value, other._value, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    /// <inheritdoc/>
    public override string ToString() => _value;

}
