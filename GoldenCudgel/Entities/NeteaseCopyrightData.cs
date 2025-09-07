using System.Runtime.Serialization;

namespace GoldenCudgel.Entities;

[DataContract]
public class NeteaseCopyrightData
{
     public string MusicId { get; set; } = "";

     public string MusicName { get; set; } = "";

     public string[][] Artist { get; set; } = new string[10][];

     public string AlbumId { get; set; } = "";

     public string Album { get; set; } = "";

     public string AlbumPicDocId { get; set; } = "";

     public string AlbumPic { get; set; } = "";

     public int Bitrate { get; set; }

     public string Mp3DocId { get; set; } = "";

     public int Duration { get; set; }

     public string MvId { get; set; } = "";

     public List<string> Alias { get; set; } = new List<string> { };

     public string Format { get; set; } = "";

     public short Fee { get; set; } = 0;

     public float VolumeDelta { get; set; } = 0.0f;
     
     public Privilege? Privilege { get; set; }
}
