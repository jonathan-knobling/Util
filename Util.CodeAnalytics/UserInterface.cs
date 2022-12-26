namespace Util.CodeAnalytics;

public static class UserInterface
{
    public static async Task<(string, string[])> GetInitialInput()
    {
        var inputDir = "";
        var inputExtensions = "";

        while (string.IsNullOrWhiteSpace(inputDir))
        {
            Console.Clear();
            Console.Write("Please enter the full path to the project to be analysed: ");
            inputDir = await Console.In.ReadLineAsync() ?? string.Empty;
        }

        while (string.IsNullOrWhiteSpace(inputExtensions))
        {
            //Reset Console
            Console.Clear();
            Console.WriteLine("Please enter the full path to the project to be analysed: " + inputDir);
    
            Console.Write("Please enter the filename extensions seperated by ',' " +
                          "of the files you want to include in the analysis: ");

            inputExtensions = await Console.In.ReadLineAsync() ?? string.Empty;
        }

        string[] extensionsArray = inputExtensions.Split(',', StringSplitOptions.TrimEntries);
        extensionsArray = extensionsArray.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        return (inputDir, extensionsArray);
    }
}