using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EBoard.Plugins;

[Serializable]
[XmlRoot("PluginDataSet")]
public class PluginDataSet : IPluginDataSet
{

    public BorderDataSet BorderDataSet { get; set; }


    public BrushDataSet BrushDataSet { get; set; }


    public string PluginAssemblyString { get; set; }


    public string PluginHeader { get; set; }


    public string PluginName { get; set; }


    public string PluginType { get; set; }


    [XmlArray("PluginDataStorage")]
    public List<PluginData> PluginDataStorage { get; set; } = new List<PluginData>();


    [XmlIgnore]
    public IPlugin Plugin { get; set; }


    public PluginDataSet()
    {
    }


    public PluginDataSet(IPlugin plugin)
    {
        Plugin = plugin;

        BorderDataSet = new BorderDataSet(plugin.BorderManagement);

        BrushDataSet = new BrushDataSet(plugin.BrushManagement);

        PluginAssemblyString = plugin.GetType().AssemblyQualifiedName;


        PluginHeader = plugin.PluginHeader;

        PluginName = plugin.PluginName;

        PluginType = plugin.GetType().FullName;

        if (plugin.PluginDataSet is not null)
        {
            if (plugin.PluginDataSet.PluginDataStorage is not null)
            {
                PluginDataStorage = plugin?.PluginDataSet?.PluginDataStorage;
                return;
            }

        }

        PluginDataStorage = new List<PluginData>();


    }


    public bool AddPluginData(PluginData pluginData)
    {
        if (PluginDataStorage.Contains(pluginData))
        {
            return false;
        }

        PluginDataStorage.Add(pluginData);

        return true;
    }
}