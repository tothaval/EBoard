/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowImageCommand 
 * 
 *  command to apply an image as background for the mainwindow
 */
using EBoard.ViewModels;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands
{
    internal class MainWindowImageCommand : ICommand
    {
        private MainViewModel _MainViewModel;


        public event EventHandler? CanExecuteChanged;


        public MainWindowImageCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
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

                    _MainViewModel.ImagePath = setPath.FileName;
                
            }

        }
    }
}
// EOF