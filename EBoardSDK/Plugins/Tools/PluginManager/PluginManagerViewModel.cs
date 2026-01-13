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
namespace EBoardSDK.Plugins.Tools.PluginManager;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class PluginManagerViewModel : EBoardElementPluginBaseViewModel
{
    private PluginSelectionViewModel? pluginSelectionViewModel;

    [ObservableProperty]
    private string pluginFolder;

    [ObservableProperty]
    private string selectedFolder;

    [ObservableProperty]
    private PluginRepresentationItem? selectedPluginFile = null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PluginCount))]
    private List<PluginRepresentationItem> pluginsInSelectedFolder = new();

    private string pluginName = "PluginManager";

    private string pluginHeader = "Plugin Manager";

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManagerViewModel"/> class.
    /// </summary>
    public PluginManagerViewModel()
    {
    }

    public PluginSelectionViewModel PluginSelectionViewModel => this.pluginSelectionViewModel;

    public int PluginCount => this.PluginsInSelectedFolder.Count;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(PluginManagerView);

    public override Type ElementPluginViewModel => typeof(PluginManagerViewModel);

    public async override void RefreshInitialization()
    {
        var dataManager = new SDKDataManager();

        var folder = await dataManager.GetInstalledPluginsDirectory();

        this.PluginFolder = folder;
        this.SelectedFolder = folder;
        this.PluginsInSelectedFolder = await dataManager.LoadPluginsFromInstalledPluginsDirectoryAsync();

        if (this.ElementViewModel != null)
        {
            this.pluginSelectionViewModel = new PluginSelectionViewModel(this.ElementViewModel, this.ShowSelectedPluginCategories);

            this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
        }
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        if (this.ElementViewModel != null)
        {

        }

        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
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
            this.ElementViewModel?.EBoardViewModel?.InstallPlugin(this.SelectedPluginFile);
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

            this.ElementViewModel?.EBoardViewModel?.UninstallPlugin(this.SelectedPluginFile);
        }

        this.PluginsInSelectedFolder = sdkPluginManager.FoundPlugins;
    }
}

// EOF