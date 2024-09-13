using EBoard.Navigation;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Commands
{
    class ShowEBoardCommand : BaseCommand
    {
        private readonly NavigationStore _navigationStore;


        public ShowEBoardCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }


        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = (EBoardViewModel)parameter;           
        }

    }
}
