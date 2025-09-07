using System.Diagnostics;
using GoldenCudgel.Chain;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;
using CommandLine;

namespace GoldenCudgel;

public class GoldenCudgel
{
    private static readonly short MinFileNum = 20;

    public static void Main(string[] args)
    {
        Parameter p = new Parameter();
        Parser.Default.ParseArguments<Options>(args)
            .WithNotParsed(HandleParseError)
            .WithParsed<Options>(o =>
            {
                p.Path = o.Path;
                p.ThreadNum = o.ThreadNum is 1 or 2 or 4 or 8 ? o.ThreadNum : (short)1;
            });
        Run(p);
    }

    private static void HandleParseError(IEnumerable<Error> errs)
    {
        errs.ToList().ForEach(Console.WriteLine);
    }

    private static void Run(Parameter parameter)
    {
        Console.WriteLine($"路径：{parameter.Path}。线程数：{parameter.ThreadNum}");

        var fileInfoList = FileUtils.ReadFileList(parameter.Path).OrderBy(file => file.Name).ToList();
        if (fileInfoList.Count == 0)
        {
            Console.WriteLine("当前路径未找到ncm文件!");
            return;
        }

        //创建单独的写入目录
        var directoryInfo = fileInfoList[0].Directory?.Parent;
        if (directoryInfo?.GetDirectories("convert").Length == 0)
        {
            directoryInfo?.CreateSubdirectory("convert");
        }

        Console.WriteLine($"找到 {fileInfoList.Count} 首歌曲.");

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        if (fileInfoList.Count >= MinFileNum)
        {
            Task[] tasks = new Task[parameter.ThreadNum];

            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    int songCount = fileInfoList.Count % parameter.ThreadNum == 0
                        ? fileInfoList.Count / parameter.ThreadNum
                        : fileInfoList.Count / parameter.ThreadNum + 1;
                    ProcessFile(fileInfoList.Skip(i1 * songCount).Take(songCount).ToList());
                });
            }

            foreach (var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks.ToArray());
        }
        else
        {
            ProcessFile(fileInfoList);
        }

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;
        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"已完成！用时 {elapsedTime}");
    }

    private static void ProcessFile(List<FileInfo> fileInfoList)
    {
        var headerHandler = AssembleChain();

        foreach (var fileInfo in fileInfoList)
        {
            var ncmObject = new NcmObject
            {
                FileName = fileInfo.Name
            };
            try
            {
                using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
                {
                    headerHandler.Handle(fileInfo, fs, ncmObject);
                    Console.WriteLine(ncmObject.ToString());
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FileName: {ncmObject.FileName}\n Error: {e}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    private static HeaderHandler AssembleChain()
    {
        var headerHandler = new HeaderHandler();
        headerHandler.SetNext(new Skip2Handler())
            .SetNext(new Rc4LengthHandler())
            .SetNext(new Rc4ContentHandler())
            .SetNext(new MetaLengthHandler())
            .SetNext(new MetaContentHandler())
            .SetNext(new CheckHandler())
            .SetNext(new Skip5Handler())
            .SetNext(new AlbumImageLengthHandler())
            .SetNext(new AlbumImageHandler())
            .SetNext(new MusicDataHandler())
            .SetNext(new FileCreateHandler())
            .SetNext(new TagLibHandler());

        return headerHandler;
    }

    private class Options
    {
        [Option('p', "Path", Required = true, HelpText = "网易云音乐下载目录")]
        public string? Path { get; set; }

        [Option('t', "ThreadNumber", HelpText = "转换线程数。只支持1、2、4、8，20首以上歌曲开启多线程处理")]
        public short ThreadNum { get; set; } = 1;
    }
}