/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ImageCommand 
 * 
 *  command to apply an image to the element background
 */
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands
{
    public class ImageCommand : ICommand
    {
        private ElementViewModel _ElementViewModel;

        public event EventHandler? CanExecuteChanged;


        public ImageCommand(ElementViewModel elementViewModel)
        {
            _ElementViewModel = elementViewModel;
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
                if (_ElementViewModel.IsContent)
                {
                    _ElementViewModel.ContentViewModel.ImagePath = setPath.FileName;
                }

                if (_ElementViewModel.IsShape)
                {
                    _ElementViewModel.ShapeViewModel.ImagePath = setPath.FileName;
                }
            }

        }
    }
}
// EOF