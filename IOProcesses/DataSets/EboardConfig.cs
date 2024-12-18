/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EboardConfig 
 * 
 *  serializable helper class to store and retrieve Eboard configuration data to
 *  or from hard drive storage.
 *  
 *  Each Eboard has to store its own properties, as well as a list of all child
 *  elements - for now. Once Elements are getting more complex, it is probably
 *  better to trigger a separate serialization process/method for the elements and their data.
 *  
 *  It is vital to further development efforts, to be able to save and load Eboard contents
 *  asap, because it reduces time needed to setup the ui and test a specific or recently added 
 *  feature.
 *  
 *  Eboard:
 *  EboardConfigDataSet stores 
 *  EboardMainConfiguration plus
 *  n EboardDataSet, each stores
 *  Eboard properties plus
 *  n ElementDataSet, each stores
 *  Element properties.
 *  
 *  
 */

using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets
{
    [Serializable]
    [XmlRoot("EboardConfiguration")]
    public class EboardConfig
    {
        [XmlIgnore]
        private readonly EBoardBrowserViewModel _EBoardBrowserViewModel;


        public int EBoardCount { get; set; }

        public int EBoardIndex { get; set; }

        public bool EBoardBrowserSwitch { get; set; }

        public Point EBoardPosition { get; set; }


        public double EBoardHeight { get; set; }


        public double EBoardWidth { get; set; }

        public BrushDataSet EBoardMainWindowBrushManager { get; set; }

        //public string FlatNotes { get; set; }



        //[XmlArray("Elements")]
        //public ObservableCollection<ElementViewModel> Elements { get; set; }

        public EboardConfig()
        {
            _EBoardBrowserViewModel = new EBoardBrowserViewModel();                
        }


        public EboardConfig(MainViewModel mainViewModel)
        {
            _EBoardBrowserViewModel = mainViewModel.EBoardBrowserViewModel;

            EBoardCount = _EBoardBrowserViewModel.EBoards.Count;
            EBoardIndex = _EBoardBrowserViewModel.EBoards.IndexOf(_EBoardBrowserViewModel.SelectedEBoard);

            EBoardBrowserSwitch = mainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch;

            EBoardPosition = new Point(mainViewModel.PositionX, mainViewModel.PositionY);

            EBoardHeight = mainViewModel.EBoardHeight;
            EBoardWidth = mainViewModel.EBoardWidth;

            EBoardMainWindowBrushManager = new BrushDataSet(mainViewModel.BrushManager);
        }
    }
}
// EOF