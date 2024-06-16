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
        Console.WriteLine("Thank you for using MergePdf");
        Console.WriteLine("Copyright (C) Tony's Studio 2024");
        Console.WriteLine();

        if (args.Length < 2 || (args[1] == "-r" && args.Length != 3))
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    MergePdf <output> <input1> <input2> ...");
            Console.WriteLine("    MergePdf <output> -r <input>");
            return;
        }

        var output = args[0];
        var isRecursive = args[1] == "-r";

        if (isRecursive)
        {
            var files = Directory.GetFiles(args[2], "*.pdf", SearchOption.TopDirectoryOnly);
            PdfHelper.Merge(output, files);
        }
        else
        {
            var files = args.Skip(1).ToArray();
            PdfHelper.Merge(output, files);
        }
    }
}