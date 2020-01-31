using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileManager
{
    public const string PNG_FILE_PATTERN = "*.png";

    public static FileInfo[] GetFilesFromFolder(string folderPath, params string[] patterns)
    {
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        if (dir.Exists)
        {
            List<FileInfo> files = new List<FileInfo>();
            foreach (var pattern in patterns)
            {
                files.AddRange(dir.GetFiles(pattern).ToList());
            }
            return files.ToArray();
        }
        else
        {
            return null;
        }
    }

    public static FileInfo[] GetFilesFromFolder(string folderPath, string searchPattern)
    {
        DirectoryInfo dir = new DirectoryInfo(folderPath);
        return dir.Exists ? dir.GetFiles(searchPattern) : null;
    }

    public static FileInfo[] GetPngJpgFilesFromFolder(string folderPath)
    {
        return GetFilesFromFolder(folderPath, "*.jpg", "*.png");
    }

    public static void CopyDirectory(string srcDirPath, string dstDirPath, string filesPattern, bool copySubDirs)
    {
        DirectoryInfo dir = new DirectoryInfo(srcDirPath);
        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + srcDirPath);
        }

        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(dstDirPath))
        {
            Directory.CreateDirectory(dstDirPath);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles(filesPattern);
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(dstDirPath, file.Name);
            file.CopyTo(temppath, true);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(dstDirPath, subdir.Name);
                CopyDirectory(subdir.FullName, temppath, filesPattern, copySubDirs);
            }
        }
    }
}
