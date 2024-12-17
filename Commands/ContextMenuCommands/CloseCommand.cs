/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CloseCommand 
 * 
 *  command for application shutdown
 */
using System.Windows;

namespace EBoard.Commands.ContextMenuCommands
{
    internal class CloseCommand : BaseCommand
    {

        public CloseCommand()
        {

        }


        public override void Execute(object? parameter)
        {
            Application.Current.Shutdown();
        }

    }
}
// EOF