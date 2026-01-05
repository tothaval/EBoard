// <copyright file="MainViewModel.cs" company=".">
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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainViewModel
 *
 *  view model for MainWindow
 */
namespace EBoardSDK;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Models;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;

public partial class MainViewModel : EboardFluidUIBaseViewModel
{
    private DataLocations _dataLocations;

    [ObservableProperty]
    private EBoardBrowserViewModel eBoardBrowserViewModel;

    private EBoardSDK.Models.EboardConfig eboardConfig;

    [ObservableProperty]
    private MainWindowLogoutBarViewModel mainWindowLogoutBarVM;

    [ObservableProperty]
    private MainWindowMenuBarViewModel mainWindowMenuBarVM;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private bool mainControlsVisible = true;

    private readonly Runner runner;

    public MainViewModel(EBoardSDK.Models.EboardConfig eboardConfig, DataLocations dataLocations, Runner runner)
        : base()
    {
        this.runner = runner;

        this._dataLocations = dataLocations;

        this.eboardConfig = eboardConfig;
        this.eboardConfig?.EBoardContext?.Design?.LoadBrushesFromColorData();

        this.SetFluidUI(this.eboardConfig.EBoardContext);

        var helper = new SharedMethod_UI();
        helper.SetupTitleAndText(
            this.FluidUI.DataBlock,
            "Eboard",
            "the eboard main window\n\nyou can change this description");

        this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, fluidUIContextHasStand: false));

        this.EBoardBrowserViewModel = new EBoardBrowserViewModel(this);

        this.MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this, eboardConfig);
        this.MainWindowLogoutBarVM = new MainWindowLogoutBarViewModel(this.EBoardBrowserViewModel.FluidUI.Design, this);

        this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public EboardConfig EBoardConfig => this.eboardConfig;

    public void DeselectElements()
    {
        if (this.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            this.EBoardBrowserViewModel.SelectedEBoard.DeselectElements();
        }
    }

    public EBoardSDK.Models.EboardConfig GetEboardConfig()
    {
        this.eboardConfig.EBoardIndex = this.EBoardBrowserViewModel.CurrentSelectionID;
        this.eboardConfig.EBoardCount = this.EBoardBrowserViewModel.EBoardCount;
        this.eboardConfig.EBoardBrowserSwitch = this.MainWindowMenuBarVM.EBoardBrowserSwitch;

        this.eboardConfig.EBoardContext = (FluidUIContext)this.FluidUI;

        this.eboardConfig.EBoardBrowserViewContext = (FluidUIContext)this.EBoardBrowserViewModel.FluidUI;

        return this.eboardConfig;
    }

    public Runner GetRunnerInstance()
    {
        return this.runner;
    }

    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        var screenDataList = this.EBoardBrowserViewModel.GetScreenData();

        return screenDataList;
    }

    public EBoardTaskResult SetScreenData(IList<EBoardSDK.Models.EboardScreen> eboardScreens)
    {
        eboardScreens.Select(x => x).ToList().ForEach(
            escreen =>
            {
                var eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(escreen, this);

                if (this.eboardConfig.EBoardIndex > this.eboardConfig.EBoardCount)
                {
                    this.eboardConfig.EBoardIndex = 0;
                }

                if (escreen.ID == this.eboardConfig.EBoardIndex)
                {
                    this.EBoardBrowserViewModel.SelectedEBoard = eBoardViewModel;
                }

                this.EBoardBrowserViewModel.AddEBoardViewModel(eBoardViewModel);
            });

        return EBoardTaskResult.Success;
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    [RelayCommand]
    private void FirstEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("First");
    }

    [RelayCommand]
    private void LastEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Last");
    }

    [RelayCommand]
    private void MainControlsVisibilityChange()
    {
        if (this.MainControlsVisible)
        {
            this.MainControlsVisible = false;

            this.OnPropertyChanged(nameof(this.MainControlsVisible));
            return;
        }

        this.MainControlsVisible = true;
        this.OnPropertyChanged(nameof(this.MainControlsVisible));
    }

    [RelayCommand]
    private void NextEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Next");
    }

    [RelayCommand]
    private void PrevEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Prev");
    }

    [RelayCommand]
    private void SetSavePath()
    {
        this._dataLocations.EBoardDataContextPath = new SharedMethod_UI().SetSaveDirectory();
    }

    [RelayCommand]
    private void SwitchMenuVisibility()
    {
        this.MainControlsVisibilityChange();
    }
}

// EOF