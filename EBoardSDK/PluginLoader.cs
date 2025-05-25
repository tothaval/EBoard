using EBoardConfigManager.Helper;
using EBoardConfigManager.Models;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoardSDK
{
    public static class PluginLoader
    {
        public static Task<IList<EBoardElementPluginBaseViewModel>> LoadPlugins(string path)
        {
            var assemblies = Loader.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

            IList<EBoardElementPluginBaseViewModel> plugins = [];

            assemblies.Select(x => x).ToList().ForEach(async dllfile => 
            {
                try
                {
                    var assembly = Assembly.LoadFrom(dllfile.FullName);
                    var types = assembly.GetExportedTypes();

                    // TODO ggf ein Interface oder Datenklasse mit Liste von Plugins, um Pluginbibliotheken einlesen zu können. nur wie sortieren?

                    var baseType = types.Where(dlltype => dlltype.BaseType != null && dlltype.BaseType.Equals(typeof(EBoardElementPluginBaseViewModel))).Any();

                    if (!baseType)
                    {
                        return;
                    }

                    var baseviewmodeltype = types.Where(dlltype => dlltype.BaseType!.Equals(typeof(EBoardElementPluginBaseViewModel))).FirstOrDefault();

                    if (baseviewmodeltype == null)
                    {
                        return;
                    }

                    try
                    {

                        var baseviewmodel = Activator.CreateInstance(baseviewmodeltype!) as EBoardElementPluginBaseViewModel;

                        if (baseviewmodel != null)
                        {
                            plugins.Add(baseviewmodel);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);

                        throw;
                    }
                }
                catch (Exception)
                {
                    // TODO log events of try catch block, specify what dll was skipped
                    // prevent not updated external dlls from crashing the application
                    // rethrow or pass exception to caller

                    return;
                }

            });

            return Task.FromResult(plugins);
        }
    }
}
