// <copyright file="PlacementDataSet.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  PlacementDataSet
 *
 *  serializable helper class to store and retrieve placement related data to
 *  or from hard drive storage.
 */
using System.Windows;
using System.Xml.Serialization;

namespace EBoardSDK.Models.DataSets;

[Serializable]
public class PlacementDataSet
{
    [XmlIgnore]
    private PlacementManagement _PlacementManagement = new PlacementManagement();

    public double Angle { get; set; }

    public Point Position { get; set; }

    public int Z { get; set; }

    public PlacementDataSet()
    {
    }

    public PlacementDataSet(PlacementManagement placementManagement)
    {
        this._PlacementManagement = placementManagement;

        if (this._PlacementManagement == null)
        {
            this._PlacementManagement = new PlacementManagement();
        }

        this.Angle = placementManagement.Angle;
        this.Position = placementManagement.Position;
        this.Z = placementManagement.Z;
    }
}

// EOF