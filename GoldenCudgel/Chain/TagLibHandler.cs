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

        /* MacOS客户端图片从track下载目录加载，格式为track-musicId.jpg
         * Windows客户端没有track目录，仍然使用原方式
         */
        var musicFile = File.Create(ncmObject?.NewFile);
        var tagPic = new Picture();
        if (OperatingSystem.IsMacOS())
        {
            var picName = $"track-{ncmObject?.NeteaseCopyrightData?.MusicId}.jpg";
            var picPath = OperatingSystem.IsMacOS()
                ? $"{file?.Directory?.FullName}/meta/{picName}"
                : $@"{file?.Directory?.FullName}\\meta\\{picName}";
            using (var picStream = new FileStream(picPath, FileMode.Open, FileAccess.Read))
            {
                byte[] picBytes = new byte[picStream.Length];
                var readResult = picStream.Read(picBytes);
                if (readResult > 0)
                {
                    tagPic = new Picture(new ByteVector(picBytes));
                }
            }
        }
        else
        {
            tagPic = new Picture(new ByteVector(ncmObject?.AlbumImageContentArray));
        }

        musicFile.Tag.Pictures = [tagPic];


        musicFile.Tag.Title = ncmObject.NeteaseCopyrightData.MusicName;
        musicFile.Tag.Album = ncmObject.NeteaseCopyrightData.Album;
        musicFile.Tag.Performers = [ncmObject.NeteaseCopyrightData.Artist[0][0]];
        musicFile.Save();
        musicFile.Dispose();

        base.Handle(file, fs, ncmObject);
    }
}