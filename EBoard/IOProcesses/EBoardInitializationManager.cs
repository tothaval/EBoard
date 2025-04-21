/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardInitializationManager 
 * 
 *  helper class for loading and deserialization of EBoardDataSet(s) and ElementDataSet(s)
 *  on program startup
 *  
 *  steps:
 *  1. load EBoardDataSet(s)
 *  2. for each EBoardDataSet load ElementDataSet(s)
 */

using EBoard.Models;
using EBoard.Utilities.Factories;
using EBoard.ViewModels;

using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using System.IO;
using System.Xml.Serialization;
using EBoard.IOProcesses.DataSets;
using EBoardSDK.Plugins;

namespace EBoard.IOProcesses;

internal class EBoardInitializationManager
{

    private readonly EBoardBrowserViewModel _EBoardBrowserViewModel;

    private readonly IOProcessesInitializationManager _IOProcessesInitializationManager;


    public EBoardInitializationManager(IOProcessesInitializationManager iOProcesses, MainViewModel mainViewModel)
    {
        _IOProcessesInitializationManager = iOProcesses;
        _EBoardBrowserViewModel = mainViewModel.EBoardBrowserViewModel;
    }

    private async Task LoadEboardDataSetFromFile(string fileName, string filter)
    {
        string eboard_folder = fileName.Replace(".xml", "\\");

        var xmlSerializer = new XmlSerializer(typeof(EboardDataSet));

        using (var reader = new StreamReader(fileName))
        {
            try
            {
                var member = (EboardDataSet)xmlSerializer.Deserialize(reader);

                if (member != null)
                {
                    EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(member);

                    eBoardViewModel = await LoadingEBoardElements(eBoardViewModel, eboard_folder, filter);

                    await _EBoardBrowserViewModel.AddEBoardViewModel(eBoardViewModel);
                }

            }
            catch (Exception ex)
            {
                throw new FileLoadException(ex.Message);
            }
        }
    }


    private async Task<EBoardViewModel> LoadElementDataSetFromFile(EBoardViewModel eBoardViewModel, string elementFileName, XmlSerializer elementDataSetSerializer)
    {
        try
        {
            if (elementFileName.Contains("element_") && elementFileName.EndsWith($".xml"))
            {
                var reader_elements = new StreamReader(elementFileName);

                var elementData = (ElementDataSet)elementDataSetSerializer.Deserialize(reader_elements);

                if (elementData != null)
                {
                    Type? type_PluginViewModel = Type.GetType(elementData.AssemblyString);

                    IEBoardElement? externalPlugin = Activator.CreateInstance(type_PluginViewModel) as IEBoardElement;

                    if (externalPlugin is not null)
                    {
                        elementData.Plugin = externalPlugin;

                        // datasets umbauen
                        externalPlugin.PluginHeader = elementData.PluginHeader;

                        App.Current.Resources.MergedDictionaries.Add(externalPlugin.ResourceDictionary);
                    }
                    else
                    {
                        IPlugin? sdkPlugin = Activator.CreateInstance(type_PluginViewModel) as IPlugin;

                        if (sdkPlugin is not null)
                        {
                            elementData.Plugin = sdkPlugin;
                            // datasets umbauen
                            sdkPlugin.PluginHeader = elementData.PluginHeader;
                        }
                    }

                    await elementData.Initialize(elementFileName);

                    eBoardViewModel.Elements.Add(
                        new ElementViewModel(
                            eBoardViewModel,
                            elementData
                            )
                        );
                }

            }
        }
        catch (Exception ex)
        {
            //throw new FileLoadException(ex.Message);
        }

        return eBoardViewModel;
    }


    private async Task<EBoardViewModel> LoadingEBoardElements(EBoardViewModel eBoardViewModel, string eboard_folder, string filter)
    {

        List<string> elements = Directory.GetFiles($"{eboard_folder}", filter, SearchOption.TopDirectoryOnly).ToList();

        var xmlSerializer_ElementDataSet = new XmlSerializer(typeof(ElementDataSet));

        for (int i = 0; i < elements.Count; i++)
        {
            eBoardViewModel = await LoadElementDataSetFromFile(eBoardViewModel, elements[i], xmlSerializer_ElementDataSet);
        }

        return eBoardViewModel;

    }


    private async Task LoadingEBoardDataSets(string folderPath)
    {
        string filter = "*.xml";

        List<string> files = Directory.GetFiles(folderPath, filter, SearchOption.TopDirectoryOnly).ToList();

        if (files != null && files.Count > 0)
        {
            foreach (string file in files)
            {
                if (file.Contains("eboard_") && file.EndsWith($".xml"))
                {
                    await LoadEboardDataSetFromFile(file, filter);
                }
            }
        }
    }


    public async Task LoadEBoardDataSets()
    {
        await LoadingEBoardDataSets(_IOProcessesInitializationManager.EBoardFolder);
    }
}
// EOF