using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace ENBLauncher
{
  class Program
  {
    static public bool DoesDirectoryExist(string path)
    {
      if (true == Directory.Exists(path))
        return true;
      else
        return false;
    }
    static public bool DoesFileExist(string path)
    {
      if (true == File.Exists(path))
        return true;
      else
        return false;
    }
    static public void DeleteDirectoryLink(string path)
    {
      if (true == DoesDirectoryExist(path))
        Directory.Delete(path);
    }
    static public void DeleteFileLink(string path)
    {
      if (true == DoesFileExist(path))
        File.Delete(path);
    }

    static public void CreateDirectoryLink(string sourcePath, string destPath)
    {
      DeleteDirectoryLink(destPath);
      Directory.CreateSymbolicLink(destPath, sourcePath);
    }
    static public void CreateFileLink(string sourcePath, string destPath)
    {
      DeleteFileLink(destPath);
      File.CreateSymbolicLink(destPath, sourcePath);
    }
    static readonly string modulePath = Process.GetCurrentProcess().MainModule.FileName;
    static readonly string rootDir = Path.GetDirectoryName(modulePath);
    static readonly string exit1 = "Press any key to exit the program.";

    static void Main(string[] args)
    {
      var configuration = new ConfigurationBuilder()
              .AddIniFile(Path.GetFileNameWithoutExtension(modulePath) + @".ini")
              .Build();
      string sourceDir = Path.Combine(rootDir, configuration["Folders:sourceDir"]);
      string destDir = Path.Combine(rootDir, configuration["Folders:destDir"]);
      string[] dirList = Directory.GetDirectories(sourceDir, "*");
      string[] fileList = Directory.GetFiles(sourceDir, "*");
      foreach (string dirPath in dirList)
        CreateDirectoryLink(dirPath, Path.Combine(destDir, dirPath.Substring(sourceDir.Length + 1)));
      foreach (string filePath in fileList)
        CreateDirectoryLink(filePath, Path.Combine(destDir, filePath.Substring(sourceDir.Length + 1)));
      Environment.Exit(0);
    }
  }
}