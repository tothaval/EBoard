using EBoard.Commands;
using EBoard.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    internal class EBoardBrowserViewModel : BaseViewModel
    {
		private ObservableCollection<EBoardViewModel> _eboards;

		public ObservableCollection<EBoardViewModel> EBoards
		{
			get { return _eboards; }
			set { _eboards = value; }
		}

        public ICommand ShowEBoardCommand { get; }

        public EBoardBrowserViewModel()
        {
                
        }


        public EBoardBrowserViewModel(NavigationStore navigationStore )
        {
            ShowEBoardCommand = new ShowEBoardCommand(navigationStore);

            EBoards = new ObservableCollection<EBoardViewModel>();

            EBoards.Add(new EBoardViewModel("the one", new SolidColorBrush(Colors.GreenYellow),1));
            EBoards.Add(new EBoardViewModel("calendar", new SolidColorBrush(Colors.CornflowerBlue),2));
            EBoards.Add(new EBoardViewModel("design field", new SolidColorBrush(Colors.MediumVioletRed),5));

            navigationStore.CurrentViewModel = EBoards[0];
        }

    }
}
