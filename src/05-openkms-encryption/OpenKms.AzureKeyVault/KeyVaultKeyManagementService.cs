// using Azure;
// using Azure.Security.KeyVault.Keys;
// using Microsoft.Extensions.Options;
// using OpenKms.AzureKeyVault.Extensions;
// using OpenKms.Keys.Cryptography.Operations.Models;
// using OpenKms.Keys.Exceptions;
// using OpenKms.Keys.Management;
// using OpenKms.Keys.Management.Models;
// using OpenKms.Keys.Structs;
// using DecryptResult = OpenKms.Keys.Cryptography.Operations.Models.DecryptResult;
// using EncryptResult = OpenKms.Keys.Cryptography.Operations.Models.EncryptResult;
// using JsonWebKey = OpenKms.Keys.Models.JsonWebKey;
// using KeyOperation = OpenKms.Keys.Structs.KeyOperation;
// using KeyType = OpenKms.Keys.Structs.KeyType;
// using SignResult = OpenKms.Keys.Cryptography.Operations.Models.SignResult;
//
// namespace OpenKms.AzureKeyVault;
//
// public class KeyVaultKeyManagementService : IKeyManagementService
// {
//     private readonly KeyClient _keyClient;
//     private readonly IOptionsMonitor<AzureKeyVaultEncryptionOptions> _options;
//
//     public KeyVaultKeyManagementService(KeyClient keyClient, IOptionsMonitor<AzureKeyVaultEncryptionOptions> options)
//     {
//         _keyClient = keyClient;
//         _options = options;
//     }
//
//     public JsonWebKey GenerateKey(string keyName, CancellationToken cancellationToken = default)
//     {
//         var defaultRequest = _options.CurrentValue.DefaultGenerateKeyRequest;
//         if (defaultRequest == null)
//             throw new NotImplementedException();
//
//         return GenerateKey(keyName, defaultRequest, cancellationToken);
//     }
//
//     public JsonWebKey GenerateKey(string keyName, GenerateKeyRequest request, CancellationToken cancellationToken = default)
//     {
//         var keyVaultKeyResponse = request.KeyType.ToString() switch
//         {
//             "RSA" => CreateRsaKey(keyName, request.KeySize, cancellationToken),
//             _ => throw new ArgumentOutOfRangeException()
//         };
//
//         return keyVaultKeyResponse.Value.ToJsonWebKey();
//     }
//
//     private Response<KeyVaultKey> CreateRsaKey(string keyName, int? keySize, CancellationToken cancellationToken = default)
//     {
//         var createRsaKeyOptions = new CreateRsaKeyOptions(keyName)
//         {
//             KeySize = keySize,
//             KeyOperations = { Azure.Security.KeyVault.Keys.KeyOperation.Decrypt, Azure.Security.KeyVault.Keys.KeyOperation.Encrypt }, // TODO
//         };
//
//         return _keyClient.CreateRsaKey(createRsaKeyOptions, cancellationToken);
//     }
//
//     public JsonWebKey FindKey(KeyType keyType, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey? FindKey(string keyName, CancellationToken cancellationToken = default)
//     {
//         try
//         {
//             var keyResult = _keyClient.GetKey(keyName, cancellationToken: cancellationToken);
//             return keyResult.Value.ToJsonWebKey();
//         }
//         catch (RequestFailedException ex)
//         {
//             // Log key not found
//             return null;
//         }
//     }
//
//     public JsonWebKey GetKey(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey RotateKey(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey RotateKey(JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey ImportKey(JsonWebKey source, string? keyName = null, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey ExportKey(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public JsonWebKey ExportKey(JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public KeyState EnableKey(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public KeyState EnableKey(JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public KeyState DisableKey(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public KeyState DisableKey(JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public bool SupportsKeyType(KeyType keyType, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public bool SupportsKeyId(string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public IList<KeyOperation> SupportedKeyOperations { get; }
//     public bool CanDo(KeyOperation operation)
//     {
//         throw new NotImplementedException();
//     }
//
//     public EncryptResult Encrypt(EncryptRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public EncryptResult Encrypt(EncryptRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         if (key.KeyId == null)
//             throw new ArgumentNullException(nameof(key.KeyId));
//
//         var (keyName, keyVersion) = ParseKeyId(key.KeyId);
//         var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);
//
//         var algorithm = request.Algorithm ?? _options.CurrentValue.DefaultEncryptionAlgorithms[key.KeyType];
//         var encryptResult = cryptoClient.Encrypt(algorithm.ToString(), request.Plaintext, cancellationToken);
//
//         return new EncryptResult(encryptResult.Ciphertext, algorithm.ToString(), encryptResult.KeyId);
//     }
//
//     private static (string, string?) ParseKeyId(string keyId)
//     {
//         var splitKeyUri = keyId.Split("/keys/");
//         if (splitKeyUri.Length != 2)
//             throw new KeyIdNotSupportedException();
//
//         var keyIdParts = splitKeyUri[1].Split("/");
//
//         return keyIdParts.Length switch
//         {
//             2 => (keyIdParts[0], keyIdParts[1]),
//             1 => (keyIdParts[0], null),
//             _ => throw new KeyIdNotSupportedException()
//         };
//     }
//
//     public DecryptResult Decrypt(DecryptRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public DecryptResult Decrypt(DecryptRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         if (key.KeyId == null)
//             throw new ArgumentNullException(nameof(key.KeyId));
//
//         var (keyName, keyVersion) = ParseKeyId(key.KeyId);
//
//         var cryptoClient = _keyClient.GetCryptographyClient(keyName, keyVersion);
//
//
//         var algorithm = request.Algorithm ?? _options.CurrentValue.DefaultEncryptionAlgorithms[key.KeyType];
//         var decryptResult = cryptoClient.Decrypt(algorithm.ToString(), request.Ciphertext, cancellationToken);
//
//         return new DecryptResult(decryptResult.Plaintext, algorithm.ToString(), decryptResult.KeyId);
//     }
//
//     public SignResult Sign(SignRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public SignResult Sign(SignRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public VerifyRequest Verify(VerifyRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public VerifyRequest Verify(VerifyRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public WrapKeyResult WrapKey(WrapKeyRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public WrapKeyResult WrapKey(WrapKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public UnwrapKeyRequest UnwrapKey(UnwrapKeyRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public UnwrapKeyRequest UnwrapKey(UnwrapKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public DeriveKeyResult DeriveKey(DeriveKeyRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public DeriveKeyResult DeriveKey(DeriveKeyRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public DeriveBitsRequest DeriveBits(DeriveBitsRequest request, string keyId, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
//
//     public DeriveBitsRequest DeriveBits(DeriveBitsRequest request, JsonWebKey key, CancellationToken cancellationToken = default)
//     {
//         throw new NotImplementedException();
//     }
// }
