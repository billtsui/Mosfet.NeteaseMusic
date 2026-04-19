using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class MetaLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        short metaLength = 4;
        var metaLengthArray = ArrayPool<byte>.Shared.Rent(metaLength);
        var readResult = fs.Read(metaLengthArray, 0, metaLength);
        ncmObject.MetaLength = BitConverter.ToInt32(metaLengthArray.AsSpan(0, metaLength));

        ArrayPool<byte>.Shared.Return(metaLengthArray);
        base.Handle(file, fs, ncmObject);
    }
}