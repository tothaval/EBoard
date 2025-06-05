// <copyright file="EBoardFactory.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoard.Utilities.Factories;

using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;
using Serilog;
using System.Collections.ObjectModel;
using System.IO;

public static class EBoardFactory
{
    public static EBoardViewModel GetEBoardViewModelByEBoardDataSet(EboardScreen eboardScreen, MainViewModel mainViewModel)
    {
        var eBoardViewModel = new EBoardViewModel(mainViewModel);

        var eboardDataSet = ScreenToDataSetMapper(eboardScreen, eBoardViewModel);

        eBoardViewModel.ApplyData(eboardDataSet);

        return eBoardViewModel;
    }

    public static EboardScreen GetNewEboardScreen(string name, int depth, double width, double height)
    {
        return new EboardScreen
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardName = name,
            EBoardDepth = depth,
            BorderDataSet = new BorderDataSet(new BorderManagement() { Width = width, Height = height }),
            BrushDataSet = new BrushDataSet(new BrushManagement()),
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
            BrushDataSet = new BrushDataSet(new BrushManagement()),
        };

        return eboardDataSet;
    }

    private static EboardDataSet ScreenToDataSetMapper(EboardScreen eboardScreen, EBoardViewModel eBoardViewModel)
    {
        ObservableCollection<ElementViewModel> elementViewModels = [];

        eboardScreen.Elements.Select(x => x).ToList().ForEach(
        async element =>
        {
            var eds = new ElementDataSet()
            {
                EID = element.EID,
                PluginHeader = element.PluginHeader,
                PluginType = element.PluginName,

                BorderDataSet = element.BorderDataSet,
                BrushDataSet = element.BrushDataSet,
                PlacementDataSet = element.PlacementDataSet,
            };

            Type? type_PluginViewModel = Type.GetType(element.AssemblyName);

            if (type_PluginViewModel != null)
            {
                EBoardElementPluginBaseViewModel? externalPlugin = Activator.CreateInstance(type_PluginViewModel) as EBoardElementPluginBaseViewModel;

                if (externalPlugin != null)
                {
                    eds.Plugin = externalPlugin;

                    externalPlugin.PluginHeader = element.PluginHeader;

                    var plugindatapath = eboardScreen.ContentDataFilePaths.Where(x => x.FullName.EndsWith($"{eds.EID}.data")).FirstOrDefault();

                    if (plugindatapath != null)
                    {
                        await eds.Plugin.Load(plugindatapath.FullName);
                    }

                    try
                    {
                        App.Current.Resources.MergedDictionaries.Add(externalPlugin.ResourceDictionary);
                    }
                    catch (IOException ioex)
                    {
                        var ioexAdditionalMessage = string.Join(
                            $"\n__{type_PluginViewModel}\t",
                            $"plugin load error: {externalPlugin.PluginName}",
                            "ResourceDictionary path or file is corrupt");

                        Log.Error(ioex, ioexAdditionalMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "unhandled exception");
                        throw;
                    }
                }
            }

            var elementViewModel = new ElementViewModel();

            elementViewModel.ApplyData(eBoardViewModel, eds);

            elementViewModel.Redraw();

            elementViewModels.Add(elementViewModel);
        });

        return new EboardDataSet()
        {
            EBID = eboardScreen.EBID,
            EBoardName = eboardScreen.EBoardName,
            EBoardDepth = eboardScreen.EBoardDepth,
            BorderDataSet = eboardScreen.BorderDataSet,
            BrushDataSet = eboardScreen.BrushDataSet,

            Elements = elementViewModels,
        };
    }
}

// EOF