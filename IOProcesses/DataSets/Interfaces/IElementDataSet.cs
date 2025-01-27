using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.IOProcesses.DataSets.Interfaces;

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


    public IPlugin Plugin { get; set; }


    public PlacementDataSet PlacementDataSet { get; set; }

}