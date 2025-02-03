namespace GoldenCudgel.Utils;

public class RC4
{
    private readonly byte[] _box = new byte[256];

    public void KSA(byte[] key)
    {
        var len = key.Length;
        for (var i = 0; i < 256; i++) _box[i] = (byte)i;

        for (int i = 0, j = 0; i < 256; i++)
        {
            j = (j + _box[i] + key[i % len]) & 0xff;
            (_box[i], _box[j]) = (_box[j], _box[i]);
        }
    }

    public void PRGA(byte[] data, int length)
    {
        for (var k = 0; k < length; k++)
        {
            var i = (k + 1) & 0xff;
            var j = (_box[i] + i) & 0xff;
            data[k] ^= _box[(_box[i] + _box[j]) & 0xff];
        }
    }
}