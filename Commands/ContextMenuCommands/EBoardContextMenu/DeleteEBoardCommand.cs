/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardViewModel 
 * 
 *  view model class for EBoardView
 *  
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored(WIP) and loaded(WIP)
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
