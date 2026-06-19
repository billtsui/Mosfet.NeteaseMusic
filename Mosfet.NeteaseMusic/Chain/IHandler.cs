using Mosfet.NeteaseMusic.Entities;

namespace Mosfet.NeteaseMusic.Chain;

public interface IHandler
{
    IHandler SetNext(IHandler handler);

    void Handle(FileInfo file, FileStream fs, byte[] aesDataArray, byte[] pictureDataArray, NcmObject ncmObject);
}