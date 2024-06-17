using CommandLine;
using MergePdf.CLI;

namespace MergePdf;

class Program
{
    private static void Main(string[] args)
    {
        ConsoleColor consoleColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Thank you for using MergePdf");
        Console.WriteLine("Copyright (C) Tony's Studio 2024");
        Console.WriteLine();
        Console.ForegroundColor = consoleColor;


        try
        {
            Options options = ParseArgs(args);
            new Client(options).Run();
        }
        catch (ArgumentException)
        {
            // Nothing...
        }
    }

    private static Options ParseArgs(string[] args)
    {
        var option = new Options();

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(o => option = o)
            .WithNotParsed(_ => throw new ArgumentException("Invalid arguments"));

        List<string> inputs = [];
        foreach (string input in option.Inputs)
        {
            if (File.Exists(input))
            {
                inputs.Add(input);
            }
            else if (Directory.Exists(input))
            {
                inputs.AddRange(Directory.GetFiles(input, "*.pdf",
                    option.IsRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            }
            else
            {
                throw new ArgumentException($"File or directory {input} not found.");
            }
        }

        option.Inputs = inputs.ToArray();

        if (!option.Output.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            option.Output += ".pdf";
        }

        return option;
    }
}