using GoldenCudgel.Entities;
using TagLib;
using File = TagLib.File;

namespace GoldenCudgel.Chain;

public class TagLibHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        //如果是mp4格式，不做tag信息处理
        if (ncmObject?.NeteaseCopyrightData?.Format is "mp4" or "mp3")
        {
            return;
        }
        
        //图片从track下载目录加载，格式为track-musicId.jpg
        var picName = $"track-{ncmObject?.NeteaseCopyrightData?.MusicId}.jpg";
        var picPath = OperatingSystem.IsMacOS()
            ? $"{file?.Directory?.FullName}/meta/{picName}"
            : $@"{file?.Directory?.FullName}\\meta\\{picName}";

        var musicFile = File.Create(ncmObject?.NewFile);
        using (var picStream = new FileStream(picPath, FileMode.Open, FileAccess.Read))
        {
            byte[] picBytes = new byte[picStream.Length];
            var readResult = picStream.Read(picBytes);
            if (readResult > 0)
            {
                var tagPic = new Picture(new ByteVector(picBytes));
                musicFile.Tag.Pictures = [tagPic];
            }
        }
        
        musicFile.Tag.Title = ncmObject.NeteaseCopyrightData.MusicName;
        musicFile.Tag.Album = ncmObject.NeteaseCopyrightData.Album;
        musicFile.Tag.Performers = [ncmObject.NeteaseCopyrightData.Artist[0][0]];
        musicFile.Save();
        musicFile.Dispose();

        base.Handle(file, fs, ncmObject);
    }
}