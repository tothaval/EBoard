/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowMenuBarViewModel 
 * 
 *  view model for MainWindowMenuBarView, which has prototype element instantiation
 *  button and a prototype shape menu and a button to switch on or off the EBoardBrowserView
 */
using EBoard.Commands.ElementCreationCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    public class MainWindowMenuBarViewModel : BaseViewModel
    {
        
        // Properties & Fields
        #region Properties & Fields
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        private bool _EBoardBrowserSwitch;
        public bool EBoardBrowserSwitch
        {
            get { return _EBoardBrowserSwitch; }
            set
            {
                _EBoardBrowserSwitch = value;
                OnPropertyChanged(nameof(EBoardBrowserSwitch));
            }
        }


        private MainViewModel _MainViewModel;
        public MainViewModel MainViewModel => _MainViewModel;

        #endregion



        // Commands
        #region Commands

        public ICommand InvokePrototypeElementCommand { get; } 

        public ICommand InvokeEllipseShapeCommand { get; }

        public ICommand InvokePrototypeShapeElementCommand { get; }

        #endregion


        public MainWindowMenuBarViewModel(MainViewModel mainViewModel)
        {
            title = "EBoard";

            _MainViewModel = mainViewModel;

            InvokePrototypeElementCommand = new InvokePrototypeElementCommand(mainViewModel);

            InvokePrototypeShapeElementCommand = new InvokePrototypeShapeElementCommand(mainViewModel);

            InvokeEllipseShapeCommand = new InvokeEllipseShapeCommand(mainViewModel);
        }


    }
}
// EOF