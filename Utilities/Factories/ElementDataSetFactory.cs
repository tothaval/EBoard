using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.IOProcesses.DataSets.Interfaces;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.CodeDom;
using EBoard.Plugins.Elements.StandardText;

namespace EBoard.Utilities.Factories;

public static class ElementDataSetFactory
{    
    public static ElementDataSet GetElementDataSet(IPlugin? plugin = null)
    {
        ElementDataSet elementDataSet = new ElementDataSet();
     
        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        if (plugin is not null)
        {
            elementDataSet.Plugin = plugin;
        }

        return elementDataSet;
    }


    internal static ElementDataSet GetElementDataSetByCommandParameter(string commandParameter)
    {
        string[] shards = commandParameter.Split('.');

        string pluginView = string.Concat($"EBoard.Plugins.{shards[0]}.{shards[1]}.{shards[1]}View");
        string pluginViewModel = string.Concat($"EBoard.Plugins.{shards[0]}.{shards[1]}.{shards[1]}ViewModel");

        Type? type_PluginView = Type.GetType(pluginView);
        Type? type_PluginViewModel = Type.GetType(pluginViewModel);
    
        UserControl? pluginViewInstance = (UserControl)Activator.CreateInstance(type_PluginView);
        IPlugin? plugin = Activator.CreateInstance(type_PluginViewModel) as IPlugin;

        if (pluginViewInstance is not null && plugin is not null)
        {
            pluginViewInstance.DataContext = plugin;

            plugin.PluginHeader = shards[2];
            
            ElementDataSet elementDataSet = GetElementDataSet(plugin);

            return elementDataSet;
        }

        return ElementDataSetFactory.GetElementDataSet(plugin: new StandardTextViewModel()
        {
            Text = "Plugin Instantiation Error"
        });

    }

    //public static class FactoryPattern<K, T> where T : class, K, new()
    //{
    //    public static K GetInstance()
    //    {
    //        K objK;

    //        objK = new T();

    //        return objK;
    //    }
    //}


}
