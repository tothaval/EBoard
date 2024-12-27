/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardImageCommand 
 * 
 *  command to apply an image to the selected eboards background
 */
using EBoard.ViewModels;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands
{
    public class EBoardImageCommand : ICommand
    {
        private EBoardViewModel _EBoardViewModel;

        public event EventHandler? CanExecuteChanged;


        public EBoardImageCommand(EBoardViewModel eBoardViewModel)
        {
            _EBoardViewModel = eBoardViewModel;               
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
                    _EBoardViewModel.ImagePath = setPath.FileName;
                
            }

        }
    }
}
// EOF