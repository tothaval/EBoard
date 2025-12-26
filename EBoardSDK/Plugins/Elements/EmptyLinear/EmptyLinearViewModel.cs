// <copyright file="EmptyLinearViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.EmptyLinear;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class EmptyLinearViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    [ObservableProperty]
    private LinearGradientBrush background = new LinearGradientBrush(
        [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.Navy, 0.5)],
        new Point(0, 0),
        new Point(0.5, 1));

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Empty Linear Element";

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    private string pluginName = "EmptyLinear";

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override string ElementPluginName => "Linear";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EmptyLinearView);

    public override Type ElementPluginViewModel => typeof(EmptyLinearViewModel);

    public EmptyLinearViewModel() => this.InstantiateProperties();

    private void InstantiateProperties()
    {
        //this.BorderManagement = new BorderManagement();
        //this.BrushManagement = new BrushManagement();
    }

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