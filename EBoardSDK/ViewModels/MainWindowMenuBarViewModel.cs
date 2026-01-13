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
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System.Windows.Threading;

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
    private SDKPluginManager sDKPluginManager;

    private readonly IFluidUIDesignModel brushManagement;
    private readonly IFluidUIFontModel fontManagement;

    private IFluidUIDesignModel screenBrushManagement;
    private IFluidUIFontModel screenFontManagement;

    private MainViewModel mainViewModel;

    [ObservableProperty]
    private bool eBoardBrowserSwitch = false;

    [ObservableProperty]
    private bool screenControlSwitch = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindowMenuBarViewModel"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    /// <param name="eboardConfig"></param>
    public MainWindowMenuBarViewModel(MainViewModel mainViewModel, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.mainViewModel = mainViewModel;
        this.brushManagement = mainViewModel.EBoardBrowserViewModel.FluidUI.Design!;
        this.screenBrushManagement = this.mainViewModel.FluidUI.Design!;

        this.fontManagement = mainViewModel.EBoardBrowserViewModel.FluidUI.Font!;
        this.screenFontManagement = this.mainViewModel.FluidUI.Font!;

        this.EBoardBrowserSwitch = eboardConfig.EBoardBrowserSwitch;
        this.ScreenControlSwitch = eboardConfig.ScreenControlSwitch;

        this.mainViewModel.EBoardBrowserViewModel.PropertyChanged += this.EBoardBrowserViewModel_PropertyChanged;

        this.SearchForPlugins();
    }

    public List<PluginRepresentationItem> FoundAddons { get; private set; } = [];

    public List<PluginRepresentationItem> FoundElements { get; private set; } = [];

    public List<PluginRepresentationItem> FoundShapes { get; private set; } = [];

    public List<PluginRepresentationItem> FoundAreas { get; private set; } = [];

    public List<PluginRepresentationItem> FoundTools { get; private set; } = [];

    public List<PluginRepresentationItem> FoundPlugins { get; private set; } = [];

    public IFluidUIDesignModel BrushManagement => this.brushManagement;

    public IFluidUIDesignModel ScreenBrushManagement => this.screenBrushManagement;

    public IFluidUIFontModel FontManagement => this.fontManagement;

    public IFluidUIFontModel ScreenFontManagement => this.screenFontManagement;

    public MainViewModel MainViewModel => this.mainViewModel;

    public EBoardBrowserViewModel EBoardBrowserViewModel => this.MainViewModel.EBoardBrowserViewModel;

    public void InvokePluginNTimes(Type pluginType, int count)
    {
        for (int i = 0; i < count; i++)
        {
            this.InvokePlugin(pluginType);
        }
    }

    internal void SearchForPlugins()
    {
        if (this.sDKPluginManager == null)
        {
            this.sDKPluginManager = new SDKPluginManager();
        }

        this.FoundPlugins = this.sDKPluginManager.FoundPlugins;
        this.FoundAddons = this.sDKPluginManager.FoundAddons;
        this.FoundElements = this.sDKPluginManager.FoundElements;
        this.FoundShapes = this.sDKPluginManager.FoundShapes;
        this.FoundAreas = this.sDKPluginManager.FoundAreas;

        var pluginItems = new List<PluginRepresentationItem>();
        foreach (var item in this.sDKPluginManager.FoundTools)
        {
            pluginItems.Add(new PluginRepresentationItem()
            {
                PluginHeader = item.PluginHeader,
                PluginLogo = item.PluginLogo,
                PluginName = item.PluginName,
            });
        }

        this.FoundTools = pluginItems;

        this.OnPropertyChanged(nameof(this.FoundPlugins));
        this.OnPropertyChanged(nameof(this.FoundAddons));
        this.OnPropertyChanged(nameof(this.FoundElements));
        this.OnPropertyChanged(nameof(this.FoundShapes));
        this.OnPropertyChanged(nameof(this.FoundAreas));
        this.OnPropertyChanged(nameof(this.FoundTools));
    }

    internal void InstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.sDKPluginManager.InstallPlugin(eBoardElementPluginBaseViewModel);

        this.SearchForPlugins();
    }

    internal void UninstallPlugin(PluginRepresentationItem eBoardElementPluginBaseViewModel)
    {
        this.sDKPluginManager.UninstallPlugin(eBoardElementPluginBaseViewModel);

        this.SearchForPlugins();
    }

    internal void Update()
    {
        this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
        this.OnPropertyChanged(nameof(this.ScreenFontManagement));
    }

    private void EBoardBrowserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            this.screenBrushManagement = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.FluidUI.Design!;
            this.screenFontManagement = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard.FluidUI.Font!;

            this.OnPropertyChanged(nameof(this.ScreenBrushManagement));
            this.OnPropertyChanged(nameof(this.ScreenFontManagement));
        }
    }

    private async void InvokeObjectAsPluginAsync(object s)
    {
        if (s is PluginRepresentationItem)
        {
            var pluginitem = s as PluginRepresentationItem;
            if (pluginitem != null)
            {
                var plugin = await this.sDKPluginManager.InvokePluginByName(pluginitem.PluginName ?? string.Empty);

                var selectedEboard = this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard;

                if (selectedEboard != null)
                {
                    this.sDKPluginManager.InvokePluginOnEboard(plugin, selectedEboard);
                }
            }
        }
    }

    partial void OnEBoardBrowserSwitchChanged(bool value)
    {
        if (value && this.ScreenControlSwitch)
        {
            this.ScreenControlSwitch = false;
        }
    }

    partial void OnScreenControlSwitchChanged(bool value)
    {
        if (value && this.EBoardBrowserSwitch)
        {
            this.EBoardBrowserSwitch = false;
        }
    }

    [RelayCommand]
    private async void InvokePlugin(object s)
    {
        this.InvokeObjectAsPluginAsync(s);
    }
}

// EOF