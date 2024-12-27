/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardBrowserImageCommand 
 * 
 *  command to apply an image to the EBoardBrowserViewModel background
 */
using EBoard.ViewModels;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands.EBoardBrowserContextMenu
{
    internal class EBoardBrowserImageCommand : ICommand
    {
        private EBoardBrowserViewModel _EBoardBrowserViewModel;

        public event EventHandler? CanExecuteChanged;


        public EBoardBrowserImageCommand(EBoardBrowserViewModel eBoardBrowserViewModel)
        {
            _EBoardBrowserViewModel = eBoardBrowserViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {

            Microsoft.Win32.OpenFileDialog setPath = new Microsoft.Win32.OpenFileDialog();
            setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");
            setPath.Filter = "files (*.*)|*.*";
            setPath.FilterIndex = 2;
            setPath.RestoreDirectory = true;

            if (setPath.ShowDialog() == true)
            {
                _EBoardBrowserViewModel.ImagePath = setPath.FileName;

            }

        }
    }
}
// EOF