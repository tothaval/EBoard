/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowMenuBarViewModel 
 * 
 *  view model for MainWindowMenuBarView, which has prototype element instantiation
 *  button and a prototype shape menu and a button to switch on or off the EBoardBrowserView
 */
using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Commands.ElementCreationCommands;
using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels;

public partial class MainWindowMenuBarViewModel : ObservableObject
{

    // Properties & Fields
    #region Properties & Fields

    [ObservableProperty]
    private string title;


    [ObservableProperty]
    private bool eBoardBrowserSwitch;


    private MainViewModel _MainViewModel;
    public MainViewModel MainViewModel => _MainViewModel;

    #endregion



    // Commands
    #region Commands
    
    public ICommand InvokeElementCommand { get; }

    #endregion


    public MainWindowMenuBarViewModel(MainViewModel mainViewModel)
    {
        title = "EBoard";

        _MainViewModel = mainViewModel;

        InvokeElementCommand = new InvokeElementCommand(mainViewModel);
    }


}
// EOF