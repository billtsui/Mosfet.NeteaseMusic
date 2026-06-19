namespace Mosfet.NeteaseMusic.Utils;

public class FileUtils
{
    public static List<FileInfo> ReadFileList(string path,string extension)
    {
        var directoryInfo = new DirectoryInfo(path);
        if (!directoryInfo.Exists) return [];

        var fileInfoList = directoryInfo.GetFiles();
        return fileInfoList.Length == 0
            ? []
            : fileInfoList.Where(f => f.Extension.Equals(extension)).ToList();
    }
}