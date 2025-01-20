/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowLogoutBarViewModel 
 * 
 *  view model for MainWindowLogoutBarView, which has two buttons atm to close the application
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Commands;
using EBoard.Utilities.SharedMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EBoard.ViewModels;

public partial class MainWindowLogoutBarViewModel : ObservableObject
{    

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

}
// EOF