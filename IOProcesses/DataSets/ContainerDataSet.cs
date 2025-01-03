﻿/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContainerDataSet 
 * 
 *  serializable helper class to store and retrieve content related data to
 *  or from hard drive storage.
 */
using EBoard.Models;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
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
        
        public string UserControlType { get; set; }
        public string AssemblyString { get; set; }




        public ObservableCollection<string> ContentDataStrings { get; set; }

        public ObservableCollection<double> ContentDataValues { get; set; }


        public ContainerDataSet()
        {
            
        }


        public ContainerDataSet(ElementDataSet elementDataSet)
        {
            ElementDataSet = elementDataSet;

            AssemblyString = elementDataSet.ElementContent.Element.GetType().AssemblyQualifiedName;

            UserControlType = elementDataSet.ElementContent.Element.GetType().FullName;

            ContentDataStrings = ((ContainerManagement)elementDataSet.ElementContent).GetStringValues();

            //ContentDataValues = ((ContainerManagement)elementDataSet.ElementContent).ContentDataValues;


        }

        
    }
}
// EOF