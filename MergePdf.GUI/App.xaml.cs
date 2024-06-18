﻿using MergePdf.GUI.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;
using MergePdf.GUI.ViewModel;

namespace MergePdf.GUI;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication
{
    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<MainViewModel>();
        containerRegistry.RegisterSingleton<FilesViewModel>();
        containerRegistry.RegisterSingleton<PageNumberViewModel>();
        containerRegistry.RegisterForNavigation<MainView, MainViewModel>();
        containerRegistry.RegisterForNavigation<FilesView, FilesViewModel>();
        containerRegistry.RegisterForNavigation<PageNumberView, PageNumberViewModel>();
    }

    protected override Window CreateShell()
    {
        return Container.Resolve<MainView>();
    }
}