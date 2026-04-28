using System.Buffers;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class MetaLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray, NcmObject ncmObject)
    {
        short metaLength = 4;
        byte[] temp = new byte[metaLength];
        var readResult = fs.Read(temp, 0, metaLength);
        ncmObject.MetaLength = BitConverter.ToInt32(temp);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}