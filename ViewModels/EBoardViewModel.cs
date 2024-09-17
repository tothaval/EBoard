using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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


        private int _EBoardDepth;
        public int EBoardDepth
        {
            get { return _EBoardDepth; }
            set
            {
                _EBoardDepth = value;
                OnPropertyChanged(nameof(EBoardDepth));
            }
        }


        private double _EBoardHeight;
        public double EBoardHeight
        {
            get { return _EBoardHeight; }
            set
            {
                _EBoardHeight = value;
                OnPropertyChanged(nameof(EBoardHeight));
            }
        }


        private double _EBoardWidth;
        public double EBoardWidth
        {
            get { return _EBoardWidth; }
            set
            {
                _EBoardWidth = value;
                OnPropertyChanged(nameof(EBoardWidth));
            }
        }


        private string _EBoardName;
        public string EBoardName
        {
            get { return _EBoardName; }
            set
            {
                _EBoardName = value;
                OnPropertyChanged(nameof(EBoardName));
            }
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


        public EBoardViewModel(string name, double width = 0, double height = 0, int depth = 0)
        {
            EBoardName = name;
            EBoardDepth = depth;
            EBoardHeight = height;
            EBoardWidth = width;

            if (name.Equals(""))
            {
                name = "eboard";
            }

            if (width == 0)
            {   
                EBoardWidth = 800;
            }
            if (height == 0)
            {
                EBoardHeight = 640;
            }
            if (depth == 0)
            {
                EBoardDepth = 64;
            }

            _eBoardBackgroundBrush = new SolidColorBrush(Colors.CornflowerBlue);

            elements = new ObservableCollection<ElementViewModel>();

            ElementViewModel evm1 = new ElementViewModel(
                this,
                25,
                25,
                0,
                $"evm",
                new SolidColorBrush(Colors.Gray),
                new TextBox() { Text = $"evm_textbox" }
                );




            ElementViewModel evm2 = new ElementViewModel(
                this,
                250,
                25,
                0,
                $"evm",
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
                $"evm",
                new SolidColorBrush(Colors.Blue),
                new Button() { Content = $"evm_textbox", Style = Application.Current.FindResource("DefaultButtonStyle") as Style }
                );


            elements.Add(evm1);
            elements.Add(evm2);
            elements.Add(evm3);
        }
    }
}
