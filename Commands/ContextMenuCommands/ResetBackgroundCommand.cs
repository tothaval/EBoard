/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ResetEBoardBrowserBackgroundCommand 
 * 
 *  command to replace the background image of EBoardBrowserViewModel
 *  with a default SolidColorBrush
 */
using EBoard.ViewModels;
using EBoard.Interfaces;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands
{
    internal class ResetBackgroundCommand : ICommand
    {
        private IElementBackgroundImage _IElementBackgroundImage;

        public event EventHandler? CanExecuteChanged;


        public ResetBackgroundCommand(IElementBackgroundImage elementBackgroundImage)
        {
            _IElementBackgroundImage = elementBackgroundImage;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _IElementBackgroundImage.ImagePath = string.Empty;

        }
    }
}
// EOF