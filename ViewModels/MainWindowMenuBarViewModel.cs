using EBoard.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels
{
    public class MainWindowMenuBarViewModel : BaseViewModel
    {

		private string title;
		public string Title
		{
			get { return title; }
			set { title = value;
                OnPropertyChanged(nameof(Title));
            }
		}


        private Brush _backgroundBrush;
        public Brush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set { _backgroundBrush = value; }
        }


        private bool _EBoardBrowserSwitch;
        public bool EBoardBrowserSwitch
        {
            get { return _EBoardBrowserSwitch; }
            set
            {
                _EBoardBrowserSwitch = value;
                OnPropertyChanged(nameof(EBoardBrowserSwitch));
            }
        }


        public ICommand CloseCommand { get; }


        public MainWindowMenuBarViewModel()
        {
            title = "EBoard";

            CloseCommand = new CloseApplicationCommand();
        }


    }
}
// EOF