// <copyright file="PluginManagerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.PluginManager;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// TODO:
/// when refactoring and developing this towards a flexible plugin architecture, implement the
/// following condition:
/// Eboard category plugins can not be deinstalled and only be used from inside the eboard sdk.
/// eboard category can only be given to eboardsdk plugins or they won't be used after an to implemented assembly check.
/// </summary>
public partial class PluginManagerViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "PluginManager";
    private readonly string pluginHeader = "Plugin Manager";

    private PluginSelectionViewModel? pluginSelectionViewModel;

    [ObservableProperty]
    private string pluginFolder = string.Empty;

    [ObservableProperty]
    private string selectedFolder = string.Empty;

    [ObservableProperty]
    private PluginRepresentationItem? selectedPluginFile = null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PluginCount))]
    private List<PluginRepresentationItem> pluginsInSelectedFolder = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManagerViewModel"/> class.
    /// </summary>
    public PluginManagerViewModel()
    {
    }

    public PluginSelectionViewModel PluginSelectionViewModel => this.pluginSelectionViewModel;

    public int PluginCount => this.PluginsInSelectedFolder.Count;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new ();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(PluginManagerViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    /// <inheritdoc/>
    public async override void RefreshInitialization()
    {
        this.instantiatedAsElement = true;

        var dataManager = new SDKDataManager();

        var folder = await dataManager.GetInstalledPluginsDirectory();

        this.PluginFolder = folder;
        this.SelectedFolder = folder;
        this.PluginsInSelectedFolder = await dataManager.LoadPluginsFromInstalledPluginsDirectoryAsync();

        if (this.ElementViewModel != null)
        {
            this.pluginSelectionViewModel = new PluginSelectionViewModel(this.ElementViewModel.FluidUI, this.ShowSelectedPluginCategories, singleSelectionTarget: false);

            this.instantiatedAsElement = true;

            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);

            this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
            this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
        }

        this.Setup();
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Load(string path)"));
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Save(string path)"));
    }

    /// <inheritdoc/>
    protected override void Setup()
    {
    }

    private void ShowSelectedPluginCategories()
    {
        if (this.PluginSelectionViewModel == null || this.PluginSelectionViewModel.SelectedPlugins == null)
        {
            return;
        }

        this.PluginsInSelectedFolder = this.PluginSelectionViewModel.SelectedPlugins;
    }

    [RelayCommand]
    private void InstallPlugin()
    {
        var sdkPluginManager = new SDKPluginManager();

        if (this.SelectedPluginFile != null)
        {
            this.ElementViewModel?.ScreenViewModel?.InstallPlugin(this.SelectedPluginFile);
        }

        this.PluginsInSelectedFolder = sdkPluginManager.FoundPlugins;
    }

    [RelayCommand]
    private async void SelectFolder()
    {
        var dir = new SharedMethod_UI().SetSaveDirectory();

        if (!string.IsNullOrWhiteSpace(dir))
        {
            this.SelectedFolder = dir;

            var dataManager = new SDKDataManager();

            var plugins = await dataManager.LoadPluginRepresentationItemsAsync(dir);

            this.PluginsInSelectedFolder = plugins;
        }
    }

    [RelayCommand]
    private async Task ShowInstalledPlugins()
    {
        var dataManager = new SDKDataManager();
        var pluginManager = new SDKPluginManager();

        var plugins = pluginManager.FoundPlugins;

        this.SelectedFolder = await dataManager.GetInstalledPluginsDirectory();
        this.PluginsInSelectedFolder = plugins;

    }

    [RelayCommand]
    private void UninstallPlugin()
    {
        // plugin aus listen entfernen, mainwindowmenubar aktualisieren, dll nicht löschen.
        // tricky, muss zunächst eine Liste aller Plugins zugreifbar irgendwo haben, sonst
        // habe ich wie jetzt verschiedene zustände in den pluginmanager instanzen.
        var sdkPluginManager = new SDKPluginManager();

        if (this.SelectedPluginFile != null)
        {
            string question = "do you want to uninstall the selected plugin?\n\nWIP\nplugin uninstall is not permanent at the moment";
            string title = "plugin deinstallation";

            MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            this.ElementViewModel?.ScreenViewModel?.UninstallPlugin(this.SelectedPluginFile);
        }

        this.PluginsInSelectedFolder = sdkPluginManager.FoundPlugins;
    }
}

// EOF