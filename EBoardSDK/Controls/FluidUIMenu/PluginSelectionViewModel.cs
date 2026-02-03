// <copyright file="PluginSelectionViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Controls.FluidUIMenu;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PluginSelectionViewModel : ObservableObject
{
    private readonly IFluidUIContext fluidUI;

    private bool singleSelectionTarget;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginSelectionViewModel"/> class.
    /// </summary>
    /// <param name="fluidUIContext"></param>
    /// <param name="buttonClickAction"></param>
    /// <param name="singleSelectionTarget"></param>
    public PluginSelectionViewModel(IFluidUIContext fluidUIContext, Action? buttonClickAction, bool singleSelectionTarget)
    {
        this.fluidUI = fluidUIContext;
        this.buttonClickAction = buttonClickAction;
        this.singleSelectionTarget = singleSelectionTarget;

        this.SearchForPlugins();

        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.SingleSelectionTarget));
    }

    public IFluidUIContext FluidUI => this.fluidUI;

    public bool SingleSelectionTarget => this.singleSelectionTarget;

    public List<PluginRepresentationItem> FoundAddons { get; private set; } = [];

    public List<PluginRepresentationItem> FoundElements { get; private set; } = [];

    public List<PluginRepresentationItem> FoundShapes { get; private set; } = [];

    public List<PluginRepresentationItem> FoundAreas { get; private set; } = [];

    public List<PluginRepresentationItem> FoundTools { get; private set; } = [];

    public List<PluginRepresentationItem> FoundPlugins { get; private set; } = [];

    public List<PluginRepresentationItem> FoundEboard { get; private set; } = [];

    public List<PluginRepresentationItem> FoundSystem { get; private set; } = [];

    internal void SearchForPlugins()
    {
        var sDKPluginManager = new SDKPluginManager();

        this.FoundPlugins = sDKPluginManager.FoundPlugins;
        this.FoundAddons = sDKPluginManager.FoundAddons;
        this.FoundElements = sDKPluginManager.FoundElements;
        this.FoundShapes = sDKPluginManager.FoundShapes;
        this.FoundAreas = sDKPluginManager.FoundAreas;
        this.FoundTools = sDKPluginManager.FoundTools;

        this.FoundSystem = sDKPluginManager.FoundSystem;
        this.FoundEboard = sDKPluginManager.FoundEboard;
    }

    partial void OnSelectedPluginChanged(PluginRepresentationItem? value)
    {
        if (this.SelectionTarget == SelectionTargets.All || this.SelectionTarget == SelectionTargets.Title)
        {
            return;
        }

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
            case SelectionTargets.Eboard:
                this.SelectedPlugins = this.FoundEboard;
                break;
            case SelectionTargets.System:
                this.SelectedPlugins = this.FoundSystem;
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
        var found = this.FoundPlugins.Where(x => (x.PluginName != null && x.PluginName.Equals(value)) || (x.PluginHeader != null && x.PluginHeader.Equals(value))).ToList();

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