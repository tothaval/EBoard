using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.IOProcesses.DataSets.Interfaces;

public interface IElementDataSet
{
    public bool IsContentNotShape { get; set; }

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
    /// the header text of an ElementView
    /// </summary>
    public string ElementHeader { get; set; }


    public BorderDataSet BorderDataSet { get; set; }
    public BrushDataSet BrushDataSet { get; set; }
    public PlacementDataSet PlacementDataSet { get; set; }

    public void AddBorderDataSet(BorderDataSet borderDataSet);

    public void AddBrushDataSet(BrushDataSet brushDataSet);

    public void AddPlacementDataSet(PlacementDataSet placementDataSet);
}
