// <copyright file="ShapeSummonerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Tools.Summoner;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Areas.ShapeArea;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Plugins.Tools.Coordinates;
using EBoardSDK.Plugins.Tools.Summoner;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ShapeSummonerViewModel : SummonerViewModel
{
    private string pluginHeader = "Shape Summoner";
    private string pluginName = "ShapeSummoner";

    /// <summary>
    /// Initializes a new instance of the <see cref="ShapeSummonerViewModel"/> class.
    /// </summary>
    public ShapeSummonerViewModel()
    {
    }

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

    public override Type ElementPluginView => typeof(SummonerView);

    public override Type ElementPluginViewModel => typeof(ShapeSummonerViewModel);

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            var pluginManager = new SDKPluginManager();

            this.Plugins = pluginManager.FoundShapes;
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<ShapeSummonerModel>(path);

            // TODO ggf hier und andernorts generisch machen und refactorn
            if (data != null)
            {
                var counter = 0;

                // TODO stuff
                //foreach (var linkModelList in data.Links)
                //{
                //    this.AreaViewModel.AddHorizontal();

                //    var linkViewModels = new List<LinkViewModel>();

                //    foreach (var linkModel in linkModelList)
                //    {
                //        // ggf ueber Konstruktor refaktoring verkuerzen
                //        var linkVM = new LinkViewModel();

                //        linkVM.SetEBoardAndElementViewModel(this.EBoardViewModel, this.ElementViewModel);

                //        linkVM.InsertLinkModel(linkModel);

                //        linkViewModels.Add(linkVM);
                //    }

                //    this.AreaViewModel.InsertViewModelList(linkViewModels, counter);

                //    counter++;
                //}

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = "" };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new ShapeSummonerModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }
}

// EOF