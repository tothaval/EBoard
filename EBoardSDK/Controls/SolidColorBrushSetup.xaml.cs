using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EBoardSDK.Controls
{
    /// <summary>
    /// Interaktionslogik für SolidColorBrushSetup.xaml
    /// </summary>
    public partial class SolidColorBrushSetup : UserControl
    {
        public SolidColorBrushSetup()
        {
            InitializeComponent();
        }

        //public SolidColorBrush ColorBrush
        //{
        //    get { return (SolidColorBrush)GetValue(SolidColorBrushProperty); }
        //    set { SetValue(SolidColorBrushProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SolidColorBrush.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SolidColorBrushProperty =
        //    DependencyProperty.Register("ColorBrush", typeof(SolidColorBrush), typeof(SolidColorBrushSetup), new PropertyMetadata(new SolidColorBrush()));

        public ICommand OKCommand
        {
            get { return (ICommand)GetValue(OKCommandProperty); }
            set { SetValue(OKCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OKCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OKCommandProperty =
            DependencyProperty.Register("OKCommand", typeof(ICommand), typeof(SolidColorBrushSetup), new PropertyMetadata(null));



    }
}
