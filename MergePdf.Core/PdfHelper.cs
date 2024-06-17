// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace MergePdf.Core;

public class PdfHelper
{
    private readonly string _output;
    private readonly List<string> _inputs;
    private PageNumberOptions? _options;
    private IMergeProgress _progress = new DefaultMergeProgress();

    public PdfHelper(string output, IEnumerable<string> inputs)
    {
        _output = output;
        _inputs = new List<string>(inputs);
    }

    public PdfHelper AddPageNumber(PageNumberOptions options)
    {
        _options = options;
        return this;
    }

    public PdfHelper SetCallback(IMergeProgress progress)
    {
        _progress = progress;
        return this;
    }

    public void Merge()
    {
        if (!_inputs.Any())
        {
            throw new PdfMergeException("No input files specified");
        }

        _options?.Validate();

        foreach (string input in _inputs)
        {
            if (!File.Exists(input))
            {
                throw new PdfMergeException($"File {input} not found");
            }
        }

        // Count total pages.
        int total = 0;
        foreach (string input in _inputs)
        {
            using PdfDocument inputDocument = PdfReader.Open(input, PdfDocumentOpenMode.Import);
            total += inputDocument.PageCount;
        }

        int current = 0;
        using var document = new PdfDocument();
        foreach (string input in _inputs)
        {
            if (_progress.OnMergeFile(input))
            {
                using PdfDocument inputDocument = PdfReader.Open(input, PdfDocumentOpenMode.Import);
                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    current++;
                    if (_progress.OnMergePage(current, total))
                    {
                        PdfPage page = inputDocument.Pages[i];
                        PostAddPage(document.AddPage(page), current, total);
                    }
                }
            }

        }

        document.Save(_output);
    }

    private void PostAddPage(PdfPage page, int current, int total)
    {
        if (_options == null)
        {
            return;
        }

        XGraphics gfx = XGraphics.FromPdfPage(page);
        var font = new XFont(_options.FontFamily, _options.FontSize);
        string pageNumber = _options.Format.Replace(PageNumberOptions.ValueCurrent, current.ToString())
            .Replace(PageNumberOptions.ValueTotal, total.ToString());

        XRect rect;
        switch (_options.Location)
        {
            case PageNumberOptions.TopLeft:
                rect = new XRect(10, 10, page.Width, page.Height);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                break;
            case PageNumberOptions.TopMiddle:
                rect = new XRect(0, 10, page.Width, page.Height);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.TopCenter);
                break;
            case PageNumberOptions.TopRight:
                rect = new XRect(0, 10, page.Width - 10, page.Height);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.TopRight);
                break;
            case PageNumberOptions.BottomLeft:
                rect = new XRect(10, 0, page.Width, page.Height - 10);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.BottomLeft);
                break;
            case PageNumberOptions.BottomMiddle:
                rect = new XRect(0, 0, page.Width, page.Height - 10);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.BottomCenter);
                break;
            case PageNumberOptions.BottomRight:
                rect = new XRect(0, 0, page.Width - 10, page.Height - 10);
                gfx.DrawString(pageNumber, font, XBrushes.Black, rect, XStringFormats.BottomRight);
                break;
        }
    }
}