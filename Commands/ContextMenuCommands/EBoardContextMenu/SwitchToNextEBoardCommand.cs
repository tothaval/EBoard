/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  SwitchToNextEBoardCommand 
 * 
 *  command to switch the selected eboard within EBoardBrowserViewModel to
 *  the next eboard in EBoardBrowserViewModel.EBoards observable collection,
 *  if possible
 */
using EBoard.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands.EBoardContextMenu
{
    internal class SwitchToNextEBoardCommand : ICommand
    {
        private EBoardViewModel _EBoardViewModel;

        public event EventHandler? CanExecuteChanged;


        public SwitchToNextEBoardCommand(EBoardViewModel eBoardViewModel)
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
                    for (int i = 0; i < mainViewModel.EBoardBrowserViewModel.EBoards.Count; i++)
                    {
                        if (mainViewModel.EBoardBrowserViewModel.EBoards[i] == mainViewModel.EBoardBrowserViewModel.SelectedEBoard)
                        {
                            if (i + 1 < mainViewModel.EBoardBrowserViewModel.EBoards.Count)
                            {

                                mainViewModel.EBoardBrowserViewModel.SelectedEBoard = mainViewModel.EBoardBrowserViewModel.EBoards[i + 1];

                                break;
                            }
                        
                        }
                    }
                }
            }
        }
    }
}
// EOF