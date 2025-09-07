using GoldenCudgel.Entities;
using TagLib;
using File = TagLib.File;

namespace GoldenCudgel.Chain;

public class TagLibHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        //如果是mp4格式，不做tag信息处理
        if (ncmObject.NeteaseCopyrightData.Format is "mp4" or "mp3")
        {
            return;
        }

        var currentDir = file.Directory.Parent.FullName;
        if (OperatingSystem.IsMacOS()) currentDir += "/convert/";
        if (OperatingSystem.IsWindows()) currentDir += "\\convert\\";

        var destPath = $"{currentDir + file.Name[..^4]}.{ncmObject.NeteaseCopyrightData.Format}";
        //图片从track下载目录加载，track-musicId.jpg
        var picName = $"track-{ncmObject.NeteaseCopyrightData.MusicId}.jpg";
        var picPath = OperatingSystem.IsMacOS()
            ? $"{file?.Directory?.FullName}/meta/{picName}"
            : $@"{file?.Directory?.FullName}\\meta\\{picName}";

        var musicFile = File.Create(destPath);
        using (var picStream = new FileStream(picPath, FileMode.Open, FileAccess.Read))
        {
            byte[] picBytes = new Byte[picStream.Length];
            var readResult = picStream.Read(picBytes);
            var tagPic = new Picture(new ByteVector(picBytes));
            musicFile.Tag.Pictures = [tagPic];
            musicFile.Tag.Title = ncmObject.NeteaseCopyrightData.MusicName;
            musicFile.Tag.Album = ncmObject.NeteaseCopyrightData.Album;
            musicFile.Tag.Performers = [ncmObject.NeteaseCopyrightData.Artist[0][0]];
            musicFile.Save();
            musicFile.Dispose();
        }

        base.Handle(file, fs, ncmObject);
    }
}