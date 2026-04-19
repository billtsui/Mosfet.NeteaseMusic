using System.Buffers;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class Rc4KeyHandler : AbstractHandler
{
    private static readonly byte[] CoreKey =
        { 0x68, 0x7A, 0x48, 0x52, 0x41, 0x6D, 0x73, 0x6F, 0x35, 0x6B, 0x49, 0x6E, 0x62, 0x61, 0x78, 0x57 };

    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var rc4KeyLength = ncmObject.Rc4KeyLength;
        var rc4KeyEncryptArray = ArrayPool<byte>.Shared.Rent(rc4KeyLength);

        var readResult = fs.Read(rc4KeyEncryptArray, 0, rc4KeyLength);

        for (var i = 0; i < rc4KeyEncryptArray.Length; i++) rc4KeyEncryptArray[i] ^= 0x64;

        var aesDecrypt = AESUtil.Decrypt(rc4KeyEncryptArray.AsSpan(0,rc4KeyLength).ToArray(), CoreKey);

        int keyArrayLength = aesDecrypt.Length - 17;
        var keyArray = ArrayPool<byte>.Shared.Rent(keyArrayLength);
        Array.Copy(aesDecrypt, 17, keyArray, 0, keyArrayLength);

        ncmObject.Rc4KeyContentArray = keyArray.AsSpan(0,keyArrayLength).ToArray();

        ArrayPool<byte>.Shared.Return(rc4KeyEncryptArray);
        ArrayPool<byte>.Shared.Return(keyArray);
        base.Handle(file, fs, ncmObject);
    }
}