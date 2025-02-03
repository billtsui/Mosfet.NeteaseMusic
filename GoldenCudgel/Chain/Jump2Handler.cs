using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Jump2Handler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        fs.Seek(5, SeekOrigin.Current);
        base.Handle(file, fs, ncmObject);
    }
}