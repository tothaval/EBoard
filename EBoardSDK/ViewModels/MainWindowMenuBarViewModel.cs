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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.DataSets;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.IO;
using System.Windows;
using System.Xml.Linq;

/// <summary>
/// TODO:
/// im View die Header strings mit Werten hier im Viewmodel ersetzen, die Werte entweder aus den Ressourcen ziehen
/// oder aus einer Sprachdatei, wobei vielleicht sinnvoller den diesbezüglich vorhandenen Mechanismus zu nutzen, scheint okay.
/// 
/// ganze Pluginsortierung schöner machen oder vereinheitlichen. zur Not eine Datenklasse basteln dafür.
/// ich brauche eine zentrale Liste für eine Combobox um zum Beispiel in Elementen wie Areas alle vorhandenen Plugins instanzieren zu können.
/// </summary>
public partial class MainWindowMenuBarViewModel : ObservableObject
{
    private readonly BrushManagement brushManagement;

    [ObservableProperty]
    private bool eBoardBrowserSwitch;

    [ObservableProperty]
    private bool eBoardSettingsSwitch;

    private MainViewModel mainViewModel;

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryAddons = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryElements = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryShapes = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryAreas = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> pluginCategoryTools = [];

    [ObservableProperty]
    private IList<EBoardElementPluginBaseViewModel> foundPlugins = [];

    private BrushManagement screenBrushManagement;

    public MainWindowMenuBarViewModel(MainViewModel mainViewModel, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.mainViewModel = mainViewModel;
        this.brushManagement = mainViewModel.EBoardBrowserViewModel.BrushManagement;
        this.screenBrushManagement = this.mainViewModel.BrushManagement;

        this.mainViewModel.EBoardBrowserViewModel.PropertyChanged += this.EBoardBrowserViewModel_PropertyChanged;
        this.brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

        this.InstallEboardPlugins(SDKPluginManager.SDKPlugins);

        this.InstallEboardPlugins(eboardConfig.ElementPlugins);
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    private void EBoardBrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            if (this.ScreenBrushManagement != null)
            {
                this.ScreenBrushManagement.PropertyChangedEvent -= this.ScreenBrushManagement_PropertyChangedEvent;
            }

            this.screenBrushManagement = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.BrushManagement;

            if (this.ScreenBrushManagement != null)
            {
                this.ScreenBrushManagement.PropertyChangedEvent += this.ScreenBrushManagement_PropertyChangedEvent;
            }

            this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
        }
    }

    partial void OnEBoardBrowserSwitchChanged(bool value)
    {
        if (this.EBoardSettingsSwitch)
        {
            this.EBoardSettingsSwitch = false;
        }
    }

    partial void OnEBoardSettingsSwitchChanged(bool value)
    {
        if (this.EBoardBrowserSwitch)
        {
            this.EBoardBrowserSwitch = false;
        }
    }

    private void ScreenBrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
    }

    public BrushManagement BrushManagement => this.brushManagement;

    public BrushManagement ScreenBrushManagement => this.screenBrushManagement;

    public MainViewModel MainViewModel => this.mainViewModel;

    public EBoardBrowserViewModel EBoardBrowserViewModel => this.MainViewModel.EBoardBrowserViewModel;

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

                try
                {
                    if (!Application.Current.Resources.MergedDictionaries.Contains(sdkplugin.ResourceDictionary))
                    {
                        Application.Current.Resources.MergedDictionaries.Add(sdkplugin.ResourceDictionary);
                    }
                }
                catch (IOException ioex)
                {
                    var ioexAdditionalMessage = string.Join(
                        $"\n__{sdkplugin.ElementPluginAssembly}\t",
                        $"plugin load error: {sdkplugin.PluginName}",
                        "ResourceDictionary path or file is corrupt");
                    Log.Error(ioex, ioexAdditionalMessage);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "unhandled exception");
                    throw;
                }

                var category = sdkplugin.PluginCategory;

                switch (category)
                {
                    case EBoardSDK.Enums.PluginCategories.Addon:
                        this.PluginCategoryAddons.Add(sdkplugin);

                        this.OnPropertyChanged(nameof(this.PluginCategoryAddons));
                        break;
                    case EBoardSDK.Enums.PluginCategories.Element:
                        this.PluginCategoryElements.Add(sdkplugin);
                        this.OnPropertyChanged(nameof(this.PluginCategoryElements));
                        break;
                    case EBoardSDK.Enums.PluginCategories.Shape:
                        this.PluginCategoryShapes.Add(sdkplugin);
                        this.OnPropertyChanged(nameof(this.PluginCategoryShapes));
                        break;
                    case EBoardSDK.Enums.PluginCategories.Area:
                        this.PluginCategoryAreas.Add(sdkplugin);
                        this.OnPropertyChanged(nameof(this.PluginCategoryAreas));
                        break;
                    case EBoardSDK.Enums.PluginCategories.Tool:
                        this.PluginCategoryTools.Add(sdkplugin);
                        this.OnPropertyChanged(nameof(this.PluginCategoryTools));
                        break;
                    case EBoardSDK.Enums.PluginCategories.Unkown:
                        break;
                    default:
                        break;
                }

                this.FoundPlugins.Add(sdkplugin);
                this.OnPropertyChanged(nameof(this.FoundPlugins));
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

                this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(element);
            }
        }
    }
}

// EOF