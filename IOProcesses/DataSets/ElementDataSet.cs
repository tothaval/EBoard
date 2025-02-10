/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementDataSet 
 * 
 *  serializable helper class to store and retrieve ElementViewModel(s) data to
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

using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.IOProcesses.DataSets.Interfaces;
using EBoard.ViewModels;
using System.Xml.Serialization;

namespace EBoard.Models;

[Serializable]
[XmlRoot("ElementDataSet")]
public class ElementDataSet : IElementDataSet
{
    [XmlIgnore]
    private readonly EBoardViewModel _EBoardViewModel;

    [XmlIgnore]
    private ElementViewModel _ElementViewModel { get; set; }

    [XmlIgnore]
    public ElementViewModel ElementViewModel => _ElementViewModel;


    public BorderDataSet BorderDataSet { get; set; }
    public BrushDataSet BrushDataSet { get; set; }
    public PlacementDataSet PlacementDataSet { get; set; }



    [XmlIgnore]
    public IPlugin Plugin { get; set; }


    /// <summary>
    /// determines if ElementContent is
    /// of type ShapeManagement(false)
    /// or ContentManagement(true)
    /// </summary>
    //public bool IsContentNotShape { get; set; }


    /// <summary>
    /// Element ID, built using $"Element_{DateTime().Ticks} on first
    /// creation of an element.
    /// </summary>
    public string EID { get; set; }


    /// <summary>
    /// a string representation of an assembly, where the element type can be found
    /// </summary>
    public string PluginHeader { get; set; }


    // unification effort due to reduction, build abstraction layer can be removed and simplified
    // loading can be simplified, but element as plugin container needs some rework
    // every plugin can implement certain features, and maybe should be required to do so.
    public string PluginType { get; set; }
    public string AssemblyString { get; set; }


    /// <summary>
    /// a string representation of an element type
    /// </summary>
    public string ElementTypeString { get; set; }


    /// <summary>
    /// the header text of an ElementView
    /// </summary>
    //public string ElementHeader { get; set; }




    public ElementDataSet()
    {
        //_EBoardViewModel = new EBoardViewModel("new", new BorderManagement() { Width = 1000.0, Height = 500.0}, 100);

        //_ElementViewModel = new ElementViewModel(_EBoardViewModel);
    }


    public ElementDataSet(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        _EBoardViewModel = eBoardViewModel;
        _ElementViewModel = elementViewModel;
                
        BorderDataSet = new BorderDataSet(elementViewModel.Plugin.BorderManagement);
                
        BrushDataSet = new BrushDataSet(elementViewModel.Plugin.BrushManagement);

        PlacementDataSet = new PlacementDataSet(elementViewModel.PlacementManager);

        if (BorderDataSet == null)
        {
            BorderDataSet = new BorderDataSet();
        }

        if (BrushDataSet == null)
        {
            BrushDataSet = new BrushDataSet();
        }

        if (PlacementDataSet == null)
        {
            PlacementDataSet = new PlacementDataSet();
        }
    }


    public void AddBorderDataSet(BorderDataSet borderDataSet)
    {
        BorderDataSet = borderDataSet;

        if (BorderDataSet == null)
        {
            BorderDataSet = new BorderDataSet(new BorderManagement());
        }

    }

    public void AddBrushDataSet(BrushDataSet brushDataSet)
    {
        BrushDataSet = brushDataSet;

        if (BrushDataSet == null)
        {
            BrushDataSet = new BrushDataSet(new BrushManagement());
        }
    }
    public void AddPlacementDataSet(PlacementDataSet placementDataSet)
    {
        PlacementDataSet = placementDataSet;

        if (PlacementDataSet == null)
        {
            PlacementDataSet = new PlacementDataSet(new PlacementManagement());
        }
    }

    public async Task ConvertData()
    {
        EID = ElementViewModel.EID;

        Plugin = ElementViewModel.Plugin;

        PluginHeader = ElementViewModel.Plugin.PluginHeader;

        ElementTypeString = Plugin.GetType().FullName;
        PluginType = Plugin.GetType().FullName;
        AssemblyString = Plugin.GetType().AssemblyQualifiedName;

        BorderDataSet = new BorderDataSet(ElementViewModel.Plugin.BorderManagement);

        BrushDataSet = new BrushDataSet(ElementViewModel.Plugin.BrushManagement);

        PlacementDataSet = new PlacementDataSet(ElementViewModel.PlacementManager);

        await Task.CompletedTask;
    }

    public async Task Initialize(string elementDataFileString)
    {



        if (Plugin != null)
        {
            string element_folder = elementDataFileString.Replace(".xml", "\\");

            await Plugin.Load($"{element_folder}", this);
        }

        await Task.CompletedTask;

    }
}
// EOF