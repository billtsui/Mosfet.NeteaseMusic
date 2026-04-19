using System.Buffers;
using System.Text;
using System.Text.Json;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class MetaContentHandler : AbstractHandler
{
    private static readonly byte[] MetaKey =
        { 0x23, 0x31, 0x34, 0x6C, 0x6A, 0x6B, 0x5F, 0x21, 0x5C, 0x5D, 0x26, 0x30, 0x55, 0x3C, 0x27, 0x28 };

    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        int metaLength = ncmObject.MetaLength;
        var metaContentarray = ArrayPool<byte>.Shared.Rent(metaLength);
        var result = fs.Read(metaContentarray, 0, metaLength);
        //按字节对 0x63 异或
        for (var i = 0; i < metaLength; i++) metaContentarray[i] ^= 0x63;
        
        //去除固定字符 163 key(Don't modify): 前 22个字节
        int newMetaContentLength = metaLength - 22;
        var newMetaContentArray = ArrayPool<byte>.Shared.Rent(newMetaContentLength);
        Array.Copy(metaContentarray, 22, newMetaContentArray, 0, newMetaContentLength);

        var newMetaContentBase64Array = Convert.FromBase64String(Encoding.UTF8.GetString(newMetaContentArray.AsSpan(0, newMetaContentLength)));

        var aesDecryptArray = AESUtil.Decrypt(newMetaContentBase64Array, MetaKey);
        var metaContentStr = Encoding.UTF8.GetString(aesDecryptArray);

        //去除 music: 字符
        ncmObject.MetaData = metaContentStr.Replace("music:", "");
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        ncmObject.NeteaseCopyrightData =
            JsonSerializer.Deserialize<NeteaseCopyrightData>(ncmObject.MetaData, options) ??
            throw new InvalidOperationException();


        ArrayPool<byte>.Shared.Return(metaContentarray);
        ArrayPool<byte>.Shared.Return(newMetaContentArray);
        base.Handle(file, fs, ncmObject);
    }
}