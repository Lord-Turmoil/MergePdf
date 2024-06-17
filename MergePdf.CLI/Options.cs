using CommandLine;

namespace MergePdf;

class Options
{
    [Option('o', "output", Required = true, HelpText = "Output PDF file, will automatically add .pdf extension")]
    public string Output { get; set; }

    [Option('r', "recursive", Default = false, HelpText = "Find PDF files in given directory")]
    public bool IsRecursive { get; set; } = false;

    [Option('i', "input", Required = true, HelpText = "Input PDF files or directory")]
    public IEnumerable<string> Inputs { get; set; }

    [Option('n', "number", Default = false, HelpText = "Add page number to each page")]
    public bool AddPageNumber { get; set; } = false;

    [Option('f', "font", Required = false, HelpText = "Font name for page number")]
    public string? FontFamily { get; set; }

    [Option('s', "size", Required = false, HelpText = "Font size for page number")]
    public int FontSize { get; set; }

    [Option('l', "location", Required = false, HelpText = "Location of page number, tl, tm, tr, bl, bm, br")]
    public string? Location { get; set; }

    [Option("format", Required = false, HelpText = "Format of page number, $current and $total will be replaced")]
    public string? Format { get; set; }

    [Option('v', "verbose", Default = false, HelpText = "Print verbose log")]
    public bool Verbose { get; set; }
}