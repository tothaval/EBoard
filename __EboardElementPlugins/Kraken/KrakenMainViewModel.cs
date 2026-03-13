// <copyright file="KrakenMainViewModel.cs" company=".">
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
namespace Kraken;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
<<<<<<< Updated upstream
=======
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class KrakenMainViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string eboardViewModelTitle;

    [ObservableProperty]
    private string eboardViewModelData;

    private string pluginHeader = "Kraken Element";
    private string pluginName = "Kraken";

    public KrakenMainViewModel()
    {
        this.ElementScreenIntegrationConstraints = new EBoardSDK.Models.ElementScreenIntegrationConstraints(ElementInstantiationPolicy.Unconstrained);
    }

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary? ResourceDictionary => new() { Source = new Uri("/Kraken;component/KrakenResource.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(KrakenMainView);

    public override Type ElementPluginViewModel => typeof(KrakenMainViewModel);

    public override void RefreshInitialization()
    {
        base.RefreshInitialization();

        if (this.EBoardViewModel.FluidUI.DataBlock != null)
        {
            this.EboardViewModelTitle = this.EBoardViewModel.FluidUI.DataBlock.Title;

            var db = this.EBoardViewModel.FluidUI.DataBlock;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in this.EBoardViewModel.Elements)
            {
                stringBuilder.Append($"{item.Plugin.PluginName}<:::<{item.FluidUI.DataBlock?.Title}\n{item.FluidUI.DataBlock?.Text}\n\n");
            }

            this.EboardViewModelData = $"{db.Text}\n\n{db.IndexTextQuadValue1.Value1}\n\n" +
                $"{db.KeyTextQuadValueA.Value1}\n\n{db.IndexTextList.Count}\n\n{db.KeyTextList.Count}\n\n" +
                $"{this.EBoardViewModel.EBID}\n\n" +
                $"{this.EBoardViewModel.Elements.Count}" +
                $"{stringBuilder}";

        }

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