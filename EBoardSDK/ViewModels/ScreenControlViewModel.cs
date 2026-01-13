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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using EBoardSDK.Utilities;

public partial class ScreenControlViewModel : ObservableObject // so machen das die Settings für das Eboard auch die Oberfläche der SettingsView definieren.
{
    private FluidUISelectionViewModel fluidUISelectionViewModel;
    private PluginSelectionViewModel pluginSelectionViewModel;

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
    private EBoardElementPluginBaseViewModel? selectedPlugin;

    [ObservableProperty]
    private List<EBoardElementPluginBaseViewModel>? selectedPlugins;

    private EBoardViewModel viewModel;

    private ArrangeSelectedElements arrangeSelectedElements;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenControlViewModel"/> class.
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    public ScreenControlViewModel(EBoardViewModel eBoardViewModel)
    {
        this.viewModel = eBoardViewModel;
        this.arrangeSelectedElements = new ArrangeSelectedElements();
        this.fluidUISelectionViewModel = new FluidUISelectionViewModel(this.ViewModel, null);
        this.pluginSelectionViewModel = new PluginSelectionViewModel(this.ViewModel, null);

        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FluidUISelectionViewModel));
        this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
    }

    public FluidUISelectionViewModel FluidUISelectionViewModel => this.fluidUISelectionViewModel;

    public PluginSelectionViewModel PluginSelectionViewModel => this.pluginSelectionViewModel;

    public EBoardViewModel ViewModel => this.viewModel;

    // in anderes Modul auslagern, hier einstellen, anderswo das arrangement von selektionen setzen
    // da könnte ich hier alle Settings reinpacken, also auch aus dem eboardbrowser ggf raus kopieren
    [RelayCommand]
    private void ArrangeGroupAsLine()
    {
        this.arrangeSelectedElements.ArrangeGroupAsLine(this.ViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsSquare()
    {
        this.arrangeSelectedElements.ArrangeGroupAsSquare(this.ViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsRandomMatrix10x10()
    {
        this.arrangeSelectedElements.ArrangeGroupAsRandomMatrix10x10(this.ViewModel);
    }

    [RelayCommand]
    private void Clear() // clear selected
    {
        this.ViewModel.ClearSelectionTargets(this.SelectionTarget, titleString: this.TitleString, pluginName: this.SelectedPlugin?.PluginName);
    }

    //[RelayCommand]
    //private void Copy() // copy for paste on another screen
    //{
    //    this.ViewModel.CopySelected();
    //}

    //[RelayCommand]
    //private void Duplicate() // duplicate on screen
    //{
    //    this.ViewModel.DuplicateSelected();
    //}

    //[RelayCommand]
    //private void Move() // move to another screen, delete on origin screen?
    //{
    //    this.ViewModel.MoveSelected();
    //}

    [RelayCommand]
    private void LoadConfigurationOnSelected()
    {
        this.ViewModel.LoadFluidUIConfigOnSelectedElements(this.FluidUISelectionViewModel.ConfigurationTarget);
    }

    [RelayCommand]
    private void ResetSelected()
    {
        this.ViewModel.ResetSelectedFluidUI(this.FluidUISelectionViewModel.ConfigurationTarget);
    }

    [RelayCommand]
    private void Select()
    {
        this.ViewModel.SelectSelectionTargets(
            this.pluginSelectionViewModel.SelectionTarget,
            titleString: this.pluginSelectionViewModel.TitleString,
            pluginName: this.pluginSelectionViewModel.SelectedPlugin?.PluginName);
    }
}

// EOF