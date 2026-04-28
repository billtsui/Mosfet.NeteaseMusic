using System.Buffers;
using System.Text;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class HeaderHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray,
        NcmObject ncmObject)
    {
        fs.Seek(8, SeekOrigin.Begin);

        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}