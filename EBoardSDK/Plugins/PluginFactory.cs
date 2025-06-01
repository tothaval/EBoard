namespace EBoardSDK.Plugins;

using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.StandardText;

using System.Windows.Controls;

internal static class PluginFactory
{
    internal static IPlugin? GetPluginByCommand(string pluginCategory, string command)
    {
        string pluginView = string.Concat($"EBoardSDK.Plugins.{pluginCategory}.{command}.{command}View");
        string pluginViewModel = string.Concat($"EBoardSDK.Plugins.{pluginCategory}.{command}.{command}ViewModel");

        Type? type_PluginView = Type.GetType(pluginView);
        Type? type_PluginViewModel = Type.GetType(pluginViewModel);

        if (type_PluginView is null || type_PluginViewModel is null)
        {
            return null;
        }

        UserControl? pluginViewInstance = (UserControl)Activator.CreateInstance(type_PluginView);
        IPlugin? plugin = Activator.CreateInstance(type_PluginViewModel) as IPlugin;

        if (pluginViewInstance is not null && plugin is not null)
        {
            // TODO plugin is not the viewmodel
            pluginViewInstance.DataContext = plugin;

            return plugin;
        }

        return new StandardTextViewModel()
        {
            Text = "Plugin Instantiation Error",
        };
    }
}
