// <copyright file="EBoardBrowserViewModel.cs" company=".">
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
 *  EBoardBrowserViewModel
 *
 *  view model class for EBoardBrowserView
 *
 *  EBoardBrowserView presents the EBoardViewModels currently existent within the application,
 *  as well as functionality to create, edit or delete eboard instances.
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Controls.ScreenSetup;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using System.Collections;
using System.Collections.ObjectModel;

public partial class EBoardBrowserViewModel : EboardFluidUIBaseViewModel
{
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private bool browserPanelVisible = true;

    [ObservableProperty]
    private bool screenSetupVisible = true;

    [ObservableProperty]
    private int maxWidth = 250;

    [ObservableProperty]
    private int maxHeight = 250;

    [ObservableProperty]
    private int currentSelectionID;

    [ObservableProperty]
    private int eBoardCount;

    private ObservableCollection<EBoardViewModel> eboards;

    [ObservableProperty]
    private EBoardViewModel selectedEBoard;

    [ObservableProperty]
    private ScreenSetupViewModel screenSetupViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="EBoardBrowserViewModel"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    public EBoardBrowserViewModel(MainViewModel mainViewModel)
        : base()
    {
        this.mainViewModel = mainViewModel;

        mainViewModel.EBoardConfig?.EBoardBrowserViewContext?.Design?.LoadBrushesFromColorData();

        this.SetFluidUI(mainViewModel.EBoardConfig.EBoardBrowserViewContext);

        this.CreateFluidUIMenuViewModel();

        var manager = new FluidUIDataBlockManager(this);

        manager.SetupTitleAndText("Browser", "Use this browser to:\n\n-> navigate eboard screens\n-> add, edit or delete eboard screens\n\n\nyou can change this description");

        this.CreateFluidUIMenuViewModel();

        this.eboards = new ObservableCollection<EBoardViewModel>();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.EBoards));
    }

    public ObservableCollection<EBoardViewModel> EBoards => this.eboards;

    /// <summary>
    ///
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal Task AddEBoardViewModel(EBoardViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            this.EBoards.Add(eBoardViewModel);

            this.EBoardCount = this.EBoards.Count;
        }

        return Task.CompletedTask;
    }

    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, fluidUIContextHasStand: false));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal Task DeleteAllScreens()
    {
        this.SelectedEBoard?.Dispose();
        this.EBoards?.Clear();

        return Task.CompletedTask;
    }

    internal List<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        List<EBoardSDK.Models.EboardScreen> eboardScreens = [];

        this.EBoards.Select(x => x).ToList().ForEach(
            escreen =>
            {
                escreen.DeselectElements();

                List<ElementConfig> elementConfigs = [];

                escreen.Elements.Select(x => x).ToList().ForEach(
                    element =>
                    {
                        if (element.Plugin == null)
                        {
                            return;
                        }

                        elementConfigs.Add(new ElementConfig()
                        {
                            EID = element.EID,
                            ID = escreen.Elements.IndexOf(element),
                            PluginHeader = element.Plugin.PluginHeader,
                            PluginName = element.Plugin.PluginName,
                            Plugin = element.Plugin,
                            PluginType = element.Plugin.GetType()?.FullName!,
                            AssemblyName = element.Plugin.GetType()?.AssemblyQualifiedName!,

                            ElementContext = (FluidUIContext)element.FluidUI,
                        });
                    });

                eboardScreens.Add(new EboardScreen()
                {
                    EBoardScreenContext = (FluidUIContext)escreen.FluidUI,

                    EBID = escreen.EBID,
                    ID = this.EBoards.IndexOf(escreen),

                    Elements = elementConfigs,
                });
            });

        return eboardScreens;
    }

    internal void RefreshSelectedEboardData()
    {
        this.RefreshEBoardParameters();
    }

    internal void RemoveEBoard(EBoardViewModel? eBoardViewModel = null)
    {
        if (eBoardViewModel != null)
        {
            this.EBoards.Remove(eBoardViewModel);
        }
    }

    internal void RemoveSelectedEBoards(IList eBoardViewModelList)
    {
        if (eBoardViewModelList != null && this.EBoards.Count > 0)
        {
            var indexcount = eBoardViewModelList.Count - 1;

            for (int i = indexcount; i > -1; i--)
            {
                EBoardViewModel? screen = (EBoardViewModel)eBoardViewModelList[i]!;

                this.RemoveEBoard(screen);
            }
        }
    }

    private void RefreshEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this.ScreenSetupViewModel = new ScreenSetupViewModel(this, this.SelectedEBoard);

            this.CurrentSelectionID = this.EBoards.IndexOf(this.SelectedEBoard) + 1;
            this.EBoardCount = this.EBoards.Count;
        }
    }

    partial void OnSelectedEBoardChanged(EBoardViewModel value)
    {
        this.RefreshEBoardParameters();
    }

    partial void OnSelectedEBoardChanging(EBoardViewModel? oldValue, EBoardViewModel newValue)
    {
        if (oldValue == null || newValue == null || oldValue.Equals(newValue))
        {
            return;
        }

        oldValue.BecomesInactive();

        newValue.BecomesActive();
    }

    [RelayCommand]
    private void ToggleBrowserPanelVisibility()
    {
        this.BrowserPanelVisible = !this.BrowserPanelVisible;
    }

    [RelayCommand]
    private void ToggleScreenSetupVisibility()
    {
        this.ScreenSetupVisible = !this.ScreenSetupVisible;
    }
}

// EOF