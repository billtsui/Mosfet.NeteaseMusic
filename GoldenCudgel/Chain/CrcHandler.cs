using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class CrcHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray, NcmObject ncmObject)
    {
        fs.Seek(4, SeekOrigin.Current);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}