namespace GoldenCudgel.Entities;

public class NcmObject
{
    public string FileName { get; set; }

    public byte[] HeaderArray { get; set; }

    public string Header { get; set; }

    public byte[] Rc4KeyLengthArray { get; set; }

    public int Rc4KeyLength { get; set; }

    public byte[] Rc4KeyContentArray { get; set; }

    public byte[] MetaLengthArray { get; set; }

    public int MetaLength { get; set; }

    public byte[] MetaDataArray { get; set; }

    public string MetaData { get; set; }

    public byte[] CrcArray { get; set; }

    public byte[] AlbumImageLengthArray { get; set; }

    public int AlbumImageLength { get; set; }

    public byte[] AlbumImageContentArray { get; set; }

    public List<byte> MusicDataArray { get; set; }

    public NeteaseCopyrightData NeteaseCopyrightData { get; set; }
    
    public string NewFile { get; set; }

    public override string ToString()
    {
        return
            $"{FileName}, Meta length:{MetaLength} kb," +
            $"Cover image length:{AlbumImageLength / 1024} kb," +
            $"Music data length:{MusicDataArray.Count / 1024 / 1024} MB";
    }
}