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
    public IElementContent ElementContent { get; set; }


    /// <summary>
    /// determines if ElementContent is
    /// of type ShapeManagement(false)
    /// or ContentManagement(true)
    /// </summary>
    public bool IsContentNotShape {  get; set; }


    /// <summary>
    /// Element ID, built using $"Element_{DateTime().Ticks} on first
    /// creation of an element.
    /// </summary>
    public string EID { get; set; }


    /// <summary>
    /// a string representation of an assembly, where the element type can be found
    /// </summary>
    public string ElementAssemblyString { get; set; }
    

    /// <summary>
    /// a string representation of an element type
    /// </summary>
    public string ElementTypeString { get; set; }


    /// <summary>
    /// the header text of an ElementView
    /// </summary>
    public string ElementHeader { get; set; }




    public ElementDataSet()
    {
        //_EBoardViewModel = new EBoardViewModel("new", new BorderManagement() { Width = 1000.0, Height = 500.0}, 100);

        //_ElementViewModel = new ElementViewModel(_EBoardViewModel);
    }


    public ElementDataSet(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
    {
        _EBoardViewModel = eBoardViewModel;
        _ElementViewModel = elementViewModel;
                   

        if (elementViewModel.IsShape)
        {
            BorderDataSet = new BorderDataSet(elementViewModel.ShapeViewModel.BorderManager);
            BrushDataSet = new BrushDataSet(elementViewModel.ShapeViewModel.BrushManager);
        }

        if (elementViewModel.IsContent)
        {
            BorderDataSet = new BorderDataSet(elementViewModel.ContentViewModel.BorderManager);
            BrushDataSet = new BrushDataSet(elementViewModel.ContentViewModel.BrushManager);
        }


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

        IsContentNotShape = ElementViewModel.IsContent;

        if (ElementViewModel.IsContent)
        {
            ElementContent = ElementViewModel.ContentViewModel.Control;
            ElementHeader = ElementViewModel.ContentViewModel.ElementHeaderText;

            ElementTypeString = ElementContent.GetType().FullName;

            BorderDataSet = new BorderDataSet(ElementViewModel.ContentViewModel.BorderManager);

            BrushDataSet = new BrushDataSet(ElementViewModel.ContentViewModel.BrushManager);
        }

        if (ElementViewModel.IsShape)
        {
            ElementContent = ElementViewModel.ShapeViewModel.Control;
            ElementHeader = ElementViewModel.ShapeViewModel.ElementHeaderText;

            ElementTypeString = ElementContent.GetType().FullName;

            BorderDataSet = new BorderDataSet(ElementViewModel.ShapeViewModel.BorderManager);

            BrushDataSet = new BrushDataSet(ElementViewModel.ShapeViewModel.BrushManager);
        }

        PlacementDataSet = new PlacementDataSet(ElementViewModel.PlacementManager);

        await Task.CompletedTask;
    }

    public async Task Initialize(string elementDataFileString)
    {
        if (IsContentNotShape)
        {
            ElementContent = new ContainerManagement();

        }
        else
        {
            ElementContent = new ShapeManagement();
        }


        if (ElementContent != null)
        {
            string element_folder = elementDataFileString.Replace(".xml", "\\");

            await ElementContent.Load($"{element_folder}", this);
        }

        await Task.CompletedTask;

    }
}
// EOF