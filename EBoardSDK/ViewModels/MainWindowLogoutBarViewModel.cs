// <copyright file="MainWindowLogoutBarViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainWindowLogoutBarViewModel
 *
 *  view model for MainWindowLogoutBarView, which has two buttons atm to close the application
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;

public partial class MainWindowLogoutBarViewModel : ObservableObject
{
    private readonly BrushManagement brushManagement;
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private string txtExitEboard = "Off";

    [ObservableProperty]
    private string txtShutDownMachine = "Shutdown";

    public BrushManagement BrushManagement => this.brushManagement;

    public MainWindowLogoutBarViewModel(BrushManagement brushManagement, MainViewModel mainViewModel)
    {
        this.brushManagement = brushManagement;
        this.mainViewModel = mainViewModel;

        brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    [RelayCommand]
    private void ShutDown()
    {
        var runner = this.mainViewModel.GetRunnerInstance();

        var saveResult = runner.SaveEboardDataAsync().Result;

        new SharedMethod_UI().ShutDownMachine();
    }
}

// EOF