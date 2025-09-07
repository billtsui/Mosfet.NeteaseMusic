using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public abstract class AbstractHandler : IHandler
{
    private IHandler? _nextHandler;

    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }

    public virtual void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        _nextHandler?.Handle(file, fs, ncmObject);
    }
}