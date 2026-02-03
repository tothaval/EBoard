// <copyright file="AboutViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.About;

using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class AboutViewModel : PluginBaseViewModel
{
    private readonly string pluginName = "About";
    private readonly string pluginHeader = "About Eboard";

    private string aboutText = string.Empty;

    private string licenseText = string.Empty;

    private string version = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AboutViewModel"/> class.
    /// </summary>
    public AboutViewModel()
    {
    }

    public string AboutText => this.aboutText;

    public string LicenseText => this.licenseText;

    public string Version => this.version;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; }

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
    public override Type PluginViewModelType => typeof(AboutViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        base.RefreshInitialization();

        this.LoadLicenseAndAbout();
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

    private async void LoadLicenseAndAbout()
    {
        var data = new SDKDataManager();
        var about = await data.LoadLicenseAndAboutAsync();

        this.aboutText = about.AboutText;
        this.licenseText = about.LicenseText;
        this.version = about.Version;

        this.OnPropertyChanged(nameof(this.AboutText));
        this.OnPropertyChanged(nameof(this.LicenseText));
        this.OnPropertyChanged(nameof(this.Version));
    }
}

// EOF