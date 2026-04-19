using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Rc4LengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var keyLengthArray = ArrayPool<byte>.Shared.Rent(4);
        var readResult = fs.Read(keyLengthArray, 0, 4);
        ncmObject.Rc4KeyLength = BitConverter.ToInt32(keyLengthArray.AsSpan(0, 4));
        ArrayPool<byte>.Shared.Return(keyLengthArray);
        
        base.Handle(file, fs, ncmObject);
    }
}