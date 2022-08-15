namespace OpenKms.Keys.Cryptography.Operations;

public interface IKeyOperator : IEncryptDecrypt, ISignVerify, IWrapUnwrap, IDeriveKeyDeriveBits
{
}
