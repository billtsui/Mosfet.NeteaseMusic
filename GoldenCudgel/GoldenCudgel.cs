using GoldenCudgel.Chain;
using GoldenCudgel.Entities;
using GoldenCudgel.Utils;

namespace GoldenCudgel;

public class GoldenCudgel
{
    private void Run(Parameter parameter)
    {
        var fileInfoList = FileUtils.ReadFileList(parameter.path);
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
            int processorCount = parameter.threadNum;

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

    private void ProcessFile(List<FileInfo> fileInfoList)
    {
        var headerHandler = AssembleChain();

        foreach (var fileInfo in fileInfoList)
        {
            var ncmObject = new NcmObject
            {
                FileName = fileInfo.Name
            };

            using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                headerHandler.Handle(fileInfo, fs, ncmObject);
                fs.Close();
            }
            Console.WriteLine(ncmObject.ToString());
        }
    }

    private HeaderHandler AssembleChain()
    {
        var headerHandler = new HeaderHandler();
        headerHandler.SetNext(new Jump2Handler())
                     .SetNext(new Rc4LengthHandler())
                     .SetNext(new Rc4ContentHandler())
                     .SetNext(new MetaLengthHandler())
                     .SetNext(new MetaContentHandler())
                     .SetNext(new CheckHandler())
                     .SetNext(new Jump2Handler())
                     .SetNext(new AlbumImageLengthHandler())
                     .SetNext(new AlbumImageHandler())
                     .SetNext(new MusicDataHandler())
                     .SetNext(new FileCreateHandler());

        return headerHandler;
    }
}
