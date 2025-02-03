using System.Security.Cryptography;

namespace GoldenCudgel.Utils;

public class AESUtil
{
    public static byte[] Decrypt(byte[] dataToDecrypt, byte[] key)
    {
        var aesAlgorithm = Aes.Create();
        aesAlgorithm.Key = key;
        aesAlgorithm.Padding = PaddingMode.PKCS7;
        aesAlgorithm.Mode = CipherMode.ECB;

        var cryptoTransform = aesAlgorithm.CreateDecryptor();

        var transformFinalBlock = cryptoTransform.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
        cryptoTransform.Dispose();
        return transformFinalBlock;
    }
}