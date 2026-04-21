using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        var length = ncmObject.AlbumImageLength;
        var readResult = fs.Read(shareArray, 0, length);

        base.Handle(file, fs, shareArray, ncmObject);
    }
}