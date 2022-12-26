namespace Util.CodeAnalytics;

public struct CodeData
{
    public int TotalLines;
    public int NumberOfFiles;
    public string BiggestFile;
    public int BiggestFileLines;
    public string SmallestFile;
    public int SmallestFileLines;

    public override string ToString()
    {
        return $"Total Lines: {TotalLines}" + Environment.NewLine +
               $"Number Of Files: {NumberOfFiles}" + Environment.NewLine +
               $"Biggest File: {BiggestFile} with {BiggestFileLines} Lines of Code" + Environment.NewLine +
               $"Smallest File: {SmallestFile} with {SmallestFileLines} Lines of Code";
    }
}

public struct FileData
{
    public int Lines;
    public string Name;
}