/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  InvokePrototypeShapeElementCommand 
 * 
 *  command to instantiate a rectangle shape onto the selected eboard instance
 */
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System.Windows.Shapes;

namespace EBoard.Commands.ElementCreationCommands
{
    internal class InvokePrototypeShapeElementCommand : BaseCommand
    {


        private MainViewModel _MainViewModel;


        public InvokePrototypeShapeElementCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
        }


        public async override void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet elementDataSet = new ElementDataSet();
                elementDataSet.EID = "-1";

                elementDataSet.ElementHeader = "Shape Element";

                elementDataSet.ElementTypeString = "EBoard.Models.ShapeManagement";

                elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
                elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
                elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

                // refactor this later into a separate function or class for shape creation, only call the
                // class or the function
                elementDataSet.ElementContent = new ShapeManagement(new Rectangle()
                    {
                        Width = elementDataSet.BorderDataSet.Width,
                        Height = elementDataSet.BorderDataSet.Height,
                        Fill = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    Stroke = await elementDataSet.BrushDataSet.BorderColor.GetBrush(),
                    StrokeThickness = elementDataSet.BorderDataSet.BorderThickness.Left
                    });


                ElementViewModel evm = new ElementViewModel(_MainViewModel.EBoardBrowserViewModel.SelectedEBoard, elementDataSet)
                {
                    IsContent = false,
                    IsShape = true
                };
                               


                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
            }
        }
    }
}
// EOF