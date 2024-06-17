# MergePdf

> Copyright &copy; Tony's Studio 2024

---

This is a simple program to merge PDF files. It simply merges all files with no modification.

## How to use it?

Quite simple, you can specify files to merge, or merge all files under a directory.

```
Copyright (C) 2024 MergePdf.CLI

  -o, --output       Required. Output PDF file, will automatically add .pdf extension

  -r, --recursive    (Default: false) Find PDF files in given directory

  -i, --input        Required. Input PDF files or directory

  -n, --number       (Default: false) Add page number to each page

  -f, --font         (Default: Aria) Font name for page number

  -s, --size         (Default: 8) Font size for page number

  -l, --location     (Default: bm) Location of page number, tl, tm, tr, bl, bm, br

  --format           Format of page number, $current and $total will be replaced

  -v, --verbose      (Default: false) Print verbose log

  --help             Display this help screen.

  --version          Display version information.
```

The location of the page number stands for "top left", "top middle", etc.

For the `format` option, you may need to use `'` to quote it on Windows. For example, `'$current / $$total'` may be rendered as `2 / 5`.

Note that it will automatically add `.pdf` extension to the output filename.

For example, you can use the following command to merge all PDFs under .\PDF as `merged.pdf`.

```powershell
MergePdf.CLI.exe -o merged -i .\PDF
```

Based on that, you can add some extra arguments if you want to add page numbers on the bottom left of the PDF, with a larger font size and a custom format.

```powershell
MergePdf.CLI.exe -o merged -i .\PDF -n -l bl -s 18 --format '$current / $total'
```

## Building the project

The core library is `MergePdf.Core`, and `MergePdf.CLI` is the command-line client for it.

Just open the project in Visual Studio, and hit Build. I added some publish options to output final artifacts in the `Publish/` folder under solution.

## TODO

- [x] Command line client.
- [ ] GUI client using WPF.