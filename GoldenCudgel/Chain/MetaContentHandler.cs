using System.Text;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;
using Newtonsoft.Json;

namespace GoldenCudgel.Chain;

public class MetaContentHandler : AbstractHandler
{
    private static readonly byte[] MetaKey =
        { 0x23, 0x31, 0x34, 0x6C, 0x6A, 0x6B, 0x5F, 0x21, 0x5C, 0x5D, 0x26, 0x30, 0x55, 0x3C, 0x27, 0x28 };

    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var metaContent = new byte[ncmObject.MetaLength];
        var result = fs.Read(metaContent, 0, ncmObject.MetaLength);

        //按字节对 0x63 异或
        for (var i = 0; i < metaContent.Length; i++) metaContent[i] ^= 0x63;

        //去除固定字符 163 key(Don't modify): 前 22个字节
        var newMetaContentArray = new byte[metaContent.Length - 22];
        Array.Copy(metaContent, 22, newMetaContentArray, 0, newMetaContentArray.Length);

        var newMetaContentBase64Array = Convert.FromBase64String(Encoding.UTF8.GetString(newMetaContentArray));

        var aesDecryptArray = AESUtil.Decrypt(newMetaContentBase64Array, MetaKey);
        var metaContentStr = Encoding.UTF8.GetString(aesDecryptArray);

        //去除 music: 字符
        ncmObject.MetaData = metaContentStr.Replace("music:", "");
        ncmObject.MetaDataArray = aesDecryptArray;
        ncmObject.NeteaseCopyrightData = JsonConvert.DeserializeObject<NeteaseCopyrightData>(ncmObject.MetaData) ??
                                         throw new InvalidOperationException();


        base.Handle(file, fs, ncmObject);
    }
}