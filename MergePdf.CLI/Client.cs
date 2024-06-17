// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

using MergePdf.Core;

namespace MergePdf.CLI;

class Client
{
    private readonly Options _options;
    private IMergeProgress _progress;

    public Client(Options options)
    {
        _options = options;
        _progress = options.Verbose ? new VerboseMergeProgress() : new DefaultMergeProgress();
    }

    public void Run()
    {
        ConsoleColor consoleColor = Console.ForegroundColor;
        try
        {
            PdfHelper helper = new PdfHelper(_options.Output, _options.Inputs)
                .SetCallback(_progress);
            if (_options.AddPageNumber)
            {
                helper.AddPageNumber(GetPageNumberOptions());
            }

            helper.Merge();
        }
        catch (PdfMergeException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.ForegroundColor = consoleColor;
        }
    }

    private PageNumberOptions GetPageNumberOptions()
    {
        PageNumberOptions pageOptions = new();
        if (_options.FontFamily != null)
        {
            pageOptions.FontFamily = _options.FontFamily;
        }

        if (_options.FontSize > 0)
        {
            pageOptions.FontSize = _options.FontSize;
        }

        if (_options.Location != null)
        {
            pageOptions.Location = _options.Location;
        }

        if (_options.Format != null)
        {
            pageOptions.Format = _options.Format;
        }

        return pageOptions;
    }

    private class VerboseMergeProgress : IMergeProgress
    {
        public bool OnMergeFile(string file)
        {
            Console.WriteLine($"Merging {file}...");
            return true;
        }

        public bool OnMergePage(int current, int total)
        {
            Console.WriteLine($"\tAdding page {current} of {total}");
            return true;
        }
    }
}