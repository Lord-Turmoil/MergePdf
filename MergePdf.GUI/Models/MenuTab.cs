namespace MergePdf.GUI.Models;

class MenuTab
{
    public MenuTab(string title, string nameSpace, string description)
    {
        Title = title;
        NameSpace = nameSpace;
        Description = description;
    }

    public string Title { get; private set; }
    public string NameSpace { get; private set; }
    public string Description { get; private set; }
}
