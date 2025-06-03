/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainWindowLogoutBarViewModel
 *
 *  view model for MainWindowLogoutBarView, which has two buttons atm to close the application
 */
namespace EBoard.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.SharedMethods;

public partial class MainWindowLogoutBarViewModel : ObservableObject
{
    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }
}

// EOF