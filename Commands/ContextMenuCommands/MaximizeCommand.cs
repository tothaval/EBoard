using System.Windows;
using System.Windows.Media;

namespace EBoard.Commands.ContextMenuCommands
{
    internal class MaximizeCommand : BaseCommand
    {

        public MaximizeCommand()
        {
            Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Maximize";
        }

        public override void Execute(object? parameter)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;


            if (mainWindow.WindowState == WindowState.Normal)
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
                Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Normalize";
            }
            else
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.Background = new SolidColorBrush(Colors.Transparent);
                Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Maximize";
            }
        }


    }
}
// EOF