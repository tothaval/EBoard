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
using EBoard.Plugins;
using EBoard.ViewModels;
using System.Xml.Serialization;

namespace EBoard.Models;

[Serializable]
[XmlRoot("ElementDataSet")]
public class ElementDataSet : IElementDataSet
{

    [XmlIgnore]
    private readonly EBoardViewModel _EBoardViewModel;


    /// <summary>
    /// Element ID, built using $"Element_{DateTime().Ticks} on first
    /// creation of an element.
    /// </summary>
    public string EID { get; set; }


    [XmlIgnore]
    private ElementViewModel _ElementViewModel { get; set; }


    [XmlIgnore]
    public ElementViewModel ElementViewModel => _ElementViewModel;


    public PlacementDataSet PlacementDataSet { get; set; } = new PlacementDataSet();


    [XmlIgnore]
    public IPlugin Plugin { get; set; }


    //[XmlIgnore]
    //public IPluginDataSet PluginDataSet { get; set; }


    public ElementDataSet()
    {

    }


    public ElementDataSet(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        _EBoardViewModel = eBoardViewModel;
        _ElementViewModel = elementViewModel;
                
        PlacementDataSet = new PlacementDataSet(elementViewModel.PlacementManager);

        if (PlacementDataSet == null)
        {
            PlacementDataSet = new PlacementDataSet();
        }
    }


    public async Task ConvertData()
    {
        EID = ElementViewModel.EID;

        Plugin = ElementViewModel.Plugin;

        PlacementDataSet = new PlacementDataSet(ElementViewModel.PlacementManager);

        await Task.CompletedTask;
    }

}
// EOF