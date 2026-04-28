using System.Buffers;
using System.Diagnostics;
using GoldenCudgel.Chain;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;
using CommandLine;

namespace GoldenCudgel;

public static class GoldenCudgel
{
    private const short MinFileCount = 20;
    private static HeaderHandler _headerHandler = new();

    public static void Main(string[] args)
    {
        var p = new Parameter();
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o =>
            {
                p.Path = o.Path;
                p.ThreadNum = o.ThreadNum is 1 or 2 or 4 or 8 ? o.ThreadNum : (short)1;
            });

        if (string.IsNullOrEmpty(p.Path)) return;
        _headerHandler = AssembleChain();
        Run(p);
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
            directoryInfo.CreateSubdirectory("convert");
        }

        Console.WriteLine($"找到 {fileInfoList.Count} 首歌曲.");

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        if (fileInfoList.Count >= MinFileCount)
        {
            Task[] tasks = new Task[parameter.ThreadNum];

            for (var i = 0; i < tasks.Length; i++)
            {
                int tempI = i;
                tasks[i] = new Task(() =>
                {
                    int songCount = fileInfoList.Count % parameter.ThreadNum == 0
                        ? fileInfoList.Count / parameter.ThreadNum
                        : fileInfoList.Count / parameter.ThreadNum + 1;
                    ProcessFile(fileInfoList.Skip(tempI * songCount).Take(songCount).ToList());
                });
            }

            tasks.AsParallel().ForAll(t => t.Start());

            Task.WaitAll(tasks.ToArray());
        }
        else
        {
            ProcessFile(fileInfoList);
        }

        stopWatch.Stop();
        var ts = stopWatch.Elapsed;
        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"已完成！用时 {elapsedTime}");
    }

    private static void ProcessFile(List<FileInfo> fileInfoList)
    {
        fileInfoList = fileInfoList.OrderByDescending(f => f.Length).ToList();

        //给图片5MB空间
        var pictureDataArray = new byte[5 * 1024 * 1024];

        //rc4给1024kb
        byte[] rc4KeyDataArray = new byte[1024 * 1024];

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
                    _headerHandler.Handle(fileInfo, fs, rc4KeyDataArray, pictureDataArray, ncmObject);
                }

                Console.WriteLine(ncmObject.ToString());
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"FileName: {ncmObject.FileName}\n Error: {e}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Array.Clear(pictureDataArray, 0, pictureDataArray.Length);
                Array.Clear(rc4KeyDataArray, 0, rc4KeyDataArray.Length);
            }
        }
    }

    private static HeaderHandler AssembleChain()
    {
        _headerHandler.SetNext(new Skip2Handler())
            .SetNext(new Rc4KeyLengthHandler())
            .SetNext(new Rc4KeyHandler())
            .SetNext(new MetaLengthHandler())
            .SetNext(new MetaContentHandler())
            .SetNext(new CrcHandler())
            .SetNext(new Skip5Handler())
            .SetNext(new AlbumImageLengthHandler())
            .SetNext(new AlbumImageHandler())
            .SetNext(new MusicDataHandler());

        return _headerHandler;
    }
}