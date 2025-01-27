using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Plugins;

public interface IPluginDataSet
{

    public BorderDataSet BorderDataSet { get; set; }

    public BrushDataSet BrushDataSet { get; set; }

    public string PluginAssemblyString { get; set; }


    public string PluginHeader { get; set; }


    public string PluginName { get; set; }


    public string PluginType { get; set; }


    public List<PluginData> PluginDataStorage { get; set; }


    public bool AddPluginData(PluginData pluginData);

}