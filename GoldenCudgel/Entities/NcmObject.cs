namespace GoldenCudgel.Entities;

public class NcmObject
{
    public string? FileName { get; init; }
    
    public string? Header { get; set; }
    
    public int Rc4KeyLength { get; set; }

    public int Rc4KeyContentStart { get; set; }
    
    public int MetaLength { get; set; }
    
    public string? MetaData { get; set; }

    public int MusicDataArrayLength { get; set; }
    public int AlbumImageLength { get; set; }
    
    public NeteaseCopyrightData? NeteaseCopyrightData { get; set; }
    
    public string? NewFile { get; set; }

    public override string ToString()
    {
        return
            $"{FileName}, Meta length:{MetaLength} kb," +
            $"Cover image length:{AlbumImageLength / 1024} kb," +
            $"Music data length:{MusicDataArrayLength / 1024 / 1024} MB";
    }
}