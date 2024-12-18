using EBoard.Interfaces;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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


        public override void Execute(object? parameter)
        {
            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet elementDataSet = new ElementDataSet();
                elementDataSet.EID = "-1";
                elementDataSet.X = 25;
                elementDataSet.Y = 25;
                elementDataSet.Z = 0;
                elementDataSet.ElementHeader = "Shape Element";
                elementDataSet.ElementHeight = 75.0;
                elementDataSet.ElementWidth = 225.0;
                elementDataSet.ElementTypeString = "EBoard.Models.ShapeManagement";

                elementDataSet.AddBrushManager(new BrushManagement());

                // refactor this later into a separate function or class for shape creation, only call the
                // class or the function
                elementDataSet.ElementContent = new ShapeManagement(new Ellipse()
                {
                    Width = elementDataSet.ElementWidth,
                    Height = elementDataSet.ElementHeight,
                    Fill = elementDataSet.BrushManager.Background,
                    Stroke = elementDataSet.BrushManager.Border,
                    StrokeThickness = elementDataSet.BrushManager.BorderThickness.Left
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
