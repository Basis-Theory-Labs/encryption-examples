using Encryption.Structs;

namespace Encryption;

public abstract class EncryptionHandlerOptions
{
    /// <summary>
    /// Check that the options are valid. Should throw an exception if things are not ok.
    /// </summary>
    public virtual void Validate() { }

    /// <summary>
    /// Checks that the options are valid for a specific scheme
    /// </summary>
    /// <param name="scheme">The scheme being validated.</param>
    public virtual void Validate(string scheme)
        => Validate();

    public abstract EncryptionAlgorithm EncryptionAlgorithm { get; set; }

    public abstract KeyType KeyType { get; set; }

    public abstract int KeySize { get; set; }

    public virtual IList<KeyOperation> KeyOperations { get; set; } = new List<KeyOperation>
    {
        KeyOperation.Decrypt,
        KeyOperation.Encrypt
    };
}
