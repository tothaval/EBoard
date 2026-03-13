// <copyright file="TwoXThreeImageAreaViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;

using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Elements.Image;
<<<<<<< Updated upstream
=======
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
>>>>>>> Stashed changes
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class TwoXThreeImageAreaViewModel : EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "2X3V Image Area";
    private string pluginName = "TwoX3VImageArea";

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoXThreeImageAreaViewModel"/> class.
    /// </summary>
    public TwoXThreeImageAreaViewModel()
    {
<<<<<<< Updated upstream
=======
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.ImageViewModel1.SetContainerIsArea();
        this.ImageViewModel2.SetContainerIsArea();
        this.ImageViewModel3.SetContainerIsArea();
        this.ImageViewModel4.SetContainerIsArea();
        this.ImageViewModel5.SetContainerIsArea();
        this.ImageViewModel6.SetContainerIsArea();
>>>>>>> Stashed changes
    }

    public ImageViewModel ImageViewModel1 { get; } = new();

    public ImageViewModel ImageViewModel2 { get; } = new();

    public ImageViewModel ImageViewModel3 { get; } = new();

    public ImageViewModel ImageViewModel4 { get; } = new();

    public ImageViewModel ImageViewModel5 { get; } = new();

    public ImageViewModel ImageViewModel6 { get; } = new();

    public override PluginCategories PluginCategory => PluginCategories.Area;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(TwoXThreeImageAreaView);

    public override Type ElementPluginViewModel => typeof(TwoXThreeImageAreaViewModel);

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<TwoXThreeImageAreaModel>(path);

            if (data != null)
            {
                this.ImageViewModel1.SetLinkedFile(data.Link1);
                this.ImageViewModel2.SetLinkedFile(data.Link2);
                this.ImageViewModel3.SetLinkedFile(data.Link3);
                this.ImageViewModel4.SetLinkedFile(data.Link4);
                this.ImageViewModel5.SetLinkedFile(data.Link5);
                this.ImageViewModel6.SetLinkedFile(data.Link6);

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = string.Empty };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new TwoXThreeImageAreaModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    [RelayCommand]
    private void Reset()
    {
    }
}

// EOF