// <copyright file="MainWindowMenuBarViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainWindowMenuBarViewModel
 *
 *  view model for MainWindowMenuBarView, which has prototype element instantiation
 *  button and a prototype shape menu and a button to switch on or off the EBoardBrowserView
 */
namespace EBoard.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.IOProcesses.DataSets;
using EBoard.Utilities.Factories;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins;
using Serilog;
using System.IO;
using System.Windows.Input;

public partial class MainWindowMenuBarViewModel : ObservableObject
{
    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private bool eBoardBrowserSwitch;

    private MainViewModel mainViewModel;

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryAddons = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryElements = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryShapes = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryTools = [];

#if RELEASE

    [ObservableProperty]
    private bool isDebug = false;

#endif

#if DEBUG

    [ObservableProperty]
    private bool isDebug = true;

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginProjects;
#endif

    public MainWindowMenuBarViewModel(MainViewModel mainViewModel, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.title = "EBoard";

        this.mainViewModel = mainViewModel;

        this.InstallEboardPlugins(SDKPluginManager.SDKPlugins);

        this.InstallEboardPlugins(eboardConfig.ElementPlugins);

#if DEBUG
        this.PluginProjects = eboardConfig.CurrentDevelopmentPlugins;
#endif
    }

    public MainViewModel MainViewModel => this.mainViewModel;

    public EBoardBrowserViewModel EBoardBrowserViewModel => this.MainViewModel.EBoardBrowserViewModel;

    public ICommand InvokeElementCommand { get; }

    private void InstallEboardPlugins(IList<EBoardElementPluginBaseViewModel> elements)
    {
        elements.ToList().ForEach(
            sdkplugin =>
            {
                try
                {
                    _ = sdkplugin.Initialize();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "plugin initialization error");
                }

                var category = sdkplugin.PluginCategory;

                switch (category)
                {
                    case EBoardSDK.Enums.PluginCategories.Addon:
                        this.PluginCategoryAddons.Add(sdkplugin);
                        break;
                    case EBoardSDK.Enums.PluginCategories.Element:
                        this.PluginCategoryElements.Add(sdkplugin);
                        break;
                    case EBoardSDK.Enums.PluginCategories.Shape:
                        this.PluginCategoryShapes.Add(sdkplugin);
                        break;
                    case EBoardSDK.Enums.PluginCategories.Tool:
                        this.PluginCategoryTools.Add(sdkplugin);
                        break;
                    case EBoardSDK.Enums.PluginCategories.Unkown:
                        break;
                    default:
                        break;
                }
            });
    }

    [RelayCommand]
    private void ClearElements()
    {
        this.mainViewModel?.EBoardBrowserViewModel?.DeleteAllElements();
    }

    [RelayCommand]
    private void InvokePlugin(object s)
    {
        if (s is Type)
        {
            var selectedEboard = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard;

            var p = s as Type;
            var plugin = Activator.CreateInstance(p) as IPlugin;

            var pt = plugin?.GetType();

            var interfaces = plugin?.GetType().GetInterfaces();

            if (selectedEboard != null &&
                plugin != null &&
                interfaces != null &&
                interfaces.Any(x => x.Name.Equals(nameof(IPlugin))))
            {
                IElementDataSet newElementDataSet = new ElementDataSet();

                newElementDataSet = ElementDataSetFactory.GetElementDataSet(
                    plugin: plugin);

                var screenpolicies = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.InstantiationPolicies;

                if (screenpolicies == null || screenpolicies.Contains(EBoardSDK.Enums.ElementInstantiationPolicy.ValueNotSet))
                {
                    return;
                }

                if (newElementDataSet.Plugin.ElementScreenIntegrationConstraints == null)
                {
                    return;
                }

                if (newElementDataSet.Plugin.ElementScreenIntegrationConstraints.InstantiationPolicy == EBoardSDK.Enums.ElementInstantiationPolicy.OnePerScreen)
                {
                    bool exists = false;

                    selectedEboard.Elements.ToList().ForEach(element =>
                    {
                        if (element.Plugin.ElementPluginViewModel.Equals(newElementDataSet.Plugin.ElementPluginViewModel))
                        {
                            exists = true;
                        }
                    });

                    if (exists)
                    {
                        return;
                    }
                }

                ElementViewModel element = new ElementViewModel(
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard,
                    newElementDataSet);

                try
                {
                    if (!App.Current.Resources.MergedDictionaries.Contains(plugin.ResourceDictionary))
                    {
                        App.Current.Resources.MergedDictionaries.Add(plugin.ResourceDictionary);
                    }
                }
                catch (IOException ioex)
                {
                    var ioexAdditionalMessage = string.Join(
                        $"\n__{p}\t",
                        $"plugin load error: {element.Plugin.PluginName}",
                        "ResourceDictionary path or file is corrupt");
                    Log.Error(ioex, ioexAdditionalMessage);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "unhandled exception");
                    throw;
                }

                this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(element);
            }
        }
    }
}

// EOF