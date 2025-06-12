namespace EEP_Drives;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// Interaktionslogik für DriveInfoView.xaml
/// </summary>
public partial class DriveInfoView : UserControl
{
    public DriveInfoView()
    {
        this.InitializeComponent();
    }

    public ICommand DriveButtonCommand
    {
        get { return (ICommand)this.GetValue(DriveButtonCommandProperty); }
        set { this.SetValue(DriveButtonCommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DriveButtonCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DriveButtonCommandProperty =
        DependencyProperty.Register("DriveButtonCommand", typeof(ICommand), typeof(DriveInfoView), new PropertyMetadata(null));

}
