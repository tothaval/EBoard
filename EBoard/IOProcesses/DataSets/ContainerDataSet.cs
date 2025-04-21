/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContainerDataSet 
 * 
 *  serializable helper class to store and retrieve content related data to
 *  or from hard drive storage.
 */
using EBoard.Models;

using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets;

[Serializable]
public class ContainerDataSet
{
    [XmlIgnore]
    private ElementDataSet _ElementDataSet { get; }
    
    [XmlIgnore]
    public IElementDataSet ElementDataSet { get; }
    
    public string UserControlType { get; set; }
    public string AssemblyString { get; set; }



    /// <summary>
    /// irgendwie überlegen, wie es noch getrennt werden kann zwischen
    /// tools und plugins, die nichts weiter speichern und laden müssen,
    /// sowie elementen mit content. diesen per interface mittels ioc
    /// laden, das element regelt die details per interface methode
    /// </summary>
    //public bool IsImplementingIElementContent { get; set; }


    public ObservableCollection<string> ContentDataStrings { get; set; }

    public ObservableCollection<double> ContentDataValues { get; set; }


    public ContainerDataSet()
    {
        
    }


    public ContainerDataSet(IElementDataSet elementDataSet)
    {
        ElementDataSet = elementDataSet;

        AssemblyString = elementDataSet.Plugin.Plugin.GetType().AssemblyQualifiedName;

        UserControlType = elementDataSet.Plugin.Plugin.GetType().FullName;

        ContentDataStrings = ((ContainerManagement)elementDataSet.Plugin).GetStringValues();

        //ContentDataValues = ((ContainerManagement)elementDataSet.ElementContent).ContentDataValues;


    }

    
}
// EOF