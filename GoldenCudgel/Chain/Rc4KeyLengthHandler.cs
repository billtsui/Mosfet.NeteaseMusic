using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Rc4KeyLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs,byte[] shareArray, NcmObject ncmObject)
    {
        var readResult = fs.Read(shareArray, 0, 4);
        ncmObject.Rc4KeyLength = BitConverter.ToInt32(shareArray.AsSpan(0, 4));
        
        base.Handle(file, fs, shareArray, ncmObject);
    }
}