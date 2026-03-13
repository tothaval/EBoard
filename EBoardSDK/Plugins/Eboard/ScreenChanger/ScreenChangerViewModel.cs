// <copyright file="ScreenChangerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.ScreenChanger;

using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

public partial class ScreenChangerViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "ScreenChanger";
    private readonly string pluginHeader = "ScreenChanger";

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenChangerViewModel"/> class.
    /// </summary>
    public ScreenChangerViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuBarViewModel"/> class.
    /// </summary>
    /// <param name="nonElementPlugin"></param>
    /// <param name="mainViewModel"></param>
    public ScreenChangerViewModel(bool nonElementPlugin, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.instantiatedAsElement = !nonElementPlugin;

        this.Setup();

        this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
        this.OnPropertyChanged(nameof(this.MainViewModel));
    }

    public NavigationContextViewModel? NavigationContextViewModel => this.MainViewModel?.NavigationContextViewModel;

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
    public override Type PluginViewModelType => typeof(ScreenChangerViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
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

    /// <inheritdoc/>
    protected override void Setup()
    {
        if (this.mainViewModel == null)
        {
            return;
        }
    }

    [RelayCommand]
    private void FirstEboard()
    {
        this.NavigationContextViewModel?.SwitchToEboard("First");
    }

    [RelayCommand]
    private void LastEboard()
    {
        this.NavigationContextViewModel?.SwitchToEboard("Last");
    }

    [RelayCommand]
    private void NextEboard()
    {
        this.NavigationContextViewModel?.SwitchToEboard("Next");
    }

    [RelayCommand]
    private void PrevEboard()
    {
        this.NavigationContextViewModel?.SwitchToEboard("Prev");
    }
}

// EOF