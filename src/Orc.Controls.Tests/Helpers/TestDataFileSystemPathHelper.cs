namespace Orc.Controls.UI;

using System;
using Catel.IO;

public static class TestDataFileSystemPathHelper
{
    public static string WorkingDirectory => Path.Combine(Environment.CurrentDirectory, "TestData", "FileSystem");
    public static string Folder1Path => Path.Combine(WorkingDirectory, "Folder1");
    public static string Folder2Path => Path.Combine(WorkingDirectory, "Folder2");
    public static string Folder21Path => Path.Combine(Folder2Path, "Folder21");
    public static string File1Path => Path.Combine(Folder1Path, "File1.txt");
    public static string File2Path => Path.Combine(Folder2Path, "File2.txt");
    public static string File21Path => Path.Combine(Folder21Path, "File21.txt");
    public static string File22Path => Path.Combine(Folder21Path, "File22.txt");
}
