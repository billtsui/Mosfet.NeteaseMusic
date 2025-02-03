namespace GoldenCudgel.Entities;

public class NcmObject
{
    private byte[] _albumImageContentArray;

    private byte[] _albumImageLengthArray;

    private byte[] _crcArray;
    private string _fileName;
    private string _header;

    private byte[] _headerArray;
    private string _metaData;

    private byte[] _metaDataArray;

    private byte[] _metaLengthArray;
    private List<byte> _musicDataArray;

    private NeteaseCopyrightData _neteaseCopyrightData;

    private byte[] _rc4KeyContentArray;

    private byte[] _rc4KeyLengthArray;


    public string FileName
    {
        get => _fileName;
        set => _fileName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public byte[] HeaderArray
    {
        get => _headerArray;
        set => _headerArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Header
    {
        get => _header;
        set => _header = value ?? throw new ArgumentNullException(nameof(value));
    }

    public byte[] Rc4KeyLengthArray
    {
        get => _rc4KeyLengthArray;
        set => _rc4KeyLengthArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Rc4KeyLength { get; set; }

    public byte[] Rc4KeyContentArray
    {
        get => _rc4KeyContentArray;
        set => _rc4KeyContentArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public byte[] MetaLengthArray
    {
        get => _metaLengthArray;
        set => _metaLengthArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int MetaLength { get; set; }

    public byte[] MetaDataArray
    {
        get => _metaDataArray;
        set => _metaDataArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string MetaData
    {
        get => _metaData;
        set => _metaData = value ?? throw new ArgumentNullException(nameof(value));
    }

    public byte[] CrcArray
    {
        get => _crcArray;
        set => _crcArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public byte[] AlbumImageLengthArray
    {
        get => _albumImageLengthArray;
        set => _albumImageLengthArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int AlbumImageLength { get; set; }

    public byte[] AlbumImageContentArray
    {
        get => _albumImageContentArray;
        set => _albumImageContentArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<byte> MusicDataArray
    {
        get => _musicDataArray;
        set => _musicDataArray = value ?? throw new ArgumentNullException(nameof(value));
    }

    public NeteaseCopyrightData NeteaseCopyrightData
    {
        get => _neteaseCopyrightData;
        set => _neteaseCopyrightData = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override string ToString()
    {
        return
            $"Name:{FileName}, " +
            $"Meta length:{MetaLength} bytes, " +
            $"Album image length:{AlbumImageLength / 1024} kb, " +
            $"Music data length:{MusicDataArray.Count / 1024 / 1024} MB";
    }
}