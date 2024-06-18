using System.Collections.ObjectModel;
using System.Diagnostics;
using MergePdf.Core;
using MergePdf.GUI.Extensions;
using MergePdf.GUI.Models;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MergePdf.GUI.ViewModel;

class MainViewModel : BindableBase
{
    private readonly IContainerProvider _containerProvider;
    private readonly IRegionManager _regionManager;

    private readonly object _mutex = new();
    private ObservableCollection<MenuTab> _menuTabs;
    private string _output;
    private bool _isMerging;

    private bool _openOnComplete;

    public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
    {
        _containerProvider = containerProvider;
        _regionManager = regionManager;

        _menuTabs = new ObservableCollection<MenuTab>();
        CreateMenuTabs();

        NavigateCommand = new DelegateCommand<MenuTab>(Navigate);

        _output = string.Empty;
    }

    public ObservableCollection<MenuTab> MenuTabs {
        set => SetProperty(ref _menuTabs, value);
        get => _menuTabs;
    }

    public string Output {
        set => SetProperty(ref _output, value);
        get => _output;
    }

    public bool OpenOnComplete {
        set => SetProperty(ref _openOnComplete, value);
        get => _openOnComplete;
    }

    public DelegateCommand<MenuTab> NavigateCommand { get; }
    public DelegateCommand LoadedCommand => new(() => { NavigateCommand.Execute(MenuTabs[0]); });
    public DelegateCommand ClearCommand => new(() => Output = string.Empty);
    public DelegateCommand MergeCommand => new(Merge);

    private void CreateMenuTabs()
    {
        MenuTabs.Add(new MenuTab("Files", "FilesView", "Select input and output files"));
        MenuTabs.Add(new MenuTab("Page Number", "PageNumberView", "Add and configure page number style"));
    }

    private void Navigate(MenuTab tab)
    {
        if (string.IsNullOrWhiteSpace(tab.NameSpace))
        {
            return;
        }

        if (_regionManager.Regions.ContainsRegionWithName(PrismManager.MainViewRegionName))
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(tab.NameSpace);

            // Output += $"Navigated to {tab.Title}\n";
        }
    }

    private void Merge()
    {
        lock (_mutex)
        {
            if (_isMerging)
            {
                Output += "Already merging files...\n";
                return;
            }

            _isMerging = true;
        }

        Output += "Merging files...\n";
        var fileViewModel = _containerProvider.Resolve<FilesViewModel>();
        var helper = new PdfHelper(fileViewModel.Output.Path, fileViewModel.Files.Select(file => file.Path));
        var pageNumberViewModel = _containerProvider.Resolve<PageNumberViewModel>();
        if (pageNumberViewModel.AddPageNumber)
        {
            var options = new PageNumberOptions {
                Location = pageNumberViewModel.SelectedLocation,
                FontFamily = pageNumberViewModel.SelectedFontFamily,
                FontSize = pageNumberViewModel.FontSize,
                Format = pageNumberViewModel.Format
            };
            helper.AddPageNumber(options);
        }

        helper.SetCallback(new MergeProgress(this));
        MergeImpl(helper);
    }

    private async void MergeImpl(PdfHelper helper)
    {
        try
        {
            await Task.Run(helper.Merge);
            Output += "Merge complete!\n";
            if (OpenOnComplete)
            {
                var path = _containerProvider.Resolve<FilesViewModel>().Output.Path;
                Process.Start("explorer.exe", $"/select,\"{path}\"");
                Output += $"Revealing {path} in explorer";
            }
        }
        catch (PdfMergeException e)
        {
            Output += $"Failed to merge: {e.Message}\n";
        }
        catch (Exception e)
        {
            Output += $"An error occurred: {e.Message}\n";
        }
        finally
        {
            lock (_mutex)
            {
                _isMerging = false;
            }
        }
    }

    private class MergeProgress : IMergeProgress
    {
        private readonly MainViewModel _viewModel;

        public MergeProgress(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        public bool OnMergeFile(string file)
        {
            _viewModel.Output += $"Merging file: {file}\n";
            return true;
        }

        public bool OnMergePage(int current, int total)
        {
            _viewModel.Output += $"\tMerging page {current} of {total}\n";
            return true;
        }
    }
}