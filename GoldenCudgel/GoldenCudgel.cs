using GoldenCudgel.Chain;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;
using CommandLine;

namespace GoldenCudgel;

public class GoldenCudgel
{
    private static short _fileHolder = 30;

    public static void Main(string[] args)
    {
        Parameter p = new Parameter();
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                if (o.Path == null)
                {
                    Console.WriteLine("缺少路径参数！");
                    return;
                }

                p.path = o.Path;
                p.threadNum = o.ThreadNum;
            });
        Run(p);
    }

    private static void Run(Parameter parameter)
    {
        Console.WriteLine($"路径：{parameter.path}。线程数：{parameter.threadNum}");

        var fileInfoList = FileUtils.ReadFileList(parameter.path).OrderBy(file => file.Name).ToList();
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

        if (fileInfoList.Count > _fileHolder)
        {
            Task[] tasks = new Task[parameter.threadNum];

            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    int songCount = fileInfoList.Count % parameter.threadNum == 0
                        ? fileInfoList.Count / parameter.threadNum
                        : fileInfoList.Count / parameter.threadNum + 1;
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

        Console.WriteLine("已完成！");
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
                    fs.Close();
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

        [Option('t', "ThreadNumber", HelpText = "转换线程数。只支持1、2、4、8，默认 1")]
        public short ThreadNum { get; set; } = 1;
    }
}