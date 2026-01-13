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
namespace EBoardSDK.Plugins.Elements.Manual;
using EBoardSDK.Enums;
using EBoardSDK.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ManualViewModel : EBoardElementPluginBaseViewModel
{
    private string manualText = string.Empty;

    private string introductionText = string.Empty;
    private string mainWindowText = string.Empty;
    private string browserText = string.Empty;
    private string screenText = string.Empty;
    private string elementText = string.Empty;
    private string pluginText = string.Empty;

    private string pluginName = "Manual";
    private string pluginHeader = "Manual";

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

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

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

    public override Type ElementPluginView => typeof(ManualView);

    public override Type ElementPluginViewModel => typeof(ManualViewModel);

    public override void RefreshInitialization()
    {
        base.RefreshInitialization();

        this.LoadManual();
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
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