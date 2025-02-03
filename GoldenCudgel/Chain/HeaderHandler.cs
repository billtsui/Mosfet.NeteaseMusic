using System.Text;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class HeaderHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var header = new byte[8];
        var readResult = fs.Read(header, 0, header.Length);

        ncmObject.HeaderArray = header;
        ncmObject.Header = Encoding.UTF8.GetString(header);

        base.Handle(file, fs, ncmObject);
    }
}