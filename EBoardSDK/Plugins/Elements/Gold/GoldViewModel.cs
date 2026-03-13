// <copyright file="GoldViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.Gold;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
<<<<<<< Updated upstream
=======
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class GoldViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    private string pluginHeader = "Gold Element";
    private string pluginName = "Gold";

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldViewModel"/> class.
    /// </summary>
    public GoldViewModel()
    {
    }

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

<<<<<<< Updated upstream
    public override string PluginHeader
=======
    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(GoldViewModel);

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            var manager = new FluidUISizeManager(this.ElementViewModel);

            manager.SetWidth(600.0);
        }
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Load(string path)
>>>>>>> Stashed changes
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

    public override Type ElementPluginView => typeof(GoldView);

    public override Type ElementPluginViewModel => typeof(GoldViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }
}

// EOF