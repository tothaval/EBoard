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
namespace EBoard.IOProcesses.DataSets;

using EBoard.ViewModels;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("ElementDataSet")]
public class ElementDataSet : IElementDataSet
{
    [XmlIgnore]
    private readonly EBoardViewModel eBoardViewModel;

    [XmlIgnore]
    private readonly ElementViewModel elementViewModel;

    public ElementDataSet()
    {
    }

    public ElementDataSet(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        this.eBoardViewModel = eBoardViewModel;
        this.elementViewModel = elementViewModel;

        this.BorderDataSet = new BorderDataSet(elementViewModel.Plugin.BorderManagement);

        this.BrushDataSet = new BrushDataSet(elementViewModel.Plugin.BrushManagement);

        this.PlacementDataSet = new PlacementDataSet(elementViewModel.PlacementManager);

        if (this.BorderDataSet == null)
        {
            this.BorderDataSet = new BorderDataSet();
        }

        if (this.BrushDataSet == null)
        {
            this.BrushDataSet = new BrushDataSet();
        }

        if (this.PlacementDataSet == null)
        {
            this.PlacementDataSet = new PlacementDataSet();
        }
    }

    [XmlIgnore]
    public ElementViewModel ElementViewModel => this.elementViewModel;

    public BorderDataSet BorderDataSet { get; set; }

    public BrushDataSet BrushDataSet { get; set; }

    public PlacementDataSet PlacementDataSet { get; set; }

    [XmlIgnore]
    public IPlugin Plugin { get; set; }

    /// <summary>
    /// Gets or sets Element ID, built using $"Element_{DateTime().Ticks} on first
    /// creation of an element.
    /// </summary>
    public string EID { get; set; }

    /// <summary>
    /// Gets or sets a string representation of an assembly, where the element type can be found
    /// </summary>
    public string PluginHeader { get; set; }

    /// <summary>
    /// Gets or sets unification effort due to reduction, build abstraction layer can be removed and simplified
    /// loading can be simplified, but element as plugin container needs some rework
    /// every plugin can implement certain features, and maybe should be required to do so.
    /// </summary>
    public string PluginType { get; set; }

    /// <summary>
    /// Gets or sets.
    /// </summary>
    public string AssemblyString { get; set; }

    /// <summary>
    /// Gets or sets a string representation of an element type.
    /// </summary>
    public string ElementTypeString { get; set; }

    /// <summary>
    /// Sets the BorderDataSet parameter, if null creates a new one.
    /// </summary>
    /// <param name="borderDataSet">Apply this BorderDataSet to ElementDataSet</param>
    public void AddBorderDataSet(BorderDataSet borderDataSet)
    {
        this.BorderDataSet = borderDataSet;

        if (this.BorderDataSet == null)
        {
            this.BorderDataSet = new BorderDataSet(new BorderManagement());
        }
    }

    /// <summary>
    /// Gets or sets.
    /// </summary>
    public void AddBrushDataSet(BrushDataSet brushDataSet)
    {
        this.BrushDataSet = brushDataSet;

        if (this.BrushDataSet == null)
        {
            this.BrushDataSet = new BrushDataSet(new BrushManagement());
        }
    }

    /// <summary>
    /// Gets or sets.
    /// </summary>
    public void AddPlacementDataSet(PlacementDataSet placementDataSet)
    {
        this.PlacementDataSet = placementDataSet;

        if (this.PlacementDataSet == null)
        {
            this.PlacementDataSet = new PlacementDataSet(new PlacementManagement());
        }
    }

    /// <summary>
    /// Gets or sets.
    /// </summary>
    public async Task ConvertData()
    {
        this.EID = this.ElementViewModel.EID;

        this.Plugin = this.ElementViewModel.Plugin;

        this.PluginHeader = this.ElementViewModel.Plugin.PluginHeader;

        this.ElementTypeString = this.Plugin.GetType().FullName;
        this.PluginType = this.Plugin.GetType().FullName;
        this.AssemblyString = this.Plugin.GetType().AssemblyQualifiedName;

        this.BorderDataSet = new BorderDataSet(this.ElementViewModel.Plugin.BorderManagement);

        this.BrushDataSet = new BrushDataSet(this.ElementViewModel.Plugin.BrushManagement);

        this.PlacementDataSet = new PlacementDataSet(this.ElementViewModel.PlacementManager);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Gets or sets.
    /// </summary>
    public async Task Initialize(string elementDataFileString)
    {
        if (this.Plugin != null)
        {
            string element_folder = elementDataFileString.Replace(".xml", "\\");

            await this.Plugin.Load($"{element_folder}");
        }

        await Task.CompletedTask;
    }
}

// EOF