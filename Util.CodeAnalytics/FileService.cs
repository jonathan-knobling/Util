namespace Util.CodeAnalytics;

public static class FileService
{
    public static IEnumerable<string> GetFilesFromDirectory(string dir, string[] extensions)
    {
        if (!Directory.Exists(dir)) throw new DirectoryNotFoundException("Directory \"" + dir + "\" was not found!");

        return GetFiles(dir, extensions);
    }

    private static IEnumerable<string> GetFiles(string dir, string[] extensions)
    {
        var files = new List<string>();

        IEnumerable<string> filesInDir = Directory.EnumerateFiles(dir);
        IEnumerable<string> dirsInDir = Directory.EnumerateDirectories(dir);

        foreach (string subDir in dirsInDir)
        {
            if (subDir[^3..] is "obj" or "bin") continue;
            files.AddRange(GetFiles(subDir, extensions).Where(x => MatchFileExtensions(x, extensions)));
        }
        
        files.AddRange(filesInDir.Where(x => MatchFileExtensions(x, extensions)));
        
        return files;
    }

    private static bool MatchFileExtensions(string fileName, IEnumerable<string> extensions)
    {
        foreach (string extension in extensions)
        {
            ReadOnlySpan<char> fileNameExtension = fileName.AsSpan()[(fileName.Length - extension.Length)..];
            if (fileNameExtension.Equals(extension, StringComparison.Ordinal)) return true;
        }

        return false;
    }

    public static async Task<CodeData> AnalyseFiles(IEnumerable<string> filePaths)
    {
        var fileData = new List<FileData>();

        foreach (string filePath in filePaths)
        {
            fileData.Add(await AnalyseFile(filePath));
        }
        
        return EvaluateFileData(fileData);
    }

    private static async Task<FileData> AnalyseFile(string filePath)
    {
        string[] lines = await File.ReadAllLinesAsync(filePath);

        return new FileData
        {
            Lines = lines.Length,
            Name = ExtractFileNameFromPath(filePath)
        };
    }

    private static string ExtractFileNameFromPath(string filePath)
    {
        string[] split = filePath.Split('/', '\\');
        return split[^1];
    }

    private static CodeData EvaluateFileData(IReadOnlyCollection<FileData> fileData)
    {
        int totalLines = fileData.Sum(x => x.Lines);
        int numberOfFiles = fileData.Count;
        
        var biggestFileObj = fileData.MaxBy(x => x.Lines);
        string biggestFileName = biggestFileObj.Name;
        int biggestFileLines = biggestFileObj.Lines;

        var smallestFileObj = fileData.MinBy(x => x.Lines);
        string smallestFileName = smallestFileObj.Name;
        int smallestFileLines = smallestFileObj.Lines;
        
        return new CodeData
        {
            TotalLines = totalLines,
            NumberOfFiles = numberOfFiles,
            BiggestFile = biggestFileName,
            BiggestFileLines = biggestFileLines,
            SmallestFile = smallestFileName,
            SmallestFileLines = smallestFileLines
        };
    }
}