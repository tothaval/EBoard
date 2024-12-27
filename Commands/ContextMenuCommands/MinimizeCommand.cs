/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MinimizeCommand 
 * 
 *  command to minimize mainwindow
 */
using System.Windows;

namespace EBoard.Commands.ContextMenuCommands
{
    internal class MinimizeCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainWindow.WindowState = System.Windows.WindowState.Minimized;
        }
    }
}
// EOF