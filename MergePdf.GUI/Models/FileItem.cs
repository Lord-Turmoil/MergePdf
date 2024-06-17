namespace MergePdf.GUI.Models;

class FileItem
{
    public FileItem(string path, string filename)
    {
        Path = path;
        Filename = filename;
    }

    public string Path { get; }
    public string Filename { get; }

    public override bool Equals(object? obj)
    {
        return obj is FileItem item &&
               Path == item.Path &&
               Filename == item.Filename;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Path, Filename);
    }
}