using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using EBoard.Utilities.Factories;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace EBoard.Commands.ElementCreationCommands
{
    internal class InvokeElementCommand : BaseCommand
    {

        private MainViewModel _MainViewModel;


        public InvokeElementCommand(MainViewModel mainViewModel) => _MainViewModel = mainViewModel;


        public override void Execute(object? parameter)
        {
            string commandParameter = parameter as string;

            if (_MainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
            {
                ElementDataSet newElementDataSet = new ElementDataSet();

                if (commandParameter is null || commandParameter.Equals(string.Empty))
                {
                    newElementDataSet = ElementDataSetFactory.GetElementDataSet(
                        plugin: new StandardTextViewModel()
                        { Text = "Error: parameter string invalid or null" }
                        );
                }
                else
                {
                    newElementDataSet = ElementDataSetFactory.GetElementDataSetByCommandParameter( commandParameter );
                }



                ElementViewModel evm = new ElementViewModel(
                    _MainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                    newElementDataSet
                );

                _MainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(evm);
            }
        }
    }
}
