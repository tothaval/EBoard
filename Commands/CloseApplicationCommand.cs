/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CloseApplicationCommand 
 * 
 *  command for application shutdown
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoard.Commands
{
    internal class CloseApplicationCommand : BaseCommand
    {

        public override void Execute(object? parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
// EOF