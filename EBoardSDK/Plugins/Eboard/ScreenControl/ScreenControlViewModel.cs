// <copyright file="ScreenControlViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.ScreenControl;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIDataBlock;
using EBoardSDK.Controls.FluidUIDesign;
using EBoardSDK.Controls.FluidUIFont;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Controls.FluidUISize;
using EBoardSDK.Controls.FluidUIStand;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

public partial class ScreenControlViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "ScreenControl";
    private readonly string pluginHeader = "ScreenControl";

    private ScreenViewModel? screenViewModel;

    private FluidUISelectionViewModel fluidUISelectionViewModel;
    private PluginSelectionViewModel pluginSelectionViewModel;

    private FluidUIDataBlockSetupViewModel fluidUIDataBlockSetupViewModel;
    private FluidUIDesignSetupViewModel fluidUIDesignSetupViewModel;
    private FluidUIFontSetupViewModel fluidUIFontSetupViewModel;
    private FluidUISizeSetupViewModel fluidUISizeSetupViewModel;
    private FluidUIStandSetupViewModel fluidUIStandSetupViewModel;

    [ObservableProperty]
    private int arrangementOffsetValue;

    [ObservableProperty]
    private int arrangementRotationValue;

    [ObservableProperty]
    private string titleString = "title";

    [ObservableProperty]
    private bool isTitleSelected = false;

    [ObservableProperty]
    private SelectionTargets selectionTarget;

    [ObservableProperty]
    private PluginBaseViewModel? selectedPlugin;

    [ObservableProperty]
    private List<PluginBaseViewModel>? selectedPlugins;

    private ArrangeSelectedElements arrangeSelectedElements;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenControlViewModel"/> class.
    /// </summary>
    public ScreenControlViewModel()
    {
        this.instantiatedAsElement = false;
    }

    public ScreenViewModel? ScreenViewModel => this.screenViewModel;

    public FluidUISelectionViewModel FluidUISelectionViewModel => this.fluidUISelectionViewModel;

    public PluginSelectionViewModel PluginSelectionViewModel => this.pluginSelectionViewModel;

    public FluidUIDataBlockSetupViewModel FluidUIDataBlockSetupViewModel => this.fluidUIDataBlockSetupViewModel;

    public FluidUIDesignSetupViewModel FluidUIDesignSetupViewModel => this.fluidUIDesignSetupViewModel;

    public FluidUIFontSetupViewModel FluidUIFontSetupViewModel => this.fluidUIFontSetupViewModel;

    public FluidUISizeSetupViewModel FluidUISizeSetupViewModel => this.fluidUISizeSetupViewModel;

    public FluidUIStandSetupViewModel FluidUIStandSetupViewModel => this.fluidUIStandSetupViewModel;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new ();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(ScreenControlViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.instantiatedAsElement = true;

            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);
        }

        this.Setup();
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Load(string path)"));
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Save(string path)"));
    }

    /// <inheritdoc/>
    protected override void Setup()
    {
        if (this.MainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.screenViewModel = this.MainViewModel.NavigationContextViewModel.EboardBrowserViewModel.SelectedEboard;

            this.OnPropertyChanged(nameof(this.ScreenViewModel));
        }

        if (this.ScreenViewModel != null)
        {
            this.arrangeSelectedElements = new ArrangeSelectedElements();
            this.fluidUISelectionViewModel = new FluidUISelectionViewModel(this.ScreenViewModel, null);
            this.pluginSelectionViewModel = new PluginSelectionViewModel(this.ScreenViewModel.FluidUI, null, singleSelectionTarget: false);

            // TODO neues uebergeordnetes ViewModel bauen fuer die ganzen ViewModels
            this.fluidUIDataBlockSetupViewModel = new FluidUIDataBlockSetupViewModel(this.ScreenViewModel, FluidUIStandSettings.ElementContextArea, templateCreation: true);
            this.fluidUIDesignSetupViewModel = new FluidUIDesignSetupViewModel(this.ScreenViewModel);
            this.fluidUIFontSetupViewModel = new FluidUIFontSetupViewModel(this.ScreenViewModel);
            this.fluidUISizeSetupViewModel = new FluidUISizeSetupViewModel(this.ScreenViewModel);
            this.fluidUIStandSetupViewModel = new FluidUIStandSetupViewModel(this.ScreenViewModel, FluidUIStandSettings.ElementContextArea, this.ScreenViewModel);

            this.OnPropertyChanged(nameof(this.FluidUISelectionViewModel));
            this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
        }
    }

    // in anderes Modul auslagern, hier einstellen, anderswo das arrangement von selektionen setzen
    // da koennte ich hier alle Settings reinpacken, also auch aus dem eboardbrowser ggf raus kopieren
    [RelayCommand]
    private void ArrangeGroupAsLine()
    {
        if (this.ScreenViewModel != null)
        {
        this.arrangeSelectedElements.ArrangeGroupAsLine(this.ScreenViewModel);
        }
    }

    [RelayCommand]
    private void ArrangeGroupAsSquare()
    {
        if (this.ScreenViewModel != null)
        {
        this.arrangeSelectedElements.ArrangeGroupAsSquare(this.ScreenViewModel);
        }
    }

    [RelayCommand]
    private void ArrangeGroupAsRandomMatrix10x10()
    {
        if (this.ScreenViewModel != null)
        {
        this.arrangeSelectedElements.ArrangeGroupAsRandomMatrix10x10(this.ScreenViewModel);
        }
    }

    [RelayCommand]
    private void Clear() // clear selected
    {
        this.ScreenViewModel?.ClearSelectionTargets(this.SelectionTarget, titleString: this.TitleString, pluginName: this.SelectedPlugin?.Name);
    }

    /// <summary>
    /// Copies deep copies of selected elements to MainViewModel.ElementCopyList.
    /// </summary>
    [RelayCommand]
    private void Copy() // copy for paste on another screen
    {
        this.ScreenViewModel?.CopySelectedElements();
    }

    /// <summary>
    /// Creates deep copies of selected elements and adds them to the
    /// active screen Element list.
    /// </summary>
    [RelayCommand]
    private void Duplicate() // duplicate on screen
    {
        this.ScreenViewModel?.DuplicateSelectedElements();
    }

    /// <summary>
    /// Copies deep copies of selected elements to MainViewModel.ElementCopyList,
    /// deletes selected elements from the active screen Element list.
    /// </summary>
    [RelayCommand]
    private void Move()
    {
        this.ScreenViewModel?.CopyMoveSelectedElements();
    }

    [RelayCommand]
    private void ApplyConfigurationOnSelected()
    {
        this.ScreenViewModel?.ApplyFluidUIConfigOnSelectedElements(this.FluidUISelectionViewModel);
    }

    [RelayCommand]
    private void LoadConfigurationOnSelected()
    {
        this.ScreenViewModel?.LoadFluidUIConfigOnSelectedElements(this.FluidUISelectionViewModel);
    }

    /// <summary>
    /// Paste content of MainViewModel.ElementCopyList
    /// into the active screen instance Element list.
    /// Clears MainViewModel.ElementCopyList.
    /// </summary>
    [RelayCommand]
    private void Paste()
    {
        this.ScreenViewModel?.PasteToScreen();
    }

    [RelayCommand]
    private void ResetConfigurationOnSelected()
    {
        this.ScreenViewModel?.ResetSelectedFluidUI(this.FluidUISelectionViewModel);
    }

    [RelayCommand]
    private void Select()
    {
        this.ScreenViewModel?.SelectSelectionTargets(
            this.pluginSelectionViewModel.SelectionTarget,
            titleString: this.pluginSelectionViewModel.TitleString,
            pluginName: this.pluginSelectionViewModel.SelectedPlugin?.PluginName);
    }
}

// EOF