﻿/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainWindowLogoutBarViewModel 
 * 
 *  helper class for

 */
using EBoard.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EBoard.ViewModels
{
    class MainWindowLogoutBarViewModel : BaseViewModel
    {    
        public ICommand CloseCommand { get; }


        public MainWindowLogoutBarViewModel()
        {
            CloseCommand = new CloseApplicationCommand();
        }
    }
}
// EOF