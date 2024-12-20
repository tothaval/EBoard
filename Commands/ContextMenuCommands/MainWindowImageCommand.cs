using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
