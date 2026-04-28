using System.Buffers;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class Rc4KeyHandler : AbstractHandler
{
    private static readonly byte[] CoreKey =
        { 0x68, 0x7A, 0x48, 0x52, 0x41, 0x6D, 0x73, 0x6F, 0x35, 0x6B, 0x49, 0x6E, 0x62, 0x61, 0x78, 0x57 };

    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray, NcmObject ncmObject)
    {
        var rc4KeyLength = ncmObject.Rc4KeyLength;
        byte[] temp = new byte[rc4KeyLength];

        var readResult = fs.Read(temp, 0, rc4KeyLength);

        for (var i = 0; i < rc4KeyLength; i++) temp[i] ^= 0x64;

        var aesDecrypt = AESUtil.Decrypt(temp, CoreKey);
        ncmObject.Rc4KeyLength = aesDecrypt.Length - 17;
        Array.Copy(aesDecrypt, 17, rc4KeyDataArray, 0, aesDecrypt.Length - 17);
        
        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}