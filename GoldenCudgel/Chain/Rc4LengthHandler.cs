using GoldenCudgel.Entities;

namespace GoldenCudgel.Chain;

public class Rc4LengthHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var keyLengthArray = new byte[4];
        var readResult = fs.Read(keyLengthArray, 0, keyLengthArray.Length);
        ncmObject.Rc4KeyLengthArray = keyLengthArray;
        ncmObject.Rc4KeyLength = BitConverter.ToInt32(keyLengthArray);

        base.Handle(file, fs, ncmObject);
    }
}