/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PlacementDataSet 
 * 
 *  serializable helper class to store and retrieve placement related data to
 *  or from hard drive storage.
 */
using EBoard.Models;
using System.Windows;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets
{
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
            _PlacementManagement = placementManagement;

            if (_PlacementManagement == null)
            {
                _PlacementManagement = new PlacementManagement();
            }

            Angle = placementManagement.Angle;
            Position = placementManagement.Position;
            Z = placementManagement.Z;

        }
    }
}
// EOF