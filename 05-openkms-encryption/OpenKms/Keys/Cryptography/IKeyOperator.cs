namespace OpenKms.Keys.Cryptography;

public interface IKeyOperator : IEncryptDecrypt, ISignVerify, IWrapUnwrap, IDeriveKeyDeriveBits
{
}
