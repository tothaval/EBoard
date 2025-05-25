using EBoardSDK.Models;

using EBoard.Models;
using EBoard.ViewModels;
using EBoardSDK.Models.DataSets;
using System.Collections.ObjectModel;
using EBoard.IOProcesses.DataSets;
using EBoardSDK.Plugins;

namespace EBoard.Utilities.Factories;

public static class EBoardFactory
{

    public static EBoardViewModel GetEBoardViewModelByEBoardDataSet(EboardScreen eboardScreen, MainViewModel mainViewModel)
    {
        var eBoardViewModel = new EBoardViewModel(mainViewModel);

        var eboardDataSet = ScreenToDataSetMapper(eboardScreen, eBoardViewModel);

        eBoardViewModel.ApplyData(eboardDataSet);

        return eBoardViewModel;
    }

    private static EboardDataSet ScreenToDataSetMapper(EboardScreen eboardScreen, EBoardViewModel eBoardViewModel)
    {
        ObservableCollection<ElementViewModel> elementViewModels = [];

        eboardScreen.Elements.Select(x => x).ToList().ForEach(
        async element =>
        {
            var evm = new ElementViewModel();
            var eds = new ElementDataSet()
            {
                EID = element.EID,
                //ID = escreen.Elements.IndexOf(element),
                PluginHeader = element.PluginHeader,
                PluginType = element.PluginName,

                BorderDataSet = element.BorderDataSet,
                BrushDataSet = element.BrushDataSet,
                PlacementDataSet = element.PlacementDataSet,
            };

            Type? type_PluginViewModel = Type.GetType(element.AssemblyName);

            if (type_PluginViewModel != null)
            {
                IEBoardElement? externalPlugin = Activator.CreateInstance(type_PluginViewModel) as IEBoardElement;

                if (externalPlugin is not null)
                {
                    eds.Plugin = externalPlugin;

                    // datasets umbauen
                    externalPlugin.PluginHeader = element.PluginHeader;

                    var plugindatapath = eboardScreen.ContentDataFilePaths.Where(x => x.FullName.EndsWith($"{eds.EID}.data")).FirstOrDefault();

                    if (plugindatapath != null)
                    {
                        await eds.Plugin.Load(plugindatapath.FullName);
                    }

                    App.Current.Resources.MergedDictionaries.Add(externalPlugin.ResourceDictionary);
                } 
            }

            evm.ApplyData(eBoardViewModel, eds);

            elementViewModels.Add(evm);
        });

        return new EboardDataSet()
        {
            EBID = eboardScreen.EBID,            
            EBoardName = eboardScreen.EBoardName,
            EBoardDepth = eboardScreen.EBoardDepth,
            BorderDataSet = eboardScreen.BorderDataSet,
            BrushDataSet = eboardScreen.BrushDataSet,

            Elements = elementViewModels
        };
    }

    public static EboardScreen GetNewEboardScreen(string name, int depth, double width, double height)
    {
        return new EboardScreen
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardName = name,
            EBoardDepth = depth,
            BorderDataSet = new BorderDataSet(new BorderManagement() { Width = width, Height = height }),
            BrushDataSet = new BrushDataSet(new BrushManagement())
        };
    }

    public static EboardDataSet GetNewEBoardDataSet()
    {
        EboardDataSet eboardDataSet = new EboardDataSet
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardName = "new eboard",
            EBoardDepth = 100,
            BorderDataSet = new BorderDataSet(new BorderManagement() { Width = 640, Height = 320 }),
            BrushDataSet = new BrushDataSet(new BrushManagement())
        };

        return eboardDataSet;
    }

}
