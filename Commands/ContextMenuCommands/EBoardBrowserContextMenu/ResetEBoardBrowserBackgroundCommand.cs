using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBoard.Commands.ContextMenuCommands.EBoardBrowserContextMenu
{
    internal class ResetEBoardBrowserBackgroundCommand : ICommand
    {
        private EBoardBrowserViewModel _EBoardBrowserViewModel;

        public event EventHandler? CanExecuteChanged;


        public ResetEBoardBrowserBackgroundCommand(EBoardBrowserViewModel eBoardBrowserViewModel)
        {
            _EBoardBrowserViewModel = eBoardBrowserViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _EBoardBrowserViewModel.ImagePath = string.Empty;

        }
    }
}
