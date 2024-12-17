using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
