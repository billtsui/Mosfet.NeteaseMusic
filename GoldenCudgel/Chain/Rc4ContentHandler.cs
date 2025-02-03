using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class Rc4ContentHandler : AbstractHandler
{
    private static readonly byte[] CoreKey =
        { 0x68, 0x7A, 0x48, 0x52, 0x41, 0x6D, 0x73, 0x6F, 0x35, 0x6B, 0x49, 0x6E, 0x62, 0x61, 0x78, 0x57 };

    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        if (ncmObject.Rc4KeyLength > 0)
        {
            var rc4EncryptContent = new byte[ncmObject.Rc4KeyLength];

            var readResult = fs.Read(rc4EncryptContent, 0, ncmObject.Rc4KeyLength);

            for (var i = 0; i < rc4EncryptContent.Length; i++) rc4EncryptContent[i] ^= 0x64;

            var aesDecrypt = AESUtil.Decrypt(rc4EncryptContent, CoreKey);

            var key = new byte[aesDecrypt.Length - 17];
            Array.Copy(aesDecrypt, 17, key, 0, key.Length);

            ncmObject.Rc4KeyContentArray = key;
        }

        base.Handle(file, fs, ncmObject);
    }
}