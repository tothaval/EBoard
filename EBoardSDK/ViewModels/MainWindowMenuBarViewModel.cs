// <copyright file="MainWindowMenuBarViewModel.cs" company=".">
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
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Interfaces.FluidUIText;
using EBoardSDK.Plugins;
using Serilog;
using System.IO;
using System.Windows;
using System.Windows.Media;

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
    private readonly IFluidUIDesignModel brushManagement;
    private readonly IFluidUIFontModel fontManagement;

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

    private IFluidUIDesignModel screenBrushManagement;
    private IFluidUIFontModel screenFontManagement;

    public MainWindowMenuBarViewModel(MainViewModel mainViewModel, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.mainViewModel = mainViewModel;
        this.brushManagement = mainViewModel.EBoardBrowserViewModel.FluidUI.Design;
        this.screenBrushManagement = this.mainViewModel.FluidUI.Design;

        this.fontManagement = mainViewModel.EBoardBrowserViewModel.FluidUI.Font;
        this.screenFontManagement = this.mainViewModel.FluidUI.Font;

        this.mainViewModel.EBoardBrowserViewModel.PropertyChanged += this.EBoardBrowserViewModel_PropertyChanged;
        //this.brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

        this.InstallEboardPlugins(SDKPluginManager.SDKPlugins);

        this.InstallEboardPlugins(eboardConfig.ElementPlugins);
    }

    public IFluidUIDesignModel BrushManagement => this.brushManagement;

    public IFluidUIDesignModel ScreenBrushManagement => this.screenBrushManagement;

    public IFluidUIFontModel FontManagement => this.fontManagement;

    public IFluidUIFontModel ScreenFontManagement => this.screenFontManagement;

    public MainViewModel MainViewModel => this.mainViewModel;

    public EBoardBrowserViewModel EBoardBrowserViewModel => this.MainViewModel.EBoardBrowserViewModel;

    //private void BrushManagement_PropertyChangedEvent()
    //{
    //    this.OnPropertyChanged(nameof(this.BrushManagement));
    //    this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
    //}

    private void EBoardBrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            if (this.ScreenBrushManagement != null)
            {
                this.ScreenBrushManagement.PropertyChangedEvent -= this.ScreenBrushManagement_PropertyChangedEvent;
            }

            if (this.ScreenFontManagement != null)
            {
                this.ScreenFontManagement.PropertyChangedEvent -= this.ScreenFontManagement_PropertyChangedEvent;
            }

            this.screenBrushManagement = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.FluidUI.Design;
            this.screenFontManagement = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.FluidUI.Font;

            if (this.ScreenBrushManagement != null)
            {
                this.ScreenBrushManagement.PropertyChangedEvent += this.ScreenBrushManagement_PropertyChangedEvent;
            }

            if (this.ScreenFontManagement != null)
            {
                this.ScreenFontManagement.PropertyChangedEvent += this.ScreenFontManagement_PropertyChangedEvent; ;
            }

            this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
            this.OnPropertyChanged(nameof(this.ScreenFontManagement));
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

    private void ScreenFontManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.ScreenFontManagement));
    }

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

            if (selectedEboard != null && p != null)
            {
                var elementViewModel = new ElementViewModel(selectedEboard);

                if (Activator.CreateInstance(p) is IPlugin plugin)
                {
                    plugin.SetEBoardAndElementViewModel(selectedEboard, elementViewModel);

                    var interfaces = plugin?.GetType().GetInterfaces();

                    if (interfaces != null &&
                        interfaces.Any(x => x.Name.Equals(nameof(IPlugin))))
                    {
                        var screenpolicies = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.InstantiationPolicies;

                        if (screenpolicies == null || screenpolicies.Contains(EBoardSDK.Enums.ElementInstantiationPolicy.ValueNotSet))
                        {
                            return;
                        }

                        if (plugin.ElementScreenIntegrationConstraints == null)
                        {
                            return;
                        }

                        if (plugin.ElementScreenIntegrationConstraints.InstantiationPolicy == EBoardSDK.Enums.ElementInstantiationPolicy.OnePerScreen)
                        {
                            bool exists = false;

                            selectedEboard.Elements.ToList().ForEach(element =>
                            {
                                if (element.Plugin.ElementPluginViewModel.Equals(plugin.ElementPluginViewModel))
                                {
                                    exists = true;
                                }
                            });

                            if (exists)
                            {
                                return;
                            }
                        }

                        if (plugin.PluginCategory == Enums.PluginCategories.Shape)
                        {
                            elementViewModel.FluidUI.Design.Background = new SolidColorBrush(Colors.Transparent);
                            elementViewModel.FluidUI.Design.Foreground = new SolidColorBrush(Colors.Transparent);
                            elementViewModel.FluidUI.Design.Border = new SolidColorBrush(Colors.Transparent);

                            elementViewModel.FluidUI.Size.Margin = new Thickness(0);
                            elementViewModel.FluidUI.Size.Padding = new Thickness(0);
                            elementViewModel.FluidUI.Size.BorderThickness = new Thickness(0);
                            elementViewModel.FluidUI.Size.CornerRadius = new CornerRadius(0);

                            elementViewModel.Redraw();
                        }

                        elementViewModel.Plugin = plugin;

                        elementViewModel.Plugin.SetEBoardAndElementViewModel(selectedEboard, elementViewModel);

                        elementViewModel.Plugin.RefreshInitialization();

                        this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.AddElement(elementViewModel);
                    }
                }
            }
        }
    }
}

// EOF