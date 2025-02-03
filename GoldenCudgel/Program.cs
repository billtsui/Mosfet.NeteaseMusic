using GoldenCudgel.Chain;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel;

public class Program
{
    public static void Main(string[] args)
    {
    }

    private static void Run(Options options)
    {
        var fileInfoList = FileUtils.ReadFileList(options.Path);
        if (fileInfoList.Count == 0)
        {
            Console.WriteLine("No such file found.");
            return;
        }

        //创建单独的写入目录
        var directoryInfo = fileInfoList[0].Directory?.Parent;
        if (directoryInfo?.GetDirectories("convert").Length == 0)
        {
            directoryInfo?.CreateSubdirectory("convert");
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Find {fileInfoList.Count} songs.");
        if (fileInfoList.Count > 50)
        {
            //最多用4个线程处理
            int processorCount = Environment.ProcessorCount > 4
                ? 4
                : Environment.ProcessorCount;

            Task[] tasks = new Task[processorCount];

            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    int songCount = fileInfoList.Count % processorCount == 0
                        ? fileInfoList.Count / processorCount
                        : fileInfoList.Count / processorCount + 1;
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

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Done!");
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

            using var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            headerHandler.Handle(fileInfo, fs, ncmObject);
            fs.Close();
            Console.WriteLine(ncmObject.ToString());
        }
    }

    private static HeaderHandler AssembleChain()
    {
        var headerHandler = new HeaderHandler();
        var jumpHandler = new JumpHandler();
        var rc4LengthHandler = new Rc4LengthHandler();
        var rc4ContentHandler = new Rc4ContentHandler();
        var metaLengthHandler = new MetaLengthHandler();
        var metaContentHandler = new MetaContentHandler();
        var checkHandler = new CheckHandler();
        var jump2Handler = new Jump2Handler();
        var albumImageLengthHandler = new AlbumImageLengthHandler();
        var albumImageHandler = new AlbumImageHandler();
        var musicDataHandler = new MusicDataHandler();
        var fileCreateHandler = new FileCreateHandler();

        headerHandler.SetNext(jumpHandler)
            .SetNext(rc4LengthHandler)
            .SetNext(rc4ContentHandler)
            .SetNext(metaLengthHandler)
            .SetNext(metaContentHandler)
            .SetNext(checkHandler)
            .SetNext(jump2Handler)
            .SetNext(albumImageLengthHandler)
            .SetNext(albumImageHandler)
            .SetNext(musicDataHandler)
            .SetNext(fileCreateHandler);

        return headerHandler;
    }

    private class Options
    {
        // [Option('p', "path", Required = true, HelpText = "网易云音乐下载目录")]
        public string Path { get; set; }
    }
}