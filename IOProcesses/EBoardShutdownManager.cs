/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardShutdownManager 
 * 
 *  helper class for serialization and saving of EBoardConfig, EBoardDataSet(s) and ElementDataSet(s)
 *  before program termination
 *  
 *  steps:
 *  1. save EBoardConfig
 *  2. save EBoardDataSet(s)
 *  3. for each EBoardDataSet save ElementDataSet(s)
 */

using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace EBoard.IOProcesses;

internal class EBoardShutdownManager
{
    private readonly MainViewModel _MainViewModel;

    public EBoardShutdownManager(MainViewModel mainViewModel) => _MainViewModel = mainViewModel;


    public async Task Save()
    {
        IOProcessesInitializationManager iOProcesses = new IOProcessesInitializationManager();

        await SerializeEBoardConfig(iOProcesses.EBoardConfigFolder);

        await iOProcesses.CleanFolder();

        await SerializeEBoardDataSets(iOProcesses.EBoardFolder);
    }


    public async Task SerializeEBoardConfig(string saveFolderPath)
    {
        var xmlSerializer = new XmlSerializer(typeof(EboardConfig));
        EboardConfig eboardConfig = new EboardConfig(_MainViewModel);

        string path = $"{saveFolderPath}eboardconfig.xml";

        await using (var writer = new StreamWriter(path))
        {
            xmlSerializer.Serialize(writer, eboardConfig);
        }
    }


    public async Task SerializeEBoardDataSets(string saveFolderPath)
    {
        var xmlSerializer_EBoardDataSet = new XmlSerializer(typeof(EboardDataSet));

        ObservableCollection<EBoardViewModel> eBoardViewModels = _MainViewModel.EBoardBrowserViewModel.EBoards;

        if (eBoardViewModels != null && eBoardViewModels.Count > 0)
        {
            for (int i = 0; i < eBoardViewModels.Count; i++)
            {
                eBoardViewModels[i].DeselectElements();

                EboardDataSet dataSet = new EboardDataSet(eBoardViewModels[i]);

                string path = $"{saveFolderPath}eboard_{i}.xml";

                await using (var writer = new StreamWriter(path))
                {
                    xmlSerializer_EBoardDataSet.Serialize(writer, dataSet);
                }

                await ElementDataSetSerialization(saveFolderPath, eBoardViewModels, i);

            }
        }
    }


    private async Task CreateFolder(string saveFolderPath)
    {
        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }

        await Task.CompletedTask;
    }


    private async Task ElementContentDataSetSerialization(string saveFolderPath, ElementDataSet elementDataSet)
    {        
        await elementDataSet.Plugin.Save(saveFolderPath, elementDataSet);

        await Task.CompletedTask;
    }


    private async Task ElementDataSetSerialization(string saveFolderPath, ObservableCollection<EBoardViewModel> eBoardViewModels, int i)
    {
        string elementFolderPath = $"{saveFolderPath}eboard_{i}\\";

        await CreateFolder(elementFolderPath);

        var xmlSerializer_ElementDataSet = new XmlSerializer(typeof(ElementDataSet));

        for (int ii = 0; ii < eBoardViewModels[i].Elements.Count; ii++)
        {
            ElementDataSet elementDataSet = new ElementDataSet(eBoardViewModels[i], eBoardViewModels[i].Elements[ii]);

            await elementDataSet.ConvertData();

            string elementFilePath = $"{elementFolderPath}element_{ii}.xml";


            /// file is sometimes used by another process which leads to crashs, don't know if it is an visual studio issue
            /// on debugging or not, does not happen everytime.
            /// 
            /// maybe it is the loading process, that hasn't finished, gonna have to implement some sort of check if process
            /// is available or if it can be freed/closed to free the file.
            
            await using (var writer = new StreamWriter(elementFilePath))
            {
                xmlSerializer_ElementDataSet.Serialize(writer, elementDataSet);
            }

            string elementContentFolderPath = $"{elementFolderPath}element_{ii}\\";

            await CreateFolder(elementContentFolderPath);

            await ElementContentDataSetSerialization(elementContentFolderPath, elementDataSet);
        }


    }
}
// EOF