using OpenKms.Keys.Models;

namespace OpenKms.Keys.Structs;

/// <summary>
/// <see cref="JsonWebKey"/> public key use.
/// </summary>
public readonly struct PublicKeyUse : IEquatable<PublicKeyUse>
{
    private const string SignatureValue = "sig";
    private const string EncryptionValue = "enc";

    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="PublicKeyUse"/> structure.
    /// </summary>
    /// <param name="value">The string value of the instance.</param>
    public PublicKeyUse(string value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Public key use intended for "enc" (encryption).
    /// </summary>
    public static PublicKeyUse Signature { get; } = new PublicKeyUse(SignatureValue);

    /// <summary>
    /// Public key use intended for "sig" (signature).
    /// </summary>
    public static PublicKeyUse Encryption { get; } = new PublicKeyUse(EncryptionValue);

    /// <summary>
    /// Determines if two <see cref="PublicKeyUse"/> values are the same.
    /// </summary>
    /// <param name="left">The first <see cref="PublicKeyUse"/> to compare.</param>
    /// <param name="right">The second <see cref="PublicKeyUse"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are the same; otherwise, false.</returns>
    public static bool operator ==(PublicKeyUse left, PublicKeyUse right) => left.Equals(right);

    /// <summary>
    /// Determines if two <see cref="PublicKeyUse"/> values are different.
    /// </summary>
    /// <param name="left">The first <see cref="PublicKeyUse"/> to compare.</param>
    /// <param name="right">The second <see cref="PublicKeyUse"/> to compare.</param>
    /// <returns>True if <paramref name="left"/> and <paramref name="right"/> are different; otherwise, false.</returns>
    public static bool operator !=(PublicKeyUse left, PublicKeyUse right) => !left.Equals(right);

    /// <summary>
    /// Converts a string to a <see cref="KeyType"/>.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    public static implicit operator PublicKeyUse(string value) => new PublicKeyUse(value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is KeyType other && Equals(other);

    /// <inheritdoc/>
    public bool Equals(PublicKeyUse other) => string.Equals(_value, other._value, StringComparison.Ordinal);

    /// <inheritdoc/>
    public override int GetHashCode() => _value?.GetHashCode() ?? 0;

    /// <inheritdoc/>
    public override string ToString() => _value;
}
