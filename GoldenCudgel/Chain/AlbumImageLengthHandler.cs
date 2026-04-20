using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        var readResult = fs.Read(shareArray, 0, 4);

        ncmObject.AlbumImageLength = BitConverter.ToInt32(shareArray[0..4], 0);

        base.Handle(file, fs, shareArray, ncmObject);
    }
}