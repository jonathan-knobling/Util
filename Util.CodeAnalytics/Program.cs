using Util.CodeAnalytics;

(string inputDir, string[] extensions) = await UserInterface.GetInitialInput();

IEnumerable<string> filePaths = FileService.GetFilesFromDirectory(inputDir, extensions);

var result = await  FileService.AnalyseFiles(filePaths);

Console.WriteLine(result);