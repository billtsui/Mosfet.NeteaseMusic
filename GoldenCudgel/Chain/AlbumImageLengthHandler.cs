using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class AlbumImageLengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, byte[] rc4KeyDataArray, byte[] pictureDataArray, NcmObject ncmObject)
    {
        byte[] temp = new byte[4];
        var readResult = fs.Read(temp, 0, 4);

        ncmObject.AlbumImageLength = BitConverter.ToInt32(temp);
        
        base.Handle(file, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
    }
}