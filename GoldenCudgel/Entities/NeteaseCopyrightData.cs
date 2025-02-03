using System.Runtime.Serialization;

namespace GoldenCudgel.Entities;

[DataContract]
public class NeteaseCopyrightData
{
    [DataMember(Name = "musicId")] public string MusicId { get; set; }

    [DataMember(Name = "musicName")] public string MusicName { get; set; }

    [DataMember(Name = "artist")] public string[][] Artist { get; set; }

    [DataMember(Name = "albumId")] public int AlbumId { get; set; }

    [DataMember(Name = "album")] public string Album { get; set; }

    [DataMember(Name = "albumPicDocId")] public string AlbumPicDocId { get; set; }

    [DataMember(Name = "albumPic")] public string AlbumPic { get; set; }

    [DataMember(Name = "bitrate")] public int Bitrate { get; set; }

    [DataMember(Name = "mp3DocId")] public string Mp3DocId { get; set; }

    [DataMember(Name = "duration")] public int Duration { get; set; }

    [DataMember(Name = "mvId")] public string MvId { get; set; }

    [DataMember(Name = "alias")] public List<string> Alias { get; set; }

    [DataMember(Name = "format")] public string Format { get; set; }
}