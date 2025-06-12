// <copyright file="QuadValueSetupView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EBoardSDK.Controls.QuadValueSetup
{
    /// <summary>
    /// Interaktionslogik für QuadValueSetupView.xaml
    /// </summary>
    public partial class QuadValueSetupView : UserControl
    {
        public QuadValueSetupView()
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
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(QuadValueSetupView), new PropertyMetadata(null));

        public int Maximum
        {
            get { return (int)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(QuadValueSetupView), new PropertyMetadata(20));

        public string Value1
        {
            get { return (string)this.GetValue(Value1Property); }
            set { this.SetValue(Value1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Value1Property =
            DependencyProperty.Register("Value1", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("1"));

        public string Value2
        {
            get { return (string)this.GetValue(Value12Property); }
            set { this.SetValue(Value12Property, value); }
        }

        // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Value12Property =
            DependencyProperty.Register("Value2", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("2"));

        public string Value3
        {
            get { return (string)this.GetValue(Value3Property); }
            set { this.SetValue(Value3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Value3Property =
            DependencyProperty.Register("Value3", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("3"));

        public string Value4
        {
            get { return (string)this.GetValue(Value4Property); }
            set { this.SetValue(Value4Property, value); }
        }

        // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Value4Property =
            DependencyProperty.Register("Value4", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("4"));

        public ICommand OKCommand
        {
            get { return (ICommand)this.GetValue(OKCommandProperty); }
            set { this.SetValue(OKCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OKCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OKCommandProperty =
            DependencyProperty.Register("OKCommand", typeof(ICommand), typeof(QuadValueSetupView), new PropertyMetadata(null));
    }
}
