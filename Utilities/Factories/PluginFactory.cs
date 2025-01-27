using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EBoard.Utilities.Factories
{
    internal static class PluginFactory
    {
        internal static IPlugin? GetPluginByCommand(string pluginCategory, string command)
        {
            string pluginView = string.Concat($"EBoard.Plugins.{pluginCategory}.{command}.{command}View");
            string pluginViewModel = string.Concat($"EBoard.Plugins.{pluginCategory}.{command}.{command}ViewModel");

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
                pluginViewInstance.DataContext = plugin;

                return plugin;
            }

            return new StandardTextViewModel()
            {
                Text = "Plugin Instantiation Error"
            };
        }
    }
}
