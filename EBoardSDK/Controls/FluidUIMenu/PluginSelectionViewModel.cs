// <copyright file="PluginSelectionViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Controls.FluidUIMenu;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class PluginSelectionViewModel : ObservableObject
{
    private readonly EboardFluidUIBaseViewModel viewModel;

    private Action? buttonClickAction;

    [ObservableProperty]
    private string titleString = "name";

    [ObservableProperty]
    private bool isTitleSelected = false;

    [ObservableProperty]
    private SelectionTargets selectionTarget;

    [ObservableProperty]
    private PluginRepresentationItem? selectedPlugin;

    [ObservableProperty]
    private List<PluginRepresentationItem>? selectedPlugins;

    public PluginSelectionViewModel(EboardFluidUIBaseViewModel viewModel, Action? buttonClickAction)
    {
        this.viewModel = viewModel;
        this.buttonClickAction = buttonClickAction;

        this.SearchForPlugins();

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public List<PluginRepresentationItem> FoundAddons { get; private set; } = [];

    public List<PluginRepresentationItem> FoundElements { get; private set; } = [];

    public List<PluginRepresentationItem> FoundShapes { get; private set; } = [];

    public List<PluginRepresentationItem> FoundAreas { get; private set; } = [];

    public List<PluginRepresentationItem> FoundTools { get; private set; } = [];

    public List<PluginRepresentationItem> FoundPlugins { get; private set; } = [];

    internal void SearchForPlugins()
    {
        var sDKPluginManager = new SDKPluginManager();

        this.FoundPlugins = sDKPluginManager.FoundPlugins;
        this.FoundAddons = sDKPluginManager.FoundAddons;
        this.FoundElements = sDKPluginManager.FoundElements;
        this.FoundShapes = sDKPluginManager.FoundShapes;
        this.FoundAreas = sDKPluginManager.FoundAreas;
        this.FoundTools = sDKPluginManager.FoundTools;
    }

    partial void OnSelectedPluginChanged(PluginRepresentationItem? value)
    {
        if (this.SelectionTarget == SelectionTargets.All || this.SelectionTarget == SelectionTargets.Title)
        {
            return;
        }

        this.SelectedPlugins = [this.SelectedPlugin];

        this.buttonClickAction?.Invoke();
    }

    partial void OnSelectionTargetChanged(SelectionTargets value)
    {
        this.SelectedPlugin = null!;
        this.IsTitleSelected = false;

        switch (value)
        {
            case SelectionTargets.All:
                this.SelectedPlugins = null!;
                break;
            case SelectionTargets.Addons:
                this.SelectedPlugins = this.FoundAddons;
                break;
            case SelectionTargets.Elements:
                this.SelectedPlugins = this.FoundElements;
                break;
            case SelectionTargets.Shapes:
                this.SelectedPlugins = this.FoundShapes;
                break;
            case SelectionTargets.Areas:
                this.SelectedPlugins = this.FoundAreas;
                break;
            case SelectionTargets.Tools:
                this.SelectedPlugins = this.FoundTools;
                break;
            case SelectionTargets.Specific:
                this.SelectedPlugins = this.FoundPlugins;
                break;
            case SelectionTargets.Title:
                this.IsTitleSelected = true;
                break;
            default:
                break;
        }

        this.buttonClickAction?.Invoke();
    }

    partial void OnTitleStringChanged(string value)
    {
        var found = this.FoundPlugins.Where(x => x.PluginName.Equals(value) || x.PluginHeader.Equals(value)).ToList();

        if (found != null && found.Count > 0)
        {
            this.SelectedPlugins = found;
        }
        else
        {
            this.titleString = $"{value} not found";
        }

        this.buttonClickAction?.Invoke();
    }
}

// EOF