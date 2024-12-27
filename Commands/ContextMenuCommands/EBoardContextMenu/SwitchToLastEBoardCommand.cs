/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  SwitchToLastEBoardCommand 
 * 
 *  command to switch the selected eboard within EBoardBrowserViewModel to
 *  the last eboard in EBoardBrowserViewModel.EBoards observable collection.
 */
using EBoard.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands.EBoardContextMenu
{
    internal class SwitchToLastEBoardCommand : ICommand
    {
        private EBoardViewModel _EBoardViewModel;

        public event EventHandler? CanExecuteChanged;


        public SwitchToLastEBoardCommand(EBoardViewModel eBoardViewModel)
        {
            _EBoardViewModel = eBoardViewModel;
        }
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {

            MainViewModel mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;

            if (mainViewModel != null)
            {
                if (mainViewModel.EBoardBrowserViewModel.EBoards.Count > 1)
                {
                    mainViewModel.EBoardBrowserViewModel.SelectedEBoard = mainViewModel.EBoardBrowserViewModel.EBoards.Last();
                }
            }
        }
    }
}
// EOF