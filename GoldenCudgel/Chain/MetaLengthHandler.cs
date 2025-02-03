using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class MetaLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var metaLengthArray = new byte[4];
        var readResult = fs.Read(metaLengthArray, 0, metaLengthArray.Length);
        ncmObject.MetaLengthArray = metaLengthArray;
        ncmObject.MetaLength = BitConverter.ToInt32(metaLengthArray, 0);

        base.Handle(file, fs, ncmObject);
    }
}