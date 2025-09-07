using GoldenCudgel.Entities;
using TagLib;
using File = TagLib.File;

namespace GoldenCudgel.Chain;

/**
 * mp4格式转换异常:TagLibSharp在处理 mp4 格式的时候会强制检查 box header声明的 box size和实际box size大小是否匹配
 * 如果不匹配会抛出异常，并且将file writable置为false,导致无法写入tag信息
 */
public class FileCreateHandler : AbstractHandler
{
    public override void Handle(FileInfo file, FileStream fs, NcmObject ncmObject)
    {
        var currentDir = file.Directory.Parent.FullName;
        if (OperatingSystem.IsMacOS()) currentDir += "/convert/";
        if (OperatingSystem.IsWindows()) currentDir += "\\convert\\";

        var newFile = $"{currentDir + file.Name[..^4]}.{ncmObject.NeteaseCopyrightData.Format}";

        ncmObject.NewFile = newFile;
        using (var stream = new FileStream(newFile, FileMode.Create, FileAccess.Write))
        {
            stream.Write(ncmObject.MusicDataArray.ToArray());
            stream.Close();
        }
        
        base.Handle(file, fs, ncmObject);
    }
}
