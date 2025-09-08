using CommandLine;
namespace GoldenCudgel.Entities;

public class Options
{
    [Option('p', "Path", Required = true, HelpText = "网易云音乐下载目录")]
    public string? Path { get; set; }

    [Option('t', "ThreadNumber", HelpText = "转换线程数。只支持1、2、4、8，20首以上歌曲开启多线程处理")]
    public short ThreadNum { get; set; } = 1;
}