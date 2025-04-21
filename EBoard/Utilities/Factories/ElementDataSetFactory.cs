using EBoard.IOProcesses.DataSets;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.StandardText;
using System.Reflection;
using System.Windows.Controls;

namespace EBoard.Utilities.Factories;

public static class ElementDataSetFactory
{
    public static ElementDataSet GetElementDataSet(IPlugin? plugin = null, IEBoardElement? externalPlugin = null)
    {
        ElementDataSet elementDataSet = new ElementDataSet();

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        if (plugin is not null)
        {
            elementDataSet.Plugin = plugin;
        }

        if (externalPlugin is not null)
        {
            elementDataSet.Plugin = externalPlugin;
        }


        return elementDataSet;
    }


    internal static ElementDataSet GetElementDataSetByCommandParameter(string commandParameter)
    {
        string[] shards = commandParameter.Split('.');

        string eboardSDKPlugins = "EBoardSDK.Plugins.";

        Assembly assembly = Assembly.GetAssembly(typeof(EBoardSDK.Models.BorderManagement))!;

        string pluginView = $"{eboardSDKPlugins}{shards[0]}.{shards[1]}.{shards[1]}View, {assembly}";

        string pluginViewModel = $"{eboardSDKPlugins}{shards[0]}.{shards[1]}.{shards[1]}ViewModel, {assembly}";

        Type? type_PluginView = Type.GetType(pluginView);
        Type? type_PluginViewModel = Type.GetType(pluginViewModel);

        if (type_PluginView == null || type_PluginViewModel == null)
        {
            //ArgumentNullException.ThrowIfNull(type_PluginView);
            //ArgumentNullException.ThrowIfNull(type_PluginViewModel);

            return ElementDataSetFactory.GetElementDataSet(plugin: new StandardTextViewModel()
            {
                Text = "Plugin Instantiation Error"
            });
        }

        UserControl? pluginViewInstance = (UserControl)Activator.CreateInstance(type_PluginView)!;
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
