using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Skip5Handler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        fs.Seek(5, SeekOrigin.Current);
        base.Handle(file, fs, shareArray, ncmObject);
    }
}