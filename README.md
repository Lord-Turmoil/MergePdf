# MergePdf

> Copyright &copy; Tony's Studio 2024

---

This is a simple program to merge PDF files. It simply merges all files with no modification.

## How to use it?

Quite simple, you can specify files to merge, or merge all files under a directory.

```powershell
> MergePdf.exe <output> <input1> <input2> ...
> MergePdf.exe <output> -r <input>
```

Or you can run the program without arguments to set them interactively.

```powershell
> MergePdf.exe
Output filename: output
Recursive? (y/n): y
Input directory: ./
```

Note that it will automatically add `.pdf` extension to the output filename.

## Building the project

Just open the project in Visual Studio, and hit Build. I add a publish option, and will output final artifact in `Publish/` folder under solution.

