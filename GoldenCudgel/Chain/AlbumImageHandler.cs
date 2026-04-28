using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray, NcmObject ncmObject)
    {
        var readResult = fs.Read(pictureDataArray, 0, ncmObject.AlbumImageLength);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}