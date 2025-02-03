using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var length = ncmObject.AlbumImageLength;
        var imageArray = new byte[length];
        var readResult = fs.Read(imageArray, 0, length);

        ncmObject.AlbumImageContentArray = imageArray;
        base.Handle(file, fs, ncmObject);
    }
}