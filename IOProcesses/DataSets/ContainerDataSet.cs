using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets
{
    [Serializable]
    public class ContainerDataSet
    {
        [XmlIgnore]
        private ElementDataSet _ElementDataSet { get; }
        
        [XmlIgnore]
        public ElementDataSet ElementDataSet { get; }




        public ContainerDataSet()
        {
            
        }


        public ContainerDataSet(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;
        }

        
    }
}
