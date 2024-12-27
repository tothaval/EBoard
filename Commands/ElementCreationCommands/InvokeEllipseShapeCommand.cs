using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

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
                ElementDataSet elementDataSet = new ElementDataSet();
                elementDataSet.EID = "-1";

                elementDataSet.ElementHeader = "Shape Element";

                elementDataSet.ElementTypeString = "EBoard.Models.ShapeManagement";

                elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
                elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
                elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

                // refactor this later into a separate function or class for shape creation, only call the
                // class or the function
                elementDataSet.ElementContent = new ShapeManagement(new Ellipse()
                {


                    Width = elementDataSet.BorderDataSet.Width,
                    Height = elementDataSet.BorderDataSet.Height,
                    Fill = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    Stroke = await elementDataSet.BrushDataSet.BorderColor.GetBrush(),
                    StrokeThickness = elementDataSet.BorderDataSet.BorderThickness.Left
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
