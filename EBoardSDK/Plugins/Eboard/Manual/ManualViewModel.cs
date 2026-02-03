// <copyright file="ManualViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.Manual;

using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class ManualViewModel : EboardPluginBaseViewModel
{
    private readonly string pluginName = "Manual";
    private readonly string pluginHeader = "Manual";

    private string manualText = string.Empty;

    private string introductionText = string.Empty;
    private string mainWindowText = string.Empty;
    private string browserText = string.Empty;
    private string screenText = string.Empty;
    private string elementText = string.Empty;
    private string pluginText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManualViewModel"/> class.
    /// </summary>
    public ManualViewModel()
    {
    }

    public string IntroductionText => this.introductionText;

    public string ManualText => this.manualText;

    public string MainWindowText => this.mainWindowText;

    public string BrowserText => this.browserText;

    public string ScreenText => this.screenText;

    public string ElementText => this.elementText;

    public string PluginText => this.pluginText;

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
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(ManualViewModel);

    /// <inheritdoc/>
    public override bool InstantiatedAsElement => throw new NotImplementedException();

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.instantiatedAsElement = true;

            this.SetMainViewModel(this.ElementViewModel.ScreenViewModel.MainViewModel);
        }

        this.Setup();

        this.LoadManual();
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

    private async void LoadManual()
    {
        var data = new SDKDataManager();
        var about = await data.LoadManualAsync();

        this.introductionText = about.IntroductionText;
        this.mainWindowText = about.MainWindowText;
        this.browserText = about.BrowserText;
        this.screenText = about.ScreenText;
        this.elementText = about.ElementText;
        this.pluginText = about.PluginText;
        this.manualText = about.ManualText;

        this.OnPropertyChanged(nameof(this.IntroductionText));
        this.OnPropertyChanged(nameof(this.MainWindowText));
        this.OnPropertyChanged(nameof(this.BrowserText));
        this.OnPropertyChanged(nameof(this.ScreenText));
        this.OnPropertyChanged(nameof(this.ElementText));
        this.OnPropertyChanged(nameof(this.PluginText));
        this.OnPropertyChanged(nameof(this.ManualText));
    }
}

// EOF