using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel.Chain;

public class MusicDataHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray,
        NcmObject ncmObject)
    {
        var rc4 = new RC4();
        rc4.KSA(rc4KeyDataArray);
        var buffer = new byte[4096];

        int offset = 0;
        int sum = 0;

        var currentDir = file?.Directory?.Parent?.FullName;
        if (OperatingSystem.IsMacOS()) currentDir += "/convert/";
        if (OperatingSystem.IsWindows()) currentDir += "\\convert\\";

        var newFile = $"{currentDir + file?.Name[..^4]}.{ncmObject?.NeteaseCopyrightData?.Format}";
        using (var stream = new FileStream(newFile, FileMode.Create, FileAccess.Write))
        {
            for (int len; (len = fs.Read(buffer)) > 0;)
            {
                rc4.PRGA(buffer, len);
                stream.Write(buffer, 0, len);
                offset += len;
                sum += len;
            }

            stream.Close();
        }


        ncmObject.MusicDataArrayLength = sum;
        //兼容file signatures是mp3但后缀是flac的歌曲
        // if (BitConverter.ToString(musicDataArray[0..3])
        //     .Equals("49-44-33"))
        //     ncmObject.NeteaseCopyrightData.Format = "mp3";

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}