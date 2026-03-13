// <copyright file="ScreenSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.ScreenSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.Collections;
using System.Windows;

public partial class ScreenSetupViewModel : ObservableObject
{
<<<<<<< Updated upstream
    private EBoardBrowserViewModel viewModel;
    private EBoardViewModel eBoardViewModel;

    private readonly string txtRemoveAllEboardScreensQuestion = "Delete all screens?";

    private readonly string txtRemoveEboardQuestion = "Delete selected screen(s)?";

    private readonly string txtRemoveEboardsTitle = "Delete screen(s) confirmation";
=======
    private NavigationContextViewModel viewModel;
    private ScreenViewModel? screenViewModel;
>>>>>>> Stashed changes

    [ObservableProperty]
    private string eBoardName = "new";

    [ObservableProperty]
    private double newEBoardHeight = 480;

    [ObservableProperty]
    private double newEBoardWidth = 720;

    [ObservableProperty]
    private int eBoardDepth = 100;

    [ObservableProperty]
    private int totalPluginCount = 0;

    [ObservableProperty]
    private int addonCount = 0;

    [ObservableProperty]
    private int elementCount = 0;

    [ObservableProperty]
    private int shapeCount = 0;

    [ObservableProperty]
    private int areaCount = 0;

    [ObservableProperty]
    private int toolCount = 0;

    [ObservableProperty]
    private DateTime eBoardCreatedDate = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenSetupViewModel"/> class.
    /// </summary>
    /// <param name="eBoardBrowserViewModel"></param>
<<<<<<< Updated upstream
    /// <param name="eBoardViewModel"></param>
    public ScreenSetupViewModel(EBoardBrowserViewModel eBoardBrowserViewModel, EBoardViewModel eBoardViewModel)
=======
    public ScreenSetupViewModel(NavigationContextViewModel eBoardBrowserViewModel)
>>>>>>> Stashed changes
    {
        this.viewModel = eBoardBrowserViewModel;
        this.eBoardViewModel = eBoardViewModel;

<<<<<<< Updated upstream
        var sizeManager = new FluidUISizeManager(this.EBoardViewModel);

        this.EBoardName = new FluidUIDataBlockManager(this.EBoardViewModel).GetTitle();
        this.NewEBoardWidth = sizeManager.GetWidth();
        this.NewEBoardHeight = sizeManager.GetHeight();
        this.EBoardDepth = new FluidUIStandManager(this.EBoardViewModel).GetZmaximum();

        this.EBoardCreatedDate = this.EBoardViewModel.GetCreatedDate();

        this.GetCounts();
=======
        if (this.ViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.screenViewModel = this.ViewModel.EboardBrowserViewModel.SelectedEboard;

            var sizeManager = new FluidUISizeManager(this.ScreenViewModel);

            this.EboardName = new FluidUIDataBlockManager(this.ScreenViewModel).GetTitle();
            this.EboardWidth = sizeManager.GetWidth();
            this.EboardHeight = sizeManager.GetHeight();
            this.EboardDepth = new FluidUIStandManager(this.ScreenViewModel).GetZmaximum();
            this.EboardOpacity = new FluidUIDesignManager(this.ScreenViewModel).GetOpacity();

            this.EboardCreatedDate = this.ScreenViewModel.GetCreatedDate();

            this.ScreenViewModel.PropertyChanged += this.EBoardViewModel_PropertyChanged;
        }
>>>>>>> Stashed changes

        this.OnPropertyChanged(nameof(this.ScreenViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

<<<<<<< Updated upstream
=======
    public NavigationContextViewModel ViewModel => this.viewModel;

    public ScreenViewModel ScreenViewModel => this.screenViewModel;

    public void Dispose()
    {
        if (this.ScreenViewModel != null)
        {
            this.ScreenViewModel.PropertyChanged -= this.EBoardViewModel_PropertyChanged;
        }
    }

>>>>>>> Stashed changes
    internal void GetCounts()
    {
        this.TotalPluginCount = this.ScreenViewModel.GetTotalCount();
        this.AddonCount = this.ScreenViewModel.GetPluginCategoryCount(Enums.PluginCategories.Addon);
        this.ElementCount = this.ScreenViewModel.GetPluginCategoryCount(Enums.PluginCategories.Element);
        this.ShapeCount = this.ScreenViewModel.GetPluginCategoryCount(Enums.PluginCategories.Shape);
        this.AreaCount = this.ScreenViewModel.GetPluginCategoryCount(Enums.PluginCategories.Area);
        this.ToolCount = this.ScreenViewModel.GetPluginCategoryCount(Enums.PluginCategories.Tool);
    }

    public EBoardBrowserViewModel ViewModel => this.viewModel;

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    partial void OnEBoardDepthChanged(int value)
    {
        if (this.ScreenViewModel != null)
        {
            var manager = new FluidUIStandManager(this.ScreenViewModel);
            manager.SetZmaximum(value);
        }
    }

    partial void OnEBoardNameChanged(string value)
    {
        if (this.ScreenViewModel != null)
        {
            var manager = new FluidUIDataBlockManager(this.ScreenViewModel);
            manager.SetTitle(value);
        }
    }

    partial void OnNewEBoardWidthChanged(double value)
    {
        if (this.ScreenViewModel != null)
        {
            var manager = new FluidUISizeManager(this.ScreenViewModel);
            manager.SetWidth(value);
        }
    }

    partial void OnNewEBoardHeightChanged(double value)
    {
        if (this.ScreenViewModel != null)
        {
            var manager = new FluidUISizeManager(this.ScreenViewModel);
            manager.SetHeight(value);
        }
    }

    [RelayCommand]
    private void AddEBoard()
    {
        this.EBoardName ??= string.Empty;

        var eboardScreen = EBoardScreenFactory.GetNewEboardScreen(this.EBoardName, this.EBoardDepth, this.NewEBoardWidth, this.NewEBoardHeight);

        EBoardViewModel eBoardViewModel = EBoardScreenFactory.GetEBoardViewModelByEboardScreen(eboardScreen, this.EBoardViewModel.MainViewModel);

        this.viewModel.AddEBoardViewModel(eBoardViewModel);
    }

    [RelayCommand]
    private void DeleteAllScreens()
    {
        string question = this.txtRemoveAllEboardScreensQuestion;
        string title = this.txtRemoveEboardsTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.viewModel?.DeleteAllScreens();
    }

    [RelayCommand]
    private void DeleteSelectedScreen()
    {
        if (this.EBoardViewModel != null)
        {
            string question = this.txtRemoveEboardQuestion;
            string title = this.txtRemoveEboardsTitle;

            MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            this.viewModel?.RemoveEBoard(this.EBoardViewModel);
        }
    }

    [RelayCommand]
    private void DeleteSelectedScreens(object? selectedItems)
    {
        if (selectedItems != null)
        {
            string question = this.txtRemoveEboardQuestion;
            string title = this.txtRemoveEboardsTitle;

            MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            IList? selection = (IList)selectedItems;

            if (selection != null)
            {
                this.viewModel?.RemoveSelectedEBoards(selection);
            }
        }
    }

    [RelayCommand]
    private void EditEBoardParameters()
    {
        if (this.ScreenViewModel != null)
        {
<<<<<<< Updated upstream
            var dataManager = new FluidUIDataBlockManager(this.EBoardViewModel);
            var sizeManager = new FluidUISizeManager(this.EBoardViewModel);
            var standManager = new FluidUIStandManager(this.EBoardViewModel);
=======
            var dataManager = new FluidUIDataBlockManager(this.ScreenViewModel);
            var designManager = new FluidUIDesignManager(this.ScreenViewModel);
            var sizeManager = new FluidUISizeManager(this.ScreenViewModel);
            var standManager = new FluidUIStandManager(this.ScreenViewModel);
>>>>>>> Stashed changes

            dataManager.SetTitle(this.EBoardName);

            sizeManager.SetWidth(this.NewEBoardWidth);
            sizeManager.SetHeight(this.NewEBoardHeight);

            standManager.SetZmaximum(this.EBoardDepth);
        }
    }
}

// EOF