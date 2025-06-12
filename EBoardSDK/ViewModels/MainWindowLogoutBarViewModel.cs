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
    [ObservableProperty]
    private string txtExitEboard = "Off";

    [ObservableProperty]
    private string txtShutDownMachine = "Shutdown";

    private readonly BrushManagement brushManagement;

    public BrushManagement BrushManagement => this.brushManagement;


    public MainWindowLogoutBarViewModel(BrushManagement brushManagement)
    {
        this.brushManagement = brushManagement;

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
        new SharedMethod_UI().ShutDownMachine();
    }
}

// EOF