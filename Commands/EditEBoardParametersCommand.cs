using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Commands
{
    internal class EditEBoardParametersCommand : BaseCommand
    {
        private EBoardBrowserViewModel _EBoardBrowserViewModel;

        public EditEBoardParametersCommand(EBoardBrowserViewModel eBoardBrowserViewModel)
        {
            _EBoardBrowserViewModel = eBoardBrowserViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_EBoardBrowserViewModel.SelectedEBoard != null)
            {
                _EBoardBrowserViewModel.SelectedEBoard.EBoardName = _EBoardBrowserViewModel.EBoardName;
                _EBoardBrowserViewModel.SelectedEBoard.EBoardDepth = _EBoardBrowserViewModel.EBoardDepth;
                _EBoardBrowserViewModel.SelectedEBoard.EBoardWidth = _EBoardBrowserViewModel.EBoardWidth;
                _EBoardBrowserViewModel.SelectedEBoard.EBoardHeight = _EBoardBrowserViewModel.EBoardHeight;
            }

        }
    }
}
