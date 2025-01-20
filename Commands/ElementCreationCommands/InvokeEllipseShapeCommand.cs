/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  InvokeEllipseShapeCommand 
 * 
 *  command to instantiate a ellipse shape onto the selected eboard instance
 */
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.Utilities.Factories;
using EBoard.ViewModels;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;

namespace EBoard.Commands.ElementCreationCommands
{
    internal class InvokeEllipseShapeCommand : BaseCommand
    {

        private MainViewModel _MainViewModel;


        public InvokeEllipseShapeCommand(MainViewModel mainViewModel)
        {
            _MainViewModel = mainViewModel;
        }


        public override async void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet newElementDataSet = ElementDataSetFactory.GetShapeElementDataSet();

                newElementDataSet.ElementContent = new ShapeManagement(new Ellipse()
                {
                    Width = newElementDataSet.BorderDataSet.Width,
                    Height = newElementDataSet.BorderDataSet.Height,
                    Fill = await newElementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    Stroke = await newElementDataSet.BrushDataSet.BorderColor.GetBrush(),
                    StrokeThickness = newElementDataSet.BorderDataSet.BorderThickness.Left
                });


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