using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        var length = ncmObject.AlbumImageLength;
        //专辑图片从 0 开始存储
        var readResult = fs.Read(shareArray, 0, length);

        base.Handle(file, fs, shareArray, ncmObject);
    }
}