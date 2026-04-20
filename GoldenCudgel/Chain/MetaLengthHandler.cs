using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class MetaLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        short metaLength = 4;
        var readResult = fs.Read(shareArray, 0, metaLength);
        ncmObject.MetaLength = BitConverter.ToInt32(shareArray.AsSpan(0, metaLength));

        base.Handle(file, fs, shareArray, ncmObject);
    }
}