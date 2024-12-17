/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EboardDataSet 
 * 
 *  serializable helper class to store and retrieve EBoardViewModel(s) data to
 *  or from hard drive storage.
 *  
 *  Each Eboard has to store its own properties, as well as a list of all child
 *  elements - for now. Once Elements are getting more complex, it is probably
 *  better to trigger a separate serialization process/method for the elements and their data.
 *  
 *  It is vital to further development efforts, to be able to save and load Eboard contents
 *  asap, because it reduces time needed to setup the ui and test a specific or recently added 
 *  feature.
 */

using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;

namespace EBoard.Models
{
    [Serializable]
    [XmlRoot("EboardConfiguration")]
    public class EboardDataSet
    {

        [XmlIgnore]
        private EBoardViewModel _EBoardViewModel { get; set; }

        [XmlIgnore]
        public EBoardViewModel EBoardViewModel => _EBoardViewModel;

        public string EBID { get; set; }
        public string EBoardName { get; set; }
        public int EBoardDepth { get; set; }
        public double EBoardWidth { get; set; }
        public double EBoardHeight { get; set; }

        //[XmlIgnore]
        //private readonly FlatViewModel _flatViewModel;


        //[XmlArray("Elements")]
        //public ObservableCollection<ElementViewModel> Elements { get; set; }

        public EboardDataSet()
        {
            _EBoardViewModel = new EBoardViewModel("new", 1000.0, 500.0, 100);

            EBID = "-1";
            EBoardName = "new";
            EBoardDepth = 100;
            EBoardWidth = 1000.0;
            EBoardHeight = 500.0;
                
        }

        public EboardDataSet(EBoardViewModel eBoardViewModel)
        {
            _EBoardViewModel = eBoardViewModel;

            EBID = eBoardViewModel.EBID;
            EBoardName = eBoardViewModel.EBoardName;
            EBoardDepth = eBoardViewModel.EBoardDepth;
            EBoardWidth = eBoardViewModel.EBoardWidth;
            EBoardHeight = eBoardViewModel.EBoardHeight;                
        }


    }
}
// EOF