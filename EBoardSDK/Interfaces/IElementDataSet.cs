namespace EBoardSDK.Interfaces;

using EBoardSDK.Models.DataSets;

public interface IElementDataSet
{
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

    public IPlugin Plugin { get; set; }

    public BorderDataSet BorderDataSet { get; set; }
    public BrushDataSet BrushDataSet { get; set; }
    public PlacementDataSet PlacementDataSet { get; set; }

    public void AddBorderDataSet(BorderDataSet borderDataSet);

    public void AddBrushDataSet(BrushDataSet brushDataSet);

    public void AddPlacementDataSet(PlacementDataSet placementDataSet);
}
