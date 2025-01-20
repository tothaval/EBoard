/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  InvokePrototypeElementCommand 
 * 
 *  command for development aid
 *  
 *  invokes a standardized element to test and develop
 *  the containerization and its interactions within the program, this includes
 *  saving, loading of elements, movement of elements between EBoards and editing
 *  of elements. if it works as intended, it proves that any element type, better any content
 *  within the standard element type can be manipulated equally, independend of content. 
 *  
 *  shapes need their own standard container, because user controls and shapes are handled
 *  different by WPF. in previous iterations of the concept EBoard is based upon, it often
 *  led to huge classes, because of the shape logic and container logic being different.
 *  and if the data type is chosen to be of the more basic types, filtering becomes necessary
 *  and a lot of type casting becomes mandatory, which adds complexity but not much else.
 */
using EBoard.Models;
using EBoard.ViewModels;
using System.Windows.Controls;
using System.Windows;
using EBoard.IOProcesses.DataSets;
using EBoard.Utilities.Factories;

namespace EBoard.Commands.ElementCreationCommands
{
    internal class InvokePrototypeElementCommand : BaseCommand
    {

        private MainViewModel _MainViewModel;


        public InvokePrototypeElementCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet newElementDataSet = ElementDataSetFactory.GetContentElementDataSet(
                        elementContent: new ContainerManagement(new TextBox()
                        {
                             Text = $"evm_textbox",
                             AcceptsReturn = true,
                             AcceptsTab = true,
                             TextWrapping = TextWrapping.Wrap
                        }));

                ElementViewModel evm = new ElementViewModel(
                    _MainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                    newElementDataSet
                );

                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
            }

        }
    }
}
// EOF