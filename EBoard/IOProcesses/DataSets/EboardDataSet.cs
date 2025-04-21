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
using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using EBoard.ViewModels;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using EBoardSDK.Models.DataSets;

namespace EBoard.Models;

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

    public BorderDataSet BorderDataSet { get; set; }
    public BrushDataSet BrushDataSet { get; set; }


    [XmlIgnore]
    public ObservableCollection<ElementViewModel> Elements { get; set; } = new ObservableCollection<ElementViewModel> { };


    public EboardDataSet()
    {
        //_EBoardViewModel = new EBoardViewModel("new", new BorderManagement() { Width = 1000.0, Height = 500.0}, 100);                            
    }

    public EboardDataSet(EBoardViewModel eBoardViewModel)
    {
        _EBoardViewModel = eBoardViewModel;

        EBID = eBoardViewModel.EBID;
        EBoardName = eBoardViewModel.EBoardName;
        EBoardDepth = eBoardViewModel.EBoardDepth;

        BorderDataSet = new BorderDataSet(eBoardViewModel.BorderManager);

        BrushDataSet = new BrushDataSet(eBoardViewModel.BrushManager);


        if (BorderDataSet == null)
        {
            BorderDataSet = new BorderDataSet(new BorderManagement());
        }

        if (BrushDataSet == null)
        {
            BrushDataSet = new BrushDataSet(new BrushManagement());
        }
    }


}
// EOF