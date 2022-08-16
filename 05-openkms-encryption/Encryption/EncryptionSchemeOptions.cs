using Encryption.Structs;

namespace Encryption;

public class EncryptionSchemeOptions
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

    public virtual EncryptionAlgorithm DefaultEncryptionAlgorithm { get; set; } = EncryptionAlgorithm.RSA1_5;

    public string DefaultKeyName { get; set; } = ".default";
}
