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
    internal class MainWindowMenuBarViewModel : BaseViewModel
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


        public ICommand CloseCommand { get; }


        public MainWindowMenuBarViewModel()
        {
            title = "EBoard";

            CloseCommand = new CloseApplicationCommand();
        }


    }
}
// EOF