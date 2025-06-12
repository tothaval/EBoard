// <copyright file="MainWindow.xaml.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK;

using EBoardSDK;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
    }

    public MainWindow(MainViewModel mainViewModel)
    {
        this.InitializeComponent();

        this.DataContext = mainViewModel;
    }

    private void SizeAndPositionUpdate()
    {
        ((MainViewModel)this.DataContext).PlacementManager.Position = new Point(this.Left, this.Top);

        ((MainViewModel)this.DataContext).BorderManagement.Width = this.ActualWidth;
        ((MainViewModel)this.DataContext).BorderManagement.Height = this.ActualHeight;
    }

    private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ((MainViewModel)this.DataContext).DeselectElements();

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }

    private void EboardMainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.SizeAndPositionUpdate();

        e.Handled = true;
    }

    private void EboardMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.SizeAndPositionUpdate();

        e.Handled = true;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Normal)
        {
            this.WindowState = WindowState.Maximized;
            this.Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Normalize";
        }
        else
        {
            this.WindowState = WindowState.Normal;
            this.Background = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Maximize";
        }
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = System.Windows.WindowState.Minimized;
    }
}

// EOF