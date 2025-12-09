// <copyright file="FSNMainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// thanks to https://gargmanoj.wordpress.com/2009/03/27/wpf-101-a-simple-windows-explorer-like-directory-browser/ for the tutorial.
// this project mainly followed the tutorial steps, while aiming to integrate the solution into eboard.

namespace EEP_FSNavigator.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EEP_FSNavigator.Models;
using EEP_FSNavigator.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

public partial class FSNMainViewModel : EBoardSDK.Plugins.EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "File System Navigator";
    private string pluginName = "FSNavigator";

    private IList<FSNDirectoryInfo> currentItems;

    [ObservableProperty]
    private FSNDirectoryInfo currentDirectory;

    [ObservableProperty]
    private FSNFileSystemTreeViewModel fileSystemTreeVM;

    [ObservableProperty]
    private FSNDirectoryDetailsViewModel directoryDetailsVM;

    [ObservableProperty]
    private bool showDirectoryTree = true;

    public FSNMainViewModel()
    {
        this.BorderManagement = new BorderManagement() { Width = 450, Height = 225 };
        this.BrushManagement = new BrushManagement();

        this.ElementScreenIntegrationConstraints = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.Unconstrained);

        //this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

        this.FileSystemTreeVM = new FSNFileSystemTreeViewModel(this);
        this.DirectoryDetailsVM = new FSNDirectoryDetailsViewModel(this);

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Interval = TimeSpan.FromTicks(1);
        dispatcherTimer.Tick += DispatcherTimer_Tick;
        dispatcherTimer.Start();
    }

    private void DispatcherTimer_Tick(object? sender, EventArgs e)
    {
        if (this.BorderManagement.Width.Equals(double.NaN))
        {
            this.BorderManagement.Width = 450;
        }

        if (this.BorderManagement.Height.Equals(double.NaN))
        {
            this.BorderManagement.Height = 225;
        }

        ApplyRedraw();

        var dispatcherTimer = sender as DispatcherTimer;
        if (dispatcherTimer != null)
        {
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            dispatcherTimer.Stop();
        }
    }

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }


    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "FileSystemNavigator";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary? ResourceDictionary => new() { Source = new Uri("/EEP_FSNavigator;component/DefaultResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(FSNMainView);

    public override Type ElementPluginViewModel => typeof(FSNMainViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    public IList<FSNDirectoryInfo> CurrentItems
    {
        get
        {
            if (this.currentItems == null)
            {
                this.currentItems = new List<FSNDirectoryInfo>();
            }

            return this.currentItems;
        }

        set
        {
            this.currentItems = value;
            this.OnPropertyChanged(nameof(this.CurrentItems));
        }
    }

    [RelayCommand]
    private void ShowTree()
    {
        this.DirectoryTreeHideHandler();
    }

    partial void OnCurrentDirectoryChanged(FSNDirectoryInfo value)
    {
        this.RefreshCurrentItems();
    }

    private void DirectoryTreeHideHandler()
    {
        this.ShowDirectoryTree = false;
    }

    protected void RefreshCurrentItems()
    {
        IList<FSNDirectoryInfo> childDirectoriesList = new List<FSNDirectoryInfo>();
        IList<FSNDirectoryInfo> childFilesList = new List<FSNDirectoryInfo>();

        if (this.CurrentDirectory.Name.Equals(FSNResource.MyComputer))
        {
            childDirectoriesList = (from rd in FSNScanService.GetRootDirectories()
                                    select new FSNDirectoryInfo(rd)).ToList();
        }
        else
        {
            childDirectoriesList = (from dir in FSNScanService.GetChildDirectories(this.CurrentDirectory.Path)
                                    select new FSNDirectoryInfo(dir)).ToList();

            childFilesList = (from file in FSNScanService.GetChildFiles(this.CurrentDirectory.Path)
                              select new FSNDirectoryInfo(file)).ToList();
        }

        this.CurrentItems = childDirectoriesList;

        if (this.DirectoryDetailsVM != null && this.FileSystemTreeVM != null)
        {
            if (this.FileSystemTreeVM.CurrentTreeItem != null)
            {
                this.DirectoryDetailsVM.CurrentItem = this.FileSystemTreeVM.CurrentTreeItem;
            }
        }
    }
}

// EOF