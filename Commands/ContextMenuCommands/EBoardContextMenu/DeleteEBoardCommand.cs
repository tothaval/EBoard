/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  DeleteEBoardCommand 
 * 
 *  command to remove the selected eboard within EBoardBrowserViewModel
 *  from EBoardBrowserViewModel.EBoards observable collection.
 */
using EBoard.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands.EBoardContextMenu
{
    internal class DeleteEBoardCommand : ICommand
    {
        private EBoardViewModel _EBoardViewModel;

        public event EventHandler? CanExecuteChanged;


        public DeleteEBoardCommand(EBoardViewModel eBoardViewModel)
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
                //??
                mainViewModel.EBoardBrowserViewModel.EBoards.Remove(mainViewModel.EBoardBrowserViewModel.SelectedEBoard);


            }

        }
    }
}
// EOF