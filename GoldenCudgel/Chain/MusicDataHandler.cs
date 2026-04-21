using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class MusicDataHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        var rc4 = new RC4();
        rc4.KSA(shareArray[ncmObject.Rc4KeyContentStart..]);
        var buffer = new byte[4096];

        //开头存了图片，接着往后存音乐数据
        int offset = ncmObject.AlbumImageLength;
        int sum = 0;
        for (int len; (len = fs.Read(buffer)) > 0;)
        {
            rc4.PRGA(buffer, len);
            Array.Copy(buffer, 0, shareArray, offset, len);
            offset += len;
            sum += len;
        }

        ncmObject.MusicDataArrayLength = sum;

        //兼容file signatures是mp3但后缀是flac的歌曲
        if (BitConverter.ToString(shareArray.AsSpan(ncmObject.AlbumImageLength, 3).ToArray()).Equals("49-44-33"))
            ncmObject.NeteaseCopyrightData.Format = "mp3";

        base.Handle(file, fs, shareArray, ncmObject);
    }
}