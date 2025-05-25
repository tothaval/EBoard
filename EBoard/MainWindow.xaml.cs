using EBoard.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.DataContext = mainViewModel;
        }


        private void SizeAndPositionUpdate()
        {
            ((MainViewModel)DataContext).PlacementManager.Position = new Point(Left, Top);

            ((MainViewModel)DataContext).BorderManager.Width = ActualWidth;
            ((MainViewModel)DataContext).BorderManager.Height = ActualHeight;
        }


        private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((MainViewModel)DataContext).DeselectElements();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }


        }


        private void EboardMainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SizeAndPositionUpdate();

            e.Handled = true;
        }


        private void EboardMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeAndPositionUpdate();

            e.Handled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
                Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Normalize";
            }
            else
            {
                WindowState = WindowState.Normal;
                Background = new SolidColorBrush(Colors.Transparent);
                Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Maximize";
            }

        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
// EOF