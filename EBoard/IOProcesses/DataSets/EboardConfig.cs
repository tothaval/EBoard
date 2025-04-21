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
using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using EBoard.ViewModels;
using System.Xml.Serialization;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;
using System.Collections.ObjectModel;

namespace EBoard.IOProcesses.DataSets;

[Serializable]
[XmlRoot("EboardConfiguration")]
public class EboardConfig
{
    [XmlIgnore]
    private readonly EBoardBrowserViewModel _EBoardBrowserViewModel;


    public int EBoardCount { get; set; }

    public int EBoardIndex { get; set; }

    public bool EBoardBrowserSwitch { get; set; }


    public BorderDataSet BorderDataSet { get; set; } = new(new BorderManagement());


    public BrushDataSet BrushDataSet { get; set; } = new(new BrushManagement());


    public PlacementDataSet PlacementDataSet { get; set; } = new(new PlacementManagement());

    [XmlIgnore]
    public ObservableCollection<EBoardElementPluginBaseViewModel> Plugins { get; set; } = new ObservableCollection<EBoardElementPluginBaseViewModel>();

    public EboardConfig() => _EBoardBrowserViewModel = new EBoardBrowserViewModel();

    public EboardConfig(MainViewModel mainViewModel)
    {
        _EBoardBrowserViewModel = mainViewModel.EBoardBrowserViewModel;

        EBoardCount = _EBoardBrowserViewModel.EBoards.Count;
        EBoardIndex = _EBoardBrowserViewModel.EBoards.IndexOf(_EBoardBrowserViewModel.SelectedEBoard);

        EBoardBrowserSwitch = mainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch;

        BorderDataSet = new BorderDataSet(mainViewModel.BorderManager);

        BrushDataSet = new BrushDataSet(mainViewModel.BrushManager);

        PlacementDataSet = new PlacementDataSet(mainViewModel.PlacementManager);

        if (BorderDataSet == null)
        {
            BorderDataSet = new BorderDataSet(new BorderManagement());
        }

        if (BrushDataSet == null)
        {
            BrushDataSet = new BrushDataSet(new BrushManagement());
        }

        if (PlacementDataSet == null)
        {
            PlacementDataSet = new PlacementDataSet(new PlacementManagement());
        }
    }
}
// EOF