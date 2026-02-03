// <copyright file="SDKPluginManager.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Plugins;

using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Plugins.Addons.Dice_n_Random;
using EBoardSDK.Plugins.Addons.SoundMix;

using EBoardSDK.Plugins.Areas.FileLinkArea;
using EBoardSDK.Plugins.Areas.PluginArea;
using EBoardSDK.Plugins.Areas.ShapeArea;
using EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;

using EBoardSDK.Plugins.Eboard.About;
using EBoardSDK.Plugins.Eboard.Coordinates;
using EBoardSDK.Plugins.Eboard.EboardBrowser;
using EBoardSDK.Plugins.Eboard.LogOutBar;
using EBoardSDK.Plugins.Eboard.MainWindow;
using EBoardSDK.Plugins.Eboard.Manual;
using EBoardSDK.Plugins.Eboard.MenuBar;
using EBoardSDK.Plugins.Eboard.PluginManager;
using EBoardSDK.Plugins.Eboard.ScreenChanger;
using EBoardSDK.Plugins.Eboard.ScreenControl;
using EBoardSDK.Plugins.Eboard.ScreenDataBoard;
using EBoardSDK.Plugins.Eboard.Summoner;

using EBoardSDK.Plugins.Elements.BasicAV;
using EBoardSDK.Plugins.Elements.EmptyLinear;
using EBoardSDK.Plugins.Elements.EmptyRadial;
using EBoardSDK.Plugins.Elements.Gold;
using EBoardSDK.Plugins.Elements.Image;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol;
using EBoardSDK.Plugins.Elements.StandardText;

using EBoardSDK.Plugins.Shapes.Ellipse;
using EBoardSDK.Plugins.Shapes.Line;
using EBoardSDK.Plugins.Shapes.Path;
using EBoardSDK.Plugins.Shapes.Polygon;
using EBoardSDK.Plugins.Shapes.Rectangle;
using EBoardSDK.Plugins.Shapes.TextShape;

using EBoardSDK.Plugins.Tools.PolygonMaker;
using EBoardSDK.Plugins.Tools.Uptime;

using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml.Linq;

public class SDKPluginManager
{
    private SDKDataManager? sDKDataManager = null;

    private List<PluginRepresentationItem> foundPlugins = new();

    private List<PluginRepresentationItem> foundAddons = new();
    private List<PluginRepresentationItem> foundElements = new();
    private List<PluginRepresentationItem> foundShapes = new();
    private List<PluginRepresentationItem> foundAreas = new();
    private List<PluginRepresentationItem> foundTools = new();

    private List<PluginRepresentationItem> foundEboard = new();
    private List<PluginRepresentationItem> foundSystem = new();

    private List<PluginRepresentationItem> foundUnknown = new();

    /// <summary>
    /// a list of all plugins this eboardsdk contains.
    /// </summary>
    private List<PluginBaseViewModel> sDKPlugins =
        [

        // Addons
        new Dice_n_RandomViewModel(),
        new SoundMixMainViewModel(), // saves and loads ecf data

        // Elements
        new EmptyLinearViewModel(),
        new EmptyRadialViewModel(),
        new GoldViewModel(),

        new ImageViewModel(), // saves and loads ecf data
        new LinkViewModel(), // saves and loads ecf data
        new ProtocolViewModel(), // saves and loads ecf data
        new StandardTextViewModel(), // saves and loads ecf data
        new BasicAVMainViewModel(), // saves and loads ecf data

        // Shapes
        new EllipseViewModel(),
        new LineViewModel(), // saves and loads ecf data
        new PathViewModel(), // saves and loads ecf data
        new PolygonViewModel(), // saves and loads ecf data
        new RectangleViewModel(),
        new TextShapeViewModel(), // saves and loads ecf data

        // Areas
        new FileLinkAreaViewModel(), // saves and loads ecf data
        new PluginAreaViewModel(), // saves and loads ecf data
        new ShapeAreaViewModel(), // saves and loads ecf data
        new TwoXThreeImageAreaViewModel(), // saves and loads ecf data

        // Tools
        new PolygonMakerViewModel(), // saves and loads ecf data
        new UptimeViewModel(),

        // System

        // Eboard
        new AboutViewModel(),
        new CoordinatesViewModel(),
        new EboardBrowserViewModel(),
        new LogOutBarViewModel(), // saves and loads ecf data
        new ManualViewModel(),
        new MainWindowViewModel(),
        new MenuBarViewModel(), // saves and loads ecf data
        new PluginManagerViewModel(),
        new ScreenChangerViewModel(),
        new ScreenControlViewModel(),
        new ScreenDataBoardViewModel(),
        new SummonerViewModel(), // saves and loads ecf data
        ];

    /// <summary>
    /// Initializes a new instance of the <see cref="SDKPluginManager"/> class.
    /// </summary>
    /// <param name="sDKDataManager"></param>
    public SDKPluginManager(SDKDataManager? sDKDataManager = null)
    {
        this.sDKDataManager = sDKDataManager;

        this.Initialize();
    }

    public List<PluginRepresentationItem> FoundPlugins => this.foundPlugins;

    public List<PluginRepresentationItem> FoundAddons => this.foundAddons;

    public List<PluginRepresentationItem> FoundElements => this.foundElements;

    public List<PluginRepresentationItem> FoundShapes => this.foundShapes;

    public List<PluginRepresentationItem> FoundAreas => this.foundAreas;

    public List<PluginRepresentationItem> FoundTools => this.foundTools;

    public List<PluginRepresentationItem> FoundEboard => this.foundEboard;

    public List<PluginRepresentationItem> FoundSystem => this.foundSystem;

    public List<PluginRepresentationItem> FoundUnknown => this.foundUnknown;

    internal void DuplicatePlugin(ElementViewModel elementViewModel)
    {
        if (elementViewModel != null && elementViewModel.Plugin != null)
        {
            elementViewModel.PrepareCopy();

            var wasSelected = false;

            if (elementViewModel.IsSelected)
            {
                elementViewModel.SelectElement();
                wasSelected = true;
            }

            for (int i = 0; i < elementViewModel.DuplicationCount; i++)
            {
                var copy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(elementViewModel.FluidUI);

                var element = ElementFactory.GetElementViewModel(elementViewModel.ScreenViewModel);

                var plugin = this.InvokePluginByName(elementViewModel.Plugin.Name).Result;

                if (plugin != null)
                {
                    plugin.SetElementViewModel(element);

                    if (plugin.GetType().BaseType == typeof(EboardPluginBaseViewModel))
                    {
                        ((EboardPluginBaseViewModel)plugin).SetMainViewModel(elementViewModel.ScreenViewModel.MainViewModel);
                    }

                    plugin.RefreshInitialization();

                    plugin.InsertModel(elementViewModel.Plugin.PluginModel);

                    element.SetFluidUI(copy);
                    element.Plugin = plugin;

                    this.InvokeElementViewModelOnEboard(element);
                }
            }

            if (wasSelected)
            {
                elementViewModel.SelectElement();
            }
        }
    }

    internal void InvokeElementViewModelOnEboard(ElementViewModel element)
    {
        if (element != null && element.Plugin != null)
        {
            var mainViewModel = element.ScreenViewModel.MainViewModel;
            var interfaces = element.Plugin.GetType().GetInterfaces();

            if (interfaces != null &&
                interfaces.Any(x => x.Name.Equals(nameof(IPlugin))))
            {
                var screenpolicies = element.ScreenViewModel.InstantiationPolicies;

                if (screenpolicies == null || screenpolicies.Contains(EBoardSDK.Enums.InstantiationPolicy.ValueNotSet))
                {
                    mainViewModel.WriteToMessageStrip($"denied, screen settings forbid action, instantiation policy: ValueNotSet");
                    return;
                }

                if (element.Plugin.ScreenInstantiationConstraints == null)
                {
                    mainViewModel.WriteToMessageStrip($"denied, {element.Plugin.Name} plugin is missing ScreenInstantiationConstraints");
                    return;
                }

                if (element.Plugin.ScreenInstantiationConstraints.InstantiationPolicy == EBoardSDK.Enums.InstantiationPolicy.OnePerScreen)
                {
                    bool exists = false;

                    element.ScreenViewModel.Elements.ToList().ForEach(foundelement =>
                    {
                        if (foundelement.Plugin != null && element.Plugin.PluginViewModelType.Equals(foundelement.Plugin.PluginViewModelType))
                        {
                            exists = true;
                        }
                    });

                    if (exists)
                    {
                        mainViewModel.WriteToMessageStrip($"{element.Plugin.Name} plugin settings forbid action, instantiation policy: {element.Plugin.ScreenInstantiationConstraints.InstantiationPolicy}, plugin detected on screen {new FluidUIDataBlockManager(element.ScreenViewModel).GetTitle()}");

                        return;
                    }
                }

                element.UpdateSize();
                element.UpdateStand();

                element.ScreenViewModel.AddElement(element);
            }
        }
    }

    internal async Task<IPlugin?> InvokePluginByName(string pluginName)
    {
        IPlugin? plugin = null;

        if (string.IsNullOrWhiteSpace(pluginName))
        {
            return plugin;
        }

        if (this.foundPlugins != null && this.foundPlugins.Count > 0)
        {
            var pluginItem = this.foundPlugins.Where(x => x.PluginName != null && x.PluginName.Equals(pluginName)).FirstOrDefault();

            if (pluginItem != null && pluginItem.PluginMainViewModelType != null)
            {
                plugin = Activator.CreateInstance(pluginItem.PluginMainViewModelType) as IPlugin;

                if (plugin == null)
                {
                    return plugin;
                }

                try
                {
                    _ = await plugin.Initialize();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "plugin initialization error");
                }

                try
                {
                    if (!Application.Current.Resources.MergedDictionaries.Contains(plugin.ResourceDictionary))
                    {
                        Application.Current.Resources.MergedDictionaries.Add(plugin.ResourceDictionary);
                    }
                }
                catch (IOException ioex)
                {
                    var ioexAdditionalMessage = string.Join(
                        $"\n__{plugin.PluginAssembly}\t",
                        $"plugin load error: {plugin.Name}",
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

        return plugin;
    }

    internal void InvokePluginNTimes<T>(T plugin, int count, ScreenViewModel eBoardViewModel)
         where T : IPlugin?
    {
        for (int i = 0; i < count; i++)
        {
            this.InvokePluginOnEboard(plugin, eBoardViewModel);
        }
    }

    internal void InvokePluginOnEboard<T>(T plugin, ScreenViewModel eBoardViewModel, Point? desiredPosition = null)
         where T : IPlugin?
    {
        if (eBoardViewModel != null && plugin != null)
        {
            var mainViewModel = eBoardViewModel.MainViewModel;
            var elementViewModel = ElementFactory.GetElementViewModel(eBoardViewModel);

            var interfaces = plugin.GetType().GetInterfaces();

            if (interfaces != null &&
                interfaces.Any(x => x.Name.Equals(nameof(IPlugin))))
            {
                var screenpolicies = eBoardViewModel.InstantiationPolicies;

                if (screenpolicies == null || screenpolicies.Contains(EBoardSDK.Enums.InstantiationPolicy.ValueNotSet))
                {
                    mainViewModel.WriteToMessageStrip($"denied, screen settings forbid action, instantiation policy: ValueNotSet");
                    return;
                }

                if (plugin.ScreenInstantiationConstraints == null)
                {
                    mainViewModel.WriteToMessageStrip($"denied, {plugin.Name} plugin is missing ScreenInstantiationConstraints");
                    return;
                }

                if (plugin.ScreenInstantiationConstraints.InstantiationPolicy == EBoardSDK.Enums.InstantiationPolicy.OnePerScreen)
                {
                    bool exists = false;

                    eBoardViewModel.Elements.ToList().ForEach(element =>
                    {
                        if (element.Plugin != null && element.Plugin.PluginViewModelType != null && plugin.PluginViewModelType != null)
                        {
                            if (element.Plugin.PluginViewModelType.Equals(plugin.PluginViewModelType))
                            {
                                exists = true;
                            }
                        }
                    });

                    if (exists)
                    {
                        mainViewModel.WriteToMessageStrip($"{plugin.Name} plugin settings forbid action, instantiation policy: {plugin.ScreenInstantiationConstraints.InstantiationPolicy}, plugin instance detected on screen {new FluidUIDataBlockManager(eBoardViewModel).GetTitle()}");

                        return;
                    }
                }

                elementViewModel.Plugin = plugin;

                if (elementViewModel.Plugin.GetType().BaseType == typeof(EboardPluginBaseViewModel))
                {
                    ((EboardPluginBaseViewModel)elementViewModel.Plugin).SetMainViewModel(elementViewModel.ScreenViewModel.MainViewModel);
                }

                elementViewModel.Plugin.RefreshInitialization();

                elementViewModel.Plugin.SetElementViewModel(elementViewModel);

                if (desiredPosition != null)
                {
                    var manager = new FluidUIStandManager(elementViewModel);
                    manager.SetPosition((Point)desiredPosition);
                }

                eBoardViewModel.AddElement(elementViewModel);
            }
        }
    }

    internal void ReInitializeManager()
    {
        this.foundPlugins.Clear();
        this.foundAddons.Clear();
        this.foundElements.Clear();
        this.foundShapes.Clear();
        this.foundAreas.Clear();
        this.foundTools.Clear();
        this.foundEboard.Clear();
        this.foundSystem.Clear();
        this.foundUnknown.Clear();

        this.Initialize();
    }

    internal void UpdateManager()
    {

        this.Initialize();
    }

    internal async void InstallPlugin(PluginRepresentationItem plugin)
    {
        if (plugin == null)
        {
            return;
        }

        await this.InstallEboardPluginAsync(plugin);

        this.FillPluginCategoryLists();
    }

    internal void UninstallPlugin(PluginRepresentationItem plugin)
    {
        // TODO: alle plugins in elementen suchen und entfernen
        if (plugin == null)
        {
            return;
        }

        var found = this.foundPlugins.Where(x => x != null && x.Equals(plugin)).FirstOrDefault();

        if (found == null)
        {
            return;
        }

        this.foundPlugins.Remove(found);

        switch (plugin.PluginCategory)
        {
            case Enums.PluginCategories.Addon:
                this.foundAddons.Remove(found);
                break;
            case Enums.PluginCategories.Element:
                this.foundElements.Remove(found);
                break;
            case Enums.PluginCategories.Shape:
                this.foundShapes.Remove(found);
                break;
            case Enums.PluginCategories.Area:
                this.FoundAreas.Remove(found);
                break;
            case Enums.PluginCategories.Tool:
                this.foundTools.Remove(found);
                break;
            case Enums.PluginCategories.Eboard:
                this.foundEboard.Remove(found);
                break;
            case Enums.PluginCategories.System:
                this.foundSystem.Remove(found);
                break;
            case Enums.PluginCategories.Unkown:
                this.foundUnknown.Remove(found);
                break;
            default:
                break;
        }

        this.FillPluginCategoryLists();
    }

    private void FillPluginCategoryLists()
    {
        this.foundAddons.Clear();
        this.foundElements.Clear();
        this.foundShapes.Clear();
        this.foundAreas.Clear();
        this.foundTools.Clear();
        this.foundUnknown.Clear();

        foreach (var plugin in this.foundPlugins)
        {
            var category = plugin.PluginCategory;

            switch (category)
            {
                case EBoardSDK.Enums.PluginCategories.Addon:
                    this.foundAddons.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Element:
                    this.foundElements.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Shape:
                    this.foundShapes.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Area:
                    this.foundAreas.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Tool:
                    this.foundTools.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Eboard:
                    this.foundEboard.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.System:
                    this.foundSystem.Add(plugin);
                    break;
                case EBoardSDK.Enums.PluginCategories.Unkown:
                    this.foundUnknown.Add(plugin);
                    break;
                default:
                    break;
            }
        }
    }

    private async Task<List<PluginRepresentationItem>> GetInstalledPluginsAsync()
    {
        List<PluginRepresentationItem> plugins = [];

        try
        {
            if (this.sDKDataManager == null)
            {
                this.sDKDataManager = new SDKDataManager();
            }

            plugins = await this.sDKDataManager.LoadPluginsFromInstalledPluginsDirectoryAsync();
        }
        catch (Exception)
        {
            throw;
        }

        return plugins;
    }

    private async void Initialize()
    {
        this.foundPlugins.Clear();

        foreach (var x in this.sDKPlugins)
        {
            this.foundPlugins.Add(new PluginRepresentationItem()
            {
                PluginCategory = x.Category,
                PluginHeader = x.Header,
                PluginLogo = x.Logo,
                PluginMainViewModelType = x.PluginViewModelType,
                PluginName = x.Name,
                ScreenConstraints = x.ScreenInstantiationConstraints,
            });
        }

        await this.InstallEboardPluginsAsync();

        this.FillPluginCategoryLists();
    }

    private async Task InstallEboardPluginsAsync()
    {
        var externalPlugins = await this.GetInstalledPluginsAsync();

        externalPlugins?.ForEach(
            async sdkplugin =>
            {
                await this.InstallEboardPluginAsync(sdkplugin);
            });
    }

    private async Task InstallEboardPluginAsync(PluginRepresentationItem plugin)
    {
        if (!this.foundPlugins.Contains(plugin))
        {
            this.foundPlugins.Add(plugin);
        }
    }
}

// EOF