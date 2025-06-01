using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EBoardSDK.Controls
{
    /// <summary>
    /// Interaktionslogik für SolidColorBrushSetup.xaml
    /// </summary>
    public partial class SolidColorBrushSetup : UserControl
    {
        public SolidColorBrushSetup()
        {
            this.InitializeComponent();
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
            get { return (ICommand)this.GetValue(OKCommandProperty); }
            set { this.SetValue(OKCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OKCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OKCommandProperty =
            DependencyProperty.Register("OKCommand", typeof(ICommand), typeof(SolidColorBrushSetup), new PropertyMetadata(null));
    }
}
