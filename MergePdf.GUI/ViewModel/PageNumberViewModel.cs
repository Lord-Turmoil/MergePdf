using System.Collections.ObjectModel;
using System.Windows.Media;
using MergePdf.GUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace MergePdf.GUI.ViewModel;

class PageNumberViewModel : BindableBase
{
    private ObservableCollection<OptionItem> _locations;
    private int _selectedLocationIndex = 4;
    private bool _addPageNumber;
    private string _format = "$current / $total";
    private string _fontSizeText = "8";
    private FontFamily _fontFamily = Fonts.SystemFontFamilies.First();

    public PageNumberViewModel()
    {
        _locations = [
            new OptionItem(0, "Top Left"),
            new OptionItem(1, "Top Center"),
            new OptionItem(2, "Top Right"),
            new OptionItem(3, "Bottom Left"),
            new OptionItem(4, "Bottom Center"),
            new OptionItem(5, "Bottom Right")
        ];

        ResetFormatCommand = new DelegateCommand(ResetFormat);
    }

    public ObservableCollection<OptionItem> Locations {
        set => SetProperty(ref _locations, value);
        get => _locations;
    }

    public int SelectedLocationIndex {
        set => SetProperty(ref _selectedLocationIndex, value);
        get => _selectedLocationIndex;
    }

    public bool AddPageNumber {
        set => SetProperty(ref _addPageNumber, value);
        get => _addPageNumber;
    }

    public string Format {
        set => SetProperty(ref _format, value);
        get => _format;
    }

    public string FontSizeText {
        set {
            if (string.IsNullOrEmpty(value))
            {
                value = "8";
            }

            if (int.TryParse(value, out int fontSize))
            {
                SetProperty(ref _fontSizeText, value);
                FontSize = fontSize;
            }
        }

        get => _fontSizeText;
    }

    public DelegateCommand ResetFormatCommand { get; }

    public string SelectedLocation {
        // get lowercase letter of two words
        get {
            string[] words = _locations[_selectedLocationIndex].Option.Split(' ');
            return words[0][..1].ToLower() + words[1][..1].ToLower();
        }
    }

    public FontFamily FontFamily {
        set => SetProperty(ref _fontFamily, value);
        get => _fontFamily;
    }

    public string FontFamilyName => _fontFamily.Source;

    public int FontSize { get; set; }


    private void ResetFormat()
    {
        Format = "$current / $total";
    }
}