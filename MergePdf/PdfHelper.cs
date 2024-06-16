using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace MergePdf;

static class PdfHelper
{
    public static void Merge(string output, string[] inputs)
    {
        if (inputs.Length == 0)
        {
            throw new Exception("No input files specified");
        }

        foreach (string input in inputs)
        {
            Console.WriteLine($"Merging {input}...");
            if (!File.Exists(input))
            {
                throw new Exception($"File {input} not found");
            }
        }

        using var document = new PdfDocument();
        foreach (var input in inputs)
        {
            using var inputDocument = PdfReader.Open(input, PdfDocumentOpenMode.Import);
            for (var i = 0; i < inputDocument.PageCount; i++)
            {
                var page = inputDocument.Pages[i];
                document.AddPage(page);
            }
        }
        document.Save(output);
    }
}