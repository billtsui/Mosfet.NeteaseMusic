using System.Buffers;
using System.Text;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class HeaderHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] shareArray, NcmObject ncmObject)
    {
        var readResult = fs.Read(shareArray, 0, 8);
        ncmObject.Header = Encoding.UTF8.GetString(shareArray[0..8]);

        base.Handle(file, fs, shareArray, ncmObject);
    }
}