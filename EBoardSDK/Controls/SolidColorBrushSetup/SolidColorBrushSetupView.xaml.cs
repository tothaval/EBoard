// <copyright file="SolidColorBrushSetupView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

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

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(SolidColorBrushSetup), new PropertyMetadata(null));


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
