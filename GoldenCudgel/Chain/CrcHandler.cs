using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class CrcHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        fs.Seek(4, SeekOrigin.Current);

        base.Handle(file, fs, shareArray, ncmObject);
    }
}