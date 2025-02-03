using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public interface IHandler
{
    IHandler SetNext(IHandler handler);
    void Handle(FileInfo file, FileStream fs, NcmObject ncmObject);
}