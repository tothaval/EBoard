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
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;

public partial class ScreenSetupViewModel : ObservableObject, IDisposable
{
    private NavigationContextViewModel viewModel;
    private ScreenViewModel? eBoardViewModel;

    [ObservableProperty]
    private string eboardName = "new";

    [ObservableProperty]
    private double eboardHeight = 480;

    [ObservableProperty]
    private double eboardWidth = 720;

    [ObservableProperty]
    private int eboardDepth = 100;

    [ObservableProperty]
    private double eboardOpacity = 100;

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
    private DateTime eboardCreatedDate = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenSetupViewModel"/> class.
    /// </summary>
    /// <param name="eBoardBrowserViewModel"></param>
    /// <param name="eBoardViewModel"></param>
    public ScreenSetupViewModel(NavigationContextViewModel eBoardBrowserViewModel)
    {
        this.viewModel = eBoardBrowserViewModel;

        if (this.ViewModel.EboardBrowserViewModel.SelectedEboard != null)
        {
            this.eBoardViewModel = this.ViewModel.EboardBrowserViewModel.SelectedEboard;

            var sizeManager = new FluidUISizeManager(this.EBoardViewModel);

            this.EboardName = new FluidUIDataBlockManager(this.EBoardViewModel).GetTitle();
            this.EboardWidth = sizeManager.GetWidth();
            this.EboardHeight = sizeManager.GetHeight();
            this.EboardDepth = new FluidUIStandManager(this.EBoardViewModel).GetZmaximum();
            this.EboardOpacity = new FluidUIDesignManager(this.EBoardViewModel).GetOpacity();

            this.EboardCreatedDate = this.EBoardViewModel.GetCreatedDate();

            this.EBoardViewModel.PropertyChanged += this.EBoardViewModel_PropertyChanged;
        }

        this.OnPropertyChanged(nameof(this.EBoardViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public NavigationContextViewModel ViewModel => this.viewModel;

    public ScreenViewModel EBoardViewModel => this.eBoardViewModel;

    public void Dispose()
    {
        if (this.EBoardViewModel != null)
        {
            this.EBoardViewModel.PropertyChanged -= this.EBoardViewModel_PropertyChanged;
        }
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

    internal ScreenViewModel GetNewScreen()
    {
        this.EboardName ??= string.Empty;

        var eboardScreen = EboardScreenFactory.GetEboardScreen(this.EboardName, this.EboardDepth, this.EboardWidth, this.EboardHeight);

        ScreenViewModel eBoardViewModel = EboardScreenFactory.GetScreenViewModel(this.ViewModel.MainViewModel, eboardScreen);

        return eBoardViewModel;
    }

    private void EBoardViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.GetCounts();
    }

    partial void OnEboardDepthChanged(int value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUIStandManager(this.EBoardViewModel);
            manager.SetZmaximum(value);
        }
    }

    partial void OnEboardNameChanged(string value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUIDataBlockManager(this.EBoardViewModel);
            manager.SetTitle(value);
        }
    }

    partial void OnEboardWidthChanged(double value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUISizeManager(this.EBoardViewModel);
            manager.SetWidth(value);
        }
    }

    partial void OnEboardHeightChanged(double value)
    {
        if (this.EBoardViewModel != null)
        {
            var manager = new FluidUISizeManager(this.EBoardViewModel);
            manager.SetHeight(value);
        }
    }

    [RelayCommand]
    private void AddEboard()
    {
        this.ViewModel.AddScreenViewModel();
    }

    [RelayCommand]
    private void DeleteAllScreens()
    {
        this.ViewModel?.DeleteAllScreens();
    }

    [RelayCommand]
    private void DeleteSelectedScreen()
    {
        this.ViewModel?.RemoveActiveScreen();
    }

    [RelayCommand]
    private void DeleteSelectedScreens()
    {
        this.ViewModel?.RemoveSelectedEboards();
    }

    [RelayCommand]
    private void DeselectEboard()
    {
        this.ViewModel?.DeselectEboard();
    }

    [RelayCommand]
    private void EditEboardParameters()
    {
        if (this.EBoardViewModel != null)
        {
            var dataManager = new FluidUIDataBlockManager(this.EBoardViewModel);
            var designManager = new FluidUIDesignManager(this.EBoardViewModel);
            var sizeManager = new FluidUISizeManager(this.EBoardViewModel);
            var standManager = new FluidUIStandManager(this.EBoardViewModel);

            dataManager.SetTitle(this.EboardName);

            designManager.SetOpacity(this.EboardOpacity);

            sizeManager.SetWidth(this.EboardWidth);
            sizeManager.SetHeight(this.EboardHeight);

            standManager.SetZmaximum(this.EboardDepth);
        }
    }
}

// EOF