using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class MusicDataHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var rc4 = new RC4();
        rc4.KSA(ncmObject.Rc4KeyContentArray);
        var buffer = new byte[0x8000];
        ncmObject.MusicDataArray = [];
        for (int len; (len = fs.Read(buffer)) > 0;)
        {
            rc4.PRGA(buffer, len);
            ncmObject.MusicDataArray.AddRange(buffer);
        }

        //兼容file signatures是mp3但后缀是flac的歌曲
        if (BitConverter.ToString(ncmObject.MusicDataArray.Slice(0, 3).ToArray()).Equals("49-44-33"))
            ncmObject.NeteaseCopyrightData.Format = "mp3";

        base.Handle(file, fs, ncmObject);
    }
}