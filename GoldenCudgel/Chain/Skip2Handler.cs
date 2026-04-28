using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Skip2Handler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray,
        NcmObject ncmObject)
    {
        fs.Seek(2, SeekOrigin.Current);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}