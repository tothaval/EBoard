/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  InvokeEmptyElementCommand 
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
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Commands.ElementCreationCommands
{
    internal class InvokeEmptyElementCommand : BaseCommand
    {

        private MainViewModel _MainViewModel;


        public InvokeEmptyElementCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
        }


        public override void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet elementDataSet = new ElementDataSet();
                elementDataSet.EID = "-1";

                elementDataSet.ElementHeader = string.Empty;

                elementDataSet.ElementTypeString = "EBoard.Models.ContainerManagement";


                GradientStopCollection gradientStops = new GradientStopCollection();
                gradientStops.Add(new GradientStop(Colors.AliceBlue, 0.0));
                gradientStops.Add(new GradientStop(Colors.Navy, 0.5));

                Point start = new Point(0, 0);
                Point end = new Point(0.5, 1);

                BrushDataSet brushDataSet = new BrushDataSet(new BrushManagement());

                brushDataSet.BackgroundColor = new ColorDataSet(new LinearGradientBrush(gradientStops, start, end));
             
                elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
                elementDataSet.AddBrushDataSet(brushDataSet);
                elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));


                elementDataSet.ElementContent = new ContainerManagement(new Label()
                {
                    Content = $"\t\t\t",
                    Background = new SolidColorBrush(Colors.Transparent)
                });
                

                ElementViewModel evm = new ElementViewModel(
                    _MainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                    elementDataSet
                );

                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
            }

        }
    }
}
// EOF