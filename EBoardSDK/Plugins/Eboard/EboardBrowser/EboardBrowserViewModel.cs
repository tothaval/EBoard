// <copyright file="EboardBrowserViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.EboardBrowser;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.ScreenSetup;
using EBoardSDK.Enums;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class EboardBrowserViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "EboardBrowser";
    private readonly string pluginHeader = "Eboard Browser";

    private NavigationContextViewModel navigationContext;

    [ObservableProperty]
    private bool browserPanelVisible = true;

    [ObservableProperty]
    private bool screenSetupVisible = true;

    [ObservableProperty]
    private int maxWidth = 250;

    [ObservableProperty]
    private int maxHeight = 250;

    [ObservableProperty]
    private int currentSelectionID;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ActiveScreenIsNotNull))]
    private ScreenViewModel? selectedEboard;

    [ObservableProperty]
    private ScreenSetupViewModel? screenSetupViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="EboardBrowserViewModel"/> class.
    /// </summary>
    public EboardBrowserViewModel()
    {
        this.SetMenuItemViewModel(new EboardBrowserMenuItemViewModel(this));
        this.SetMenuItem(new EboardBrowserMenuItem(this.MenuItemViewModel!));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EboardBrowserViewModel"/> class.
    /// </summary>
    public EboardBrowserViewModel(NavigationContextViewModel navigationContextViewModel)
        : this()
    {
        this.navigationContext = navigationContextViewModel;
        this.instantiatedAsElement = false;

        this.OnPropertyChanged(nameof(this.NavigationContextViewModel));
    }

    public bool ActiveScreenIsNotNull => this.SelectedEboard != null;

    public NavigationContextViewModel NavigationContextViewModel => this.navigationContext;

    public bool ScreenExists => this.SelectedEboard != null;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(EboardBrowserViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.instantiatedAsElement = true;

            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);
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

    internal void DeselectEboard()
    {
        this.SelectedEboard = null;
    }

    /// <summary>
    /// Calls <see cref="RefreshScreenSetup()"/>,
    /// which can change <see cref="CurrentSelectionID"/>.
    /// It will create a new <see cref="ScreenSetupViewModel"/>
    /// instance on every call.
    /// </summary>
    internal void RefreshSelectedEboardData()
    {
        this.RefreshScreenSetup();
    }

    internal void SwitchToEboard(string target)
    {
        if (this.NavigationContextViewModel.Eboards.Count > 0)
        {
            switch (target.ToLower())
            {
                case "first":
                    this.SelectedEboard = this.NavigationContextViewModel.Eboards.First();
                    break;
                case "prev":
                    this.SwitchToPrevEboard();
                    break;

                case "next":
                    this.SwitchToNextEboard();
                    break;

                case "last":
                    this.SelectedEboard = this.NavigationContextViewModel.Eboards.Last();
                    break;

                default:
                    break;
            }
        }
    }

    internal void SwitchToNextEboard()
    {
        if (this.SelectedEboard == null)
        {
            this.SwitchToEboard("Last");
            return;
        }

        for (int i = 0; i < this.NavigationContextViewModel.Eboards.Count; i++)
        {
            if (this.NavigationContextViewModel.Eboards[i] == this.SelectedEboard)
            {
                if (i + 1 < this.NavigationContextViewModel.Eboards.Count)
                {
                    this.SelectedEboard = this.NavigationContextViewModel.Eboards[i + 1];

                    break;
                }
            }
        }
    }

    internal void SwitchToPrevEboard()
    {
        if (this.SelectedEboard == null)
        {
            this.SwitchToEboard("First");
            return;
        }

        for (int i = 0; i < this.NavigationContextViewModel.Eboards.Count; i++)
        {
            if (this.NavigationContextViewModel.Eboards[i] == this.SelectedEboard)
            {
                if (i - 1 >= 0)
                {
                    this.SelectedEboard = this.NavigationContextViewModel.Eboards[i - 1];

                    break;
                }
            }
        }
    }

    /// <inheritdoc/>
    protected override void Setup()
    {
        if (this.MainViewModel != null)
        {
            if (this.NavigationContextViewModel == null)
            {
                this.navigationContext = this.MainViewModel.NavigationContextViewModel;
            }
        }

        if (this.NavigationContextViewModel != null)
        {
            this.ScreenSetupViewModel = new ScreenSetupViewModel(this.NavigationContextViewModel);
        }
    }

    private void RefreshScreenSetup()
    {
        this.ScreenSetupViewModel?.Dispose();

        this.ScreenSetupViewModel = new ScreenSetupViewModel(this.NavigationContextViewModel);

        if (this.SelectedEboard != null)
        {
            this.CurrentSelectionID = this.NavigationContextViewModel.Eboards.IndexOf(this.SelectedEboard) + 1;
        }
    }

    partial void OnSelectedEboardChanged(ScreenViewModel? value)
    {
        this.RefreshScreenSetup();
    }

    partial void OnSelectedEboardChanging(ScreenViewModel? oldValue, ScreenViewModel? newValue)
    {
        if (oldValue == null || newValue == null || oldValue.Equals(newValue))
        {
            return;
        }

        oldValue.BecomesInactive();

        newValue.BecomesActive();
    }

    [RelayCommand]
    private void AddEboard()
    {
        if (this.ScreenSetupViewModel == null)
        {
            this.ScreenSetupViewModel = new ScreenSetupViewModel(this.NavigationContextViewModel);
        }

        var screen = this.ScreenSetupViewModel.GetNewScreen();

        this.NavigationContextViewModel.AddScreenViewModel(screen);
    }

    [RelayCommand]
    private void DeleteAllScreens()
    {
        this.NavigationContextViewModel?.DeleteAllScreens();
    }

    [RelayCommand]
    private void DeleteSelectedScreen()
    {
        this.NavigationContextViewModel?.RemoveActiveScreen();
    }

    [RelayCommand]
    private void DeleteSelectedScreens()
    {
        this.NavigationContextViewModel?.RemoveSelectedEboards();
    }

    [RelayCommand]
    private void Deselect()
    {
        this.NavigationContextViewModel?.DeselectEboard();
    }

    [RelayCommand]
    private void ToggleBrowserPanelVisibility()
    {
        this.BrowserPanelVisible = !this.BrowserPanelVisible;
    }

    [RelayCommand]
    private void ToggleScreenSetupVisibility()
    {
        this.ScreenSetupVisible = !this.ScreenSetupVisible;
    }
}

// EOF