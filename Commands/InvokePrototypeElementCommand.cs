using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Commands
{
    internal class InvokePrototypeElementCommand : BaseCommand
    {

        private MainViewModel _MainViewModel;


        public InvokePrototypeElementCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddPrototypeElement(); 
            }
        }


    }
}
// EOF