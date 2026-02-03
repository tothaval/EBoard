// <copyright file="NavigationContextViewModel.cs" company=".">
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
 *  NavigationContextViewModel
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
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Plugins.Eboard.EboardBrowser;
using EBoardSDK.Utilities.Factories;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

/// <summary>
/// This class represents the FluidUI Context Area (CA) that is tasked with
/// navigating between Screen CAs and managing Screen CA properties.
/// </summary>
public partial class NavigationContextViewModel : FluidUIBaseViewModel
{
    private readonly FluidUIContextAreas contextArea = FluidUIContextAreas.Navigation;

    private readonly MainViewModel mainViewModel;
    private EboardBrowserViewModel eboardBrowserViewModel;

    private ObservableCollection<ScreenViewModel> eboards;

    [ObservableProperty]
    private IList selectedEboards;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationContextViewModel"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    public NavigationContextViewModel(MainViewModel mainViewModel)
        : base()
    {
        this.mainViewModel = mainViewModel;

        this.eboards = new ObservableCollection<ScreenViewModel>();

        this.eboardBrowserViewModel = new EboardBrowserViewModel(this);
        this.eboardBrowserViewModel.SetMainViewModel(this.mainViewModel);

        this.SetFluidUI(this.mainViewModel.EBoardConfig?.EBoardBrowserViewContext);

        this.CreateFluidUIMenuViewModel();

        var manager = new FluidUIDataBlockManager(this);

        manager.SetupTitleAndText("Browser", "Use this browser to:\n\n-> navigate eboard screens\n-> add, edit or delete eboard screens\n\n\nyou can change this description");

        this.CreateFluidUIMenuViewModel();

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.EboardBrowserViewModel));
    }

    public override FluidUIContextAreas ContextArea => this.contextArea;

    public EboardBrowserViewModel EboardBrowserViewModel => this.eboardBrowserViewModel;

    public ObservableCollection<ScreenViewModel> Eboards => this.eboards;

    public int EboardCount => this.eboards.Count;

    internal MainViewModel MainViewModel => this.mainViewModel;

    internal override void CreateFluidUIMenuViewModel()
    {
        if (this.fluidUIMenuViewModel == null)
        {
            this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, Enums.FluidUIStandSettings.NoStandContextArea));
        }
    }

    /// <summary>
    /// Adds an existing <see cref="ScreenViewModel"/> to the <see cref="Eboards"/> list.
    /// </summary>
    /// <param name="eBoardViewModel">Desired: the viewmodel of the new screen.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal Task AddScreenViewModel(ScreenViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            this.Eboards.Add(eBoardViewModel);

            if (this.Eboards.Count == 1)
            {
                this.SwitchToEboard("first");
            }

            this.OnPropertyChanged(nameof(this.EboardCount));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds an existing <see cref="ScreenViewModel"/> to the <see cref="Eboards"/> list.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal Task AddScreenViewModel()
    {
        this.AddScreenViewModel(EboardScreenFactory.GetScreenViewModel(this.MainViewModel));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes all Screen CAs from <see cref="Eboards"/> list.
    ///
    /// Be advised:
    /// All element and plugin data saved in /Eboard/ScreenData directory will be deleted.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    internal Task DeleteAllScreens()
    {
        string question = MainViewModel.TxtRemoveAllEboardScreensQuestion;
        string title = MainViewModel.TxtRemoveEboardsTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return Task.CompletedTask;
        }

        this.EboardBrowserViewModel.SelectedEboard?.Dispose();

        foreach (var item in this.Eboards)
        {
            item.Dispose();
        }

        this.Eboards?.Clear();

        this.OnPropertyChanged(nameof(this.EboardCount));

        return Task.CompletedTask;
    }

    /// <summary>
    /// Deselects the active Screen CA by nulling <see cref="EboardBrowserViewModel.SelectedEboard"/>
    /// property, which results in an empty central MainWindow CA.
    /// </summary>
    internal void DeselectEboard()
    {
        this.EboardBrowserViewModel?.DeselectEboard();
    }

    /// <summary>
    /// Gets a list of <see cref="EboardScreen"/> models. Each contains FluidUI data of
    /// the Screen CA as well as a List of <see cref="ElementConfig"/> models, that hold
    /// FluidUI data of the instantiated <see cref="ElementViewModel"/> CAs.
    ///
    /// The returned list is used to save the retrieved FluidUI data to files.
    /// </summary>
    /// <returns>Returns a list of <see cref="EboardScreen"/> models.</returns>
    internal List<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        List<EBoardSDK.Models.EboardScreen> eboardScreens = [];

        this.Eboards.Select(x => x).ToList().ForEach(
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

                        elementConfigs.Add(new ElementConfig(element));
                    });

                eboardScreens.Add(new EboardScreen()
                {
                    EBoardScreenContext = (FluidUIContext)escreen.FluidUI,

                    EBID = escreen.EBID,
                    ID = this.Eboards.IndexOf(escreen),

                    Elements = elementConfigs,
                });
            });

        return eboardScreens;
    }

    internal void InsertSelectedItems(IList items)
    {
        this.SelectedEboards = items;
    }

    /// <summary>
    /// Removes <paramref name="eBoardViewModel"/> from <see cref="Eboards"/> list.
    /// </summary>
    /// <param name="eBoardViewModel">Desired is the Screen CA that should be deleted.</param>
    internal void RemoveEboard(ScreenViewModel? eBoardViewModel = null)
    {
        if (eBoardViewModel != null)
        {
            this.Eboards.Remove(eBoardViewModel);
        }
    }

    /// <summary>
    /// Removes active screen from <see cref="Eboards"/> list.
    /// </summary>
    internal void RemoveActiveScreen()
    {
        string question = MainViewModel.TxtRemoveEboardQuestion;
        string title = MainViewModel.TxtRemoveEboardTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        if (this.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.Eboards.Remove(this.EboardBrowserViewModel.SelectedEboard);
        }
    }

    /// <summary>
    /// Calls <see cref="EboardBrowserViewModel.RefreshSelectedEboardData()"/>,
    /// which can change <see cref="EboardBrowserViewModel.CurrentSelectionID"/>.
    /// It will create a new <see cref="EboardBrowserViewModel.ScreenSetupViewModel"/>
    /// instance on every call.
    /// </summary>
    internal void RefreshSelectedEboardData()
    {
        this.EboardBrowserViewModel.RefreshSelectedEboardData();
    }

    /// <summary>
    /// Removes all Screen CAs from <paramref name="eBoardViewModelList"/> from <see cref="Eboards"/> list property.
    /// </summary>
    /// <param name="selectedItems">Desired is a list of the Screen CAs that should be deleted.</param>
    internal void RemoveSelectedEboards()
    {
        if (this.SelectedEboards != null)
        {
            string question = MainViewModel.TxtRemoveEboardQuestion;
            string title = MainViewModel.TxtRemoveEboardsTitle;

            MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            if (this.SelectedEboards.Count > 0)
            {
                var count = this.SelectedEboards.Count;

                for (int i = count; i > 0; i--)
                {
                    ScreenViewModel? screen = (ScreenViewModel)this.SelectedEboards[i - 1]!;

                    this.RemoveEboard(screen);
                }
            }
        }
    }

    /// <summary>
    /// Changes <see cref="EboardBrowserViewModel.SelectedEboard"/> property by
    /// calling <see cref="EboardBrowserViewModel.SwitchToEboard(string)"/>, which
    /// results in <see cref="EboardBrowserViewModel"/>
    /// navigating the list of Screen CAs in <see cref="Eboards"/> list property
    /// and selecting the target value if possible.
    /// </summary>
    /// <param name="target">The navigation direction: valid are: first, last, prev, and next.</param>
    internal void SwitchToEboard(string target) // TODO change string to enum?
    {
        this.EboardBrowserViewModel?.SwitchToEboard(target);
    }

    [RelayCommand]
    private void AddScreen()
    {
        this.AddScreenViewModel();
    }

    [RelayCommand]
    private void DeleteSelectedScreen()
    {
        this.DeleteSelectedScreen();
    }

    [RelayCommand]
    private void DeselectScreen()
    {
        this.DeselectScreen();
    }
}

// EOF