// <copyright file="PluginLoader.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK
{
    using EBoardConfigManager.Helper;
    using EBoardSDK.Plugins;
    using Serilog;
    using System.IO;
    using System.Reflection;

    public static class PluginLoader
    {
        public static async Task<IList<EBoardElementPluginBaseViewModel>> LoadPluginsAsync(string path)
        {
            IList<EBoardElementPluginBaseViewModel> plugins = [];

            var assemblies = Loader.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);

            if (assemblies == null || assemblies.Count == 0)
            {
                return plugins;
            }

            assemblies.Select(x => x).ToList().ForEach(dllfile =>
            {
                if (dllfile.Name.Equals("EBoardSDK.dll"))
                {
                    return;
                }

                if (!dllfile.Name.StartsWith("EboardElementPlugin")
                    && !dllfile.Name.StartsWith("EBoardElementPlugin")
                    && !dllfile.Name.StartsWith("EEP_")
                    && !dllfile.Name.StartsWith("EEP"))
                {
                    return;
                }

                try
                {
                    var assembly = Assembly.LoadFrom(dllfile.FullName);
                    var types = assembly.GetExportedTypes();

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
                        var baseviewmodel = Activator.CreateInstance(baseviewmodeltype) as EBoardElementPluginBaseViewModel;

                        if (baseviewmodel != null)
                        {
                            plugins.Add(baseviewmodel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.Message);

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

            return plugins;
        }
    }
}
