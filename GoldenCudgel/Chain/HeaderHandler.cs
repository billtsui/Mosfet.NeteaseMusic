using System.Buffers;
using System.Text;
using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class HeaderHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var header = ArrayPool<byte>.Shared.Rent(8);
        var readResult = fs.Read(header, 0, 8);
        ncmObject.Header = Encoding.UTF8.GetString(header.AsSpan(0, 8));
        
        ArrayPool<byte>.Shared.Return(header);
        base.Handle(file, fs, ncmObject);
    }
}