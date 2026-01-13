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
    private EBoardBrowserViewModel viewModel;
    private EBoardViewModel eBoardViewModel;

    private readonly string txtRemoveAllEboardScreensQuestion = "Delete all screens?";

    private readonly string txtRemoveEboardQuestion = "Delete selected screen(s)?";

    private readonly string txtRemoveEboardsTitle = "Delete screen(s) confirmation";

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
    /// <param name="eBoardViewModel"></param>
    public ScreenSetupViewModel(EBoardBrowserViewModel eBoardBrowserViewModel, EBoardViewModel eBoardViewModel)
    {
        this.viewModel = eBoardBrowserViewModel;
        this.eBoardViewModel = eBoardViewModel;

        var sizeManager = new FluidUISizeManager(this.EBoardViewModel);

        this.EBoardName = new FluidUIDataBlockManager(this.EBoardViewModel).GetTitle();
        this.NewEBoardWidth = sizeManager.GetWidth();
        this.NewEBoardHeight = sizeManager.GetHeight();
        this.EBoardDepth = new FluidUIStandManager(this.EBoardViewModel).GetZmaximum();

        this.EBoardCreatedDate = this.EBoardViewModel.GetCreatedDate();

        this.GetCounts();

        this.OnPropertyChanged(nameof(this.EBoardViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    internal void GetCounts()
    {
        this.TotalPluginCount = this.EBoardViewModel.GetTotalCount();
        this.AddonCount = this.EBoardViewModel.GetPluginCategoryCount(Enums.PluginCategories.Addon);
        this.ElementCount = this.EBoardViewModel.GetPluginCategoryCount(Enums.PluginCategories.Element);
        this.ShapeCount = this.EBoardViewModel.GetPluginCategoryCount(Enums.PluginCategories.Shape);
        this.AreaCount = this.EBoardViewModel.GetPluginCategoryCount(Enums.PluginCategories.Area);
        this.ToolCount = this.EBoardViewModel.GetPluginCategoryCount(Enums.PluginCategories.Tool);
    }

    public EBoardBrowserViewModel ViewModel => this.viewModel;

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    partial void OnEBoardDepthChanged(int value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUIStandManager(this.EBoardViewModel);
            manager.SetZmaximum(value);
        }
    }

    partial void OnEBoardNameChanged(string value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUIDataBlockManager(this.EBoardViewModel);
            manager.SetTitle(value);
        }
    }

    partial void OnNewEBoardWidthChanged(double value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUISizeManager(this.EBoardViewModel);
            manager.SetWidth(value);
        }
    }

    partial void OnNewEBoardHeightChanged(double value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUISizeManager(this.EBoardViewModel);
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
        if (this.EBoardViewModel != null)
        {
            var dataManager = new FluidUIDataBlockManager(this.EBoardViewModel);
            var sizeManager = new FluidUISizeManager(this.EBoardViewModel);
            var standManager = new FluidUIStandManager(this.EBoardViewModel);

            dataManager.SetTitle(this.EBoardName);

            sizeManager.SetWidth(this.NewEBoardWidth);
            sizeManager.SetHeight(this.NewEBoardHeight);

            standManager.SetZmaximum(this.EBoardDepth);
        }
    }
}

// EOF