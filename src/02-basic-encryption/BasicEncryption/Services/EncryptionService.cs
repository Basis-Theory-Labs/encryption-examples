using System.Security.Cryptography;
using System.Text;

namespace BasicEncryption.Services;

public class EncryptionService
{
    private readonly string _encryptionKey;

    public EncryptionService(IConfiguration configuration)
    {
        _encryptionKey = configuration.GetValue<string>("EncryptionKey");
    }

    public async Task<string> Encrypt(string plaintext)
    {
        using var key = Aes.Create();
        key.Key = Encoding.ASCII.GetBytes(_encryptionKey);

        await using var msEncrypt = new MemoryStream();
        await using (var cryptoStream = new CryptoStream(msEncrypt, key.CreateEncryptor(), CryptoStreamMode.Write)) {
            await using (var streamWriter = new StreamWriter(cryptoStream))
            {
                await streamWriter.WriteAsync(plaintext);
            }
        }

        return $"{Convert.ToBase64String(msEncrypt.ToArray())}.{Convert.ToBase64String(key.IV)}";
    }

    public async Task<string> Decrypt(string ciphertext)
    {
        var keyParts = ciphertext.Split(".");

        using var key = Aes.Create();
        key.Key = Encoding.ASCII.GetBytes(_encryptionKey);
        key.IV = Convert.FromBase64String(keyParts[1]);

        await using var msDecrypt = new MemoryStream(Convert.FromBase64String(keyParts[0]));
        await using var cryptoStream = new CryptoStream(msDecrypt, key.CreateDecryptor(), CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return await streamReader.ReadToEndAsync();
    }
}
