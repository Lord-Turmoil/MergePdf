// Copyright (C) 2018 - 2024 Tony's Studio. All rights reserved.

using System.Collections.ObjectModel;
using System.Text;
using MergePdf.GUI.Models;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;

namespace MergePdf.GUI.ViewModel;

class FilesViewModel : BindableBase
{
    private readonly IContainerProvider _containerProvider;
    private ObservableCollection<FileItem> _files;
    private FileItem _output;

    public FilesViewModel(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        SelectInputFilesCommand = new DelegateCommand(SelectInputFiles);
        ClearInputFilesCommand = new DelegateCommand(ClearInputFiles);
        RemoveInputFileCommand = new DelegateCommand<FileItem>(RemoveInputFile);
        SelectOutputFileCommand = new DelegateCommand(SelectOutputFile);
        ClearOutputFileCommand = new DelegateCommand(ClearOutputFile);
        _files = new ObservableCollection<FileItem>();
        _output = new FileItem("", "");
    }

    public DelegateCommand SelectInputFilesCommand { get; }
    public DelegateCommand ClearInputFilesCommand { get; }
    public DelegateCommand<FileItem> RemoveInputFileCommand { get; }
    public DelegateCommand SelectOutputFileCommand { get; }
    public DelegateCommand ClearOutputFileCommand { get; }

    public ObservableCollection<FileItem> Files {
        set => SetProperty(ref _files, value);
        get => _files;
    }

    public FileItem Output {
        set => SetProperty(ref _output, value);
        get => _output;
    }

    private void SelectInputFiles()
    {
        var fileDialog = new OpenFileDialog {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Select PDF file(s)...",
            Multiselect = true
        };
        bool success = (bool)fileDialog.ShowDialog()!;
        StringBuilder builder = new();
        if (success)
        {
            builder.AppendLine("Added file(s): ");
            string[] paths = fileDialog.FileNames;
            string[] fileNames = fileDialog.SafeFileNames;

            for (int i = 0; i < paths.Length; i++)
            {
                if (Files.All(file => file.Path != paths[i]))
                {
                    Files.Add(new FileItem(paths[i], fileNames[i]));
                    builder.Append('\t').AppendLine(paths[i]);
                }
            }
        }
        else
        {
            builder.AppendLine("No files selected");
        }

        _containerProvider.Resolve<MainViewModel>().Output += builder.ToString();
    }

    private void ClearInputFiles()
    {
        Files.Clear();
        _containerProvider.Resolve<MainViewModel>().Output += "Input files cleared\n";
    }

    private void RemoveInputFile(FileItem item)
    {
        if (Files.Remove(item))
        {
            _containerProvider.Resolve<MainViewModel>().Output += $"Removed file: {item.Path}\n";
        }
    }

    private void SelectOutputFile()
    {
        var fileDialog = new SaveFileDialog {
            Filter = "PDF Files (*.pdf)|*.pdf",
            Title = "Select output file..."
        };
        bool success = (bool)fileDialog.ShowDialog()!;
        if (success)
        {
            Output = new FileItem(fileDialog.FileName, fileDialog.SafeFileName);
            _containerProvider.Resolve<MainViewModel>().Output += $"Output file: {Output.Path}\n";
        }
        else
        {
            _containerProvider.Resolve<MainViewModel>().Output += "No output file selected\n";
        }
    }

    private void ClearOutputFile()
    {
        Output = new FileItem("", "");
        _containerProvider.Resolve<MainViewModel>().Output += "Output file cleared\n";
    }
}