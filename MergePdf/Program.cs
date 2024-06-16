namespace MergePdf;

/// <summary>
/// Usage:
///     MergePdf <output> <input1> <input2> ...
///     MergePdf <output> -r <input>
/// </summary>
class Program
{
    private static void Main(string[] args)
    {
        var consoleColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Thank you for using MergePdf");
        Console.WriteLine("Copyright (C) Tony's Studio 2024");
        Console.WriteLine();
        Console.ForegroundColor = consoleColor;


        try
        {
            ParseArgs(args, out var output, out var inputs, out var isRecursive);
            PdfHelper.Merge(output, inputs);
        }
        catch (ArgumentException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Usage:");
            Console.WriteLine("    MergePdf <output> <input1> <input2> ...");
            Console.WriteLine("    MergePdf <output> -r <input>");
        }
        catch (Exception e)
        {
            // Write with red color
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to merge PDF files: {e.Message}");
        }
        finally
        {
            Console.ForegroundColor = consoleColor;
        }
    }

    private static void ParseArgs(string[] args, out string output, out string[] inputs, out bool isRecursive)
    {
        if (args.Length == 0)
        {
            string? line;
            Console.Write("Output filename: ");
            line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException("Output filename cannot be empty.");
            }
            output = line;

            Console.Write("Recursive? (y/n): ");
            line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new ArgumentException("Recursive cannot be empty.");
            }
            isRecursive = line == "y";

            if (isRecursive)
            {
                Console.Write("Input directory: ");
                line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    throw new ArgumentException("Input directory cannot be empty.");
                }
                inputs = Directory.GetFiles(line, "*.pdf", SearchOption.TopDirectoryOnly);
            }
            else
            {
                Console.WriteLine("Input filenames, one filename each line, end with empty line.");
                List<string> lines = [];
                while (true)
                {
                    line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    lines.Add(line.Trim());
                }

                inputs = lines.ToArray();
            }
        }
        else
        {
            if (args.Length < 2 || (args[1] == "-r" && args.Length != 3))
            {
                throw new ArgumentException("Invalid arguments.");
            }

            output = args[0];
            isRecursive = args[1] == "-r";
            inputs = isRecursive
                ? Directory.GetFiles(args[2], "*.pdf", SearchOption.TopDirectoryOnly)
                : args.Skip(1).ToArray();
        }

        if (!output.EndsWith(".pdf"))
        {
            output += output.EndsWith('.') ? "pdf" : ".pdf";
        }
    }
}