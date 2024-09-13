using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    internal class EBoardViewModel : BaseViewModel
    {
        private Brush _eBoardBackgroundBrush;

        public Brush EBoardBackgroundBrush
        {
            get { return _eBoardBackgroundBrush; }
            set
            {
                _eBoardBackgroundBrush = value;
                OnPropertyChanged(nameof(EBoardBackgroundBrush));
            }

        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private ObservableCollection<ElementViewModel> elements;
        public ObservableCollection<ElementViewModel> Elements
        {
            get { return elements; }
            set
            {
                elements = value;
                OnPropertyChanged(nameof(Elements));
            }
        }


        public EBoardViewModel(string name, Brush background, int i)
        {
            _name = name;
            _eBoardBackgroundBrush = background;

            elements = new ObservableCollection<ElementViewModel>();

            ElementViewModel evm1 = new ElementViewModel(
                this,
                25,
                25,
                0,
                $"evm{i}",
                new SolidColorBrush(Colors.Gray),
                new TextBox() { Text = $"evm{i}_textbox" }
                );




            ElementViewModel evm2 = new ElementViewModel(
                this,
                250,
                25,
                0,
                $"evm{i}",
                new SolidColorBrush(Colors.Purple),
                new Border()
                {
                    Child = new Rectangle()
                    {
                        Width = 50,
                        Height = 50,
                        Fill = new SolidColorBrush(Colors.White)
                    }
                }
                ); ;

            ElementViewModel evm3 = new ElementViewModel(
                this,
                0,
                225,
                0,
                $"evm{i}",
                new SolidColorBrush(Colors.Blue),
                new Button() { Content = $"evm{i}_button", Style = Application.Current.FindResource("DefaultButtonStyle") as Style }
                );


            elements.Add(evm1);
            elements.Add(evm2);
            elements.Add(evm3);

        }
    }
}
