// <copyright file="ScreenDataBoardViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.ScreenDataBoard;

using EBoardSDK.Controls.ScreenSetup;
using EBoardSDK.Enums;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class ScreenDataBoardViewModel : EboardPluginBaseViewModel
{
    private NavigationContextViewModel? viewModel;
    private ScreenSetupViewModel? screenSetupViewModel;

    private string pluginName = "ScreenDataBoard";

    private string pluginHeader = "Screen Data Board";

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreenDataBoardViewModel"/> class.
    /// </summary>
    public ScreenDataBoardViewModel()
    {
    }

    public NavigationContextViewModel? ViewModel => this.viewModel;

    public ScreenSetupViewModel? ScreenSetupViewModel => this.screenSetupViewModel;

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
    public override Type PluginViewModelType => typeof(ScreenDataBoardViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => this.instantiatedAsElement;

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
    protected override void Setup()
    {
        if (this.mainViewModel == null)
        {
            return;
        }

        this.viewModel = this.MainViewModel.NavigationContextViewModel;
        this.screenSetupViewModel = new ScreenSetupViewModel(this.ViewModel);

        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.ScreenSetupViewModel));
    }
}

// EOF