// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

using System.Collections.ObjectModel;
using MergePdf.GUI.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace MergePdf.GUI.ViewModel;

class PageNumberViewModel : BindableBase
{
    private ObservableCollection<OptionItem> _locations;
    private int _selectedLocationIndex = 4;
    private ObservableCollection<OptionItem> _fontFamilies;
    private int _selectedFontFamilyIndex;

    private bool _addPageNumber;
    private string _format = "$current / $total";
    private string _fontSizeText = "8";

    public PageNumberViewModel()
    {
        _locations = new ObservableCollection<OptionItem>();
        AddLocations();
        _fontFamilies = new ObservableCollection<OptionItem>();
        AddFontFamilies();
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

    public ObservableCollection<OptionItem> FontFamilies {
        set => SetProperty(ref _fontFamilies, value);
        get => _fontFamilies;
    }

    public int SelectedFontFamilyIndex {
        set => SetProperty(ref _selectedFontFamilyIndex, value);
        get => _selectedFontFamilyIndex;
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

    public string SelectedFontFamily => _fontFamilies[_selectedFontFamilyIndex].Option;

    public int FontSize { get; private set; } = 8;

    private void AddLocations()
    {
        Locations.Add(new OptionItem(0, "Top Left"));
        Locations.Add(new OptionItem(1, "Top Center"));
        Locations.Add(new OptionItem(2, "Top Right"));
        Locations.Add(new OptionItem(3, "Bottom Left"));
        Locations.Add(new OptionItem(4, "Bottom Center"));
        Locations.Add(new OptionItem(5, "Bottom Right"));
    }

    private void AddFontFamilies()
    {
        FontFamilies.Add(new OptionItem(0, "Arial"));
        FontFamilies.Add(new OptionItem(1, "Times New Roman"));
        FontFamilies.Add(new OptionItem(2, "Courier New"));
        FontFamilies.Add(new OptionItem(3, "Verdana"));
        FontFamilies.Add(new OptionItem(4, "Lucida Console"));
        FontFamilies.Add(new OptionItem(5, "Symbol"));
    }

    private void ResetFormat()
    {
        Format = "$current / $total";
    }
}