/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardConfigLoader 
 * 
 *  helper class for loading and deserialization of EBoardConfig
 *  on program startup
 */
using EBoard.IOProcesses.DataSets;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Xml.Serialization;

namespace EBoard.IOProcesses;

class EBoardConfigLoader
{
    private readonly IOProcessesInitializationManager _IOProcessesInitializationManager;


    private EboardConfig _EBoardConfig;
    public EboardConfig EBoardConfig => _EBoardConfig;


    public EBoardConfigLoader(IOProcessesInitializationManager iOProcesses) => _IOProcessesInitializationManager = iOProcesses;


    private async Task DeserializeEBoardConfig(string folderPath)
    {
        string filter = "*.xml";

        List<string> files = Directory.GetFiles(folderPath, filter, SearchOption.TopDirectoryOnly).ToList();

        foreach (string file in files)
        {
            if (file.EndsWith("eboardconfig.xml"))
            {
                var xmlSerializer = new XmlSerializer(typeof(EboardConfig));

                using (var writer = new StreamReader(file))
                {
                    try
                    {
                        var member = (EboardConfig)xmlSerializer.Deserialize(writer);

                        if (member != null)
                        {
                            _EBoardConfig = member;
                        }

                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }

    public async Task<EboardConfig> LoadEBoardConfig()
    {
        await DeserializeEBoardConfig(_IOProcessesInitializationManager.EBoardConfigFolder);

        if (EBoardConfig == null)
        {
            _EBoardConfig = new EboardConfig();  
        }

        _EBoardConfig.Plugins = await LoadPlugins();

        return _EBoardConfig;
    }

    private async Task<ObservableCollection<EBoardElementPluginBaseViewModel>> LoadPlugins()
    {
        ObservableCollection<EBoardElementPluginBaseViewModel> plugins = new();
        var assemblies = Directory.GetFiles("..\\..\\..\\ExternalPlugins", "*.dll");

        foreach (var item in assemblies)
        {
            var assembly = Assembly.LoadFrom(item);

            var baseviewmodeltype = assembly.GetExportedTypes().Where(x => x.BaseType.Equals(typeof(EBoardElementPluginBaseViewModel))).FirstOrDefault();

            var baseviewmodel = Activator.CreateInstance(baseviewmodeltype) as EBoardElementPluginBaseViewModel;

            if (baseviewmodel != null)
            {
                plugins.Add(baseviewmodel);
            }
        }

        return plugins;
    }
}
// EOF