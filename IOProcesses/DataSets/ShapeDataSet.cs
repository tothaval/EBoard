/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeDataSet 
 * 
 *  serializable helper class to store and retrieve shape related data to
 *  or from hard drive storage.
 */
using EBoard.Models;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets
{
    [Serializable]
    public class ShapeDataSet
    {
        [XmlIgnore]
        private ElementDataSet _ElementDataSet { get; }

        [XmlIgnore]
        public ElementDataSet ElementDataSet { get; }


        public string ShapeType { get; set; }

        public ShapeDataSet()
        {

        }


        public ShapeDataSet(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;
            
            ShapeType = ((Shape)elementDataSet.ElementContent.Element).GetType().FullName;
        }
    }

}
// EOF