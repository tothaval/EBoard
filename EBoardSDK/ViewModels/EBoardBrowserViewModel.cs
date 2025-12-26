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
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using System.Collections.ObjectModel;
using System.Windows;

public partial class EBoardBrowserViewModel : EboardFluidUIBaseViewModel
{
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private int currentSelectionID;

    [ObservableProperty]
    private DateTime eBoardCreatedDate;

    [ObservableProperty]
    private int eBoardContainerCount;

    [ObservableProperty]
    private int eBoardElementCount;

    [ObservableProperty]
    private int eBoardShapeCount;

    [ObservableProperty]
    private double newEBoardHeight = 480;

    [ObservableProperty]
    private double newEBoardWidth = 720;

    [ObservableProperty]
    private int eBoardCount;

    [ObservableProperty]
    private int eBoardDepth = 100;

    [ObservableProperty]
    private string eBoardName = "new";

    private ObservableCollection<EBoardViewModel> eboards;

    [ObservableProperty]
    private EBoardViewModel selectedEBoard;

    public EBoardBrowserViewModel(MainViewModel mainViewModel)
        : base()
    {
        this.mainViewModel = mainViewModel;

        mainViewModel.EBoardConfig?.EBoardBrowserViewContext?.Design?.LoadBrushesFromColorData();

        this.SetFluidUI(mainViewModel.EBoardConfig.EBoardBrowserViewContext);

        var helper = new SharedMethod_UI();
        helper.SetupTitleAndText(
            this.FluidUI.DataBlock,
            "Eboard Browser",
            "Use this browser to:\n\n-> navigate eboard screens\n-> add, edit or delete eboard screens\n\n\nyou can change this description");

        this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, fluidUIContextHasStand: false, fluidUIContextHasArea: false));

        this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

        this.EBoards = new ObservableCollection<EBoardViewModel>();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public ObservableCollection<EBoardViewModel> EBoards
    {
        get
        {
            return this.eboards;
        }

        set
        {
            this.eboards = value;
            if (this.eboards.Count == 0)
            {
                this.eboards.Clear();
            }
        }
    }

    private static string TxtRemoveAllEboardScreensQuestion => "Delete all screens?";

    private static string TxtRemoveEboardQuestion => "Remove Screen?";

    private static string TxtRemoveEboardTitle => "Screen Deletion";

    public Task AddEBoardViewModel(EBoardViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            this.EBoards.Add(eBoardViewModel);

            this.EBoardCount = this.EBoards.Count;
        }

        return Task.CompletedTask;
    }

    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        IList<EBoardSDK.Models.EboardScreen> eboardScreens = [];

        this.EBoards.Select(x => x).ToList().ForEach(
            escreen =>
            {
                ObservableCollection<ElementConfig> elementConfigs = [];

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
                            PluginType = element.Plugin.GetType().FullName,
                            AssemblyName = element.Plugin.GetType().AssemblyQualifiedName,

                            ElementContext = (FluidUIContext)element.FluidUI,
                        });
                    });

                eboardScreens.Add(new EboardScreen()
                {
                    EBoardScreenContext = (FluidUIContext)escreen.FluidUI,

                    EBID = escreen.EBID,
                    ID = this.EBoards.IndexOf(escreen),
                    EBoardDepth = escreen.EBoardDepth,
                    EBoardName = escreen.EBoardName,

                    Elements = elementConfigs,
                });
            });

        return eboardScreens;
    }

    [RelayCommand]
    private void AddEBoard()
    {
        if (this.EBoardName == null)
        {
            this.EBoardName = string.Empty;
        }

        var eboardScreen = EBoardFactory.GetNewEboardScreen(this.EBoardName, this.EBoardDepth, this.NewEBoardWidth, this.NewEBoardHeight);

        EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(eboardScreen, this.mainViewModel);

        this.EBoards.Add(eBoardViewModel);

        this.SelectedEBoard = this.EBoards.Last();

        this.RefreshEBoardParameters();
    }

    partial void OnSelectedEBoardChanging(EBoardViewModel? oldValue, EBoardViewModel newValue)
    {
        if (oldValue == null || oldValue == newValue || oldValue.Equals(newValue))
        {
            return;
        }

        oldValue.EBoardActive = false;

        oldValue.Elements.CollectionChanged -= SelectedEBoardElements_CollectionChanged;

        if (newValue == null)
        {
            return;
        }

        newValue.Elements.CollectionChanged += SelectedEBoardElements_CollectionChanged;
    }

    partial void OnSelectedEBoardChanged(EBoardViewModel value)
    {
        if (value == null)
        {
            return;
        }

        RefreshEBoardParameters();
    }

    private void SelectedEBoardElements_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (this.SelectedEBoard.Elements.Count != this.EBoardContainerCount)
        {
            this.RefreshSelectedEboardElementData();
        }
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        if (this.SelectedEBoard != null && this.EBoards.Count > 0)
        {
            this.RemoveSelectedEBoard();

            if (this.EBoards.Count > 0)
            {
                this.RefreshEBoardParameters();

                this.OnPropertyChanged(nameof(this.EBoards));
            }
        }
    }

    [RelayCommand]
    private void EditEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this.SelectedEBoard.EBoardName = this.EBoardName;
            this.SelectedEBoard.EBoardDepth = this.EBoardDepth;
            this.SelectedEBoard.Width = (int)this.NewEBoardWidth;
            this.SelectedEBoard.Height = (int)this.NewEBoardHeight;
        }
    }

    [RelayCommand]
    private void DeleteAllScreens()
    {
        string question = TxtRemoveAllEboardScreensQuestion;
        string title = TxtRemoveEboardTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.SelectedEBoard?.Dispose();
        this.EBoards?.Clear();
    }

    public void DeleteAllElements()
    {
        this.SelectedEBoard?.Clear();
    }

    public void RefreshEboardData()
    {
        this.RefreshEBoardParameters();
    }

    public void RemoveSelectedEBoard(EBoardViewModel eBoardViewModel = null)
    {
        string question = TxtRemoveEboardQuestion;
        string title = TxtRemoveEboardTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        if (eBoardViewModel != null)
        {
            this.SelectedEBoard = eBoardViewModel;
        }

        this.EBoards.Remove(this.SelectedEBoard);
        this.SelectedEBoard?.Dispose();

        if (this.EBoards.Count > 0 && result == MessageBoxResult.Yes)
        {
            this.SelectedEBoard = this.EBoards.Last();
        }
    }

    private void RefreshEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this.EBoardName = this.SelectedEBoard.EBoardName;
            this.EBoardDepth = this.SelectedEBoard.EBoardDepth;
            this.NewEBoardHeight = this.SelectedEBoard.FluidUI.Size.Height;
            this.NewEBoardWidth = this.SelectedEBoard.FluidUI.Size.Width;
            this.SelectedEBoard.EBoardActive = true;

            this.CurrentSelectionID = this.EBoards.IndexOf(this.SelectedEBoard) + 1;
            this.EBoardCount = this.EBoards.Count;
            this.EBoardCreatedDate = this.SelectedEBoard.GetCreatedDate();

            this.RefreshSelectedEboardElementData();
        }
        else
        {
            this.EBoardName = "no board selected";
            this.EBoardDepth = 10;
            this.NewEBoardHeight = 620;
            this.NewEBoardWidth = 1240;

            this.CurrentSelectionID = 0;
            this.EBoardContainerCount = 0;
            this.EBoardCount = 0;
            this.EBoardCreatedDate = DateTime.Now;
            this.EBoardElementCount = 0;
            this.EBoardShapeCount = 0;
        }
    }

    private void RefreshSelectedEboardElementData()
    {
        this.EBoardContainerCount = this.SelectedEBoard.GetContainerCount();
        this.EBoardElementCount = this.SelectedEBoard.GetElementCount();
        this.EBoardShapeCount = this.SelectedEBoard.GetShapeCount();
    }
}

// EOF