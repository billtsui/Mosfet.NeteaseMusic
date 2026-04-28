using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Rc4KeyLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray,
        NcmObject ncmObject)
    {
        byte[] buffer = new byte[4];
        var readResult = fs.Read(buffer, 0, 4);
        ncmObject.Rc4KeyLength = BitConverter.ToInt32(buffer);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}