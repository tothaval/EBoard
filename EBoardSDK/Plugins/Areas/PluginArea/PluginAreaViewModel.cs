// <copyright file="PluginAreaViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Areas.PluginArea;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
<<<<<<< Updated upstream
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Plugins.Tools.Coordinates;
using EBoardSDK.Plugins.Tools.Summoner;
=======
using EBoardSDK.Models;
using EBoardSDK.Plugins.Eboard.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
>>>>>>> Stashed changes
using System;
using System.CodeDom;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class PluginAreaViewModel : EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "Plugin Area";
    private string pluginName = "PluginArea";

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaViewModel"/> class.
    /// </summary>
    public PluginAreaViewModel()
    {
    }

    public AreaViewModel<SummonerViewModel> AreaViewModel { get; set; }

    public override PluginCategories PluginCategory => PluginCategories.Area;

<<<<<<< Updated upstream
    public override bool NoDefaultBorders { get; } = false;
=======
    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();
>>>>>>> Stashed changes

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

<<<<<<< Updated upstream
    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }
=======
    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();
>>>>>>> Stashed changes

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(PluginAreaView);

    public override Type ElementPluginViewModel => typeof(PluginAreaViewModel);

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            if (this.AreaViewModel == null)
            {
                this.AreaViewModel = new AreaViewModel<SummonerViewModel>(this.ElementViewModel);
            }

            if (this.AreaViewModel.ElementViewModel == null)
            {
                this.AreaViewModel.SetElementViewModel(this.ElementViewModel);
            }

            this.OnPropertyChanged(nameof(this.AreaViewModel));
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<PluginAreaModel>(path);

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

                this.OnPropertyChanged(nameof(this.AreaViewModel));

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

        var model = new PluginAreaModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
<<<<<<< Updated upstream
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
=======
            return;
        }

        if (model is PluginAreaModel pluginAreaModel)
        {
            this.ApplyModel(pluginAreaModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<PluginAreaModel>(json!);

            if (jsonParsed != null)
            {
                this.ApplyModel(jsonParsed);
            }
        }
        catch (JsonException jsonEx)
        {
            Log.Error(jsonEx.Message);

            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);

            throw;
        }
    }

    public override void PrepareCopy()
    {
        var model = new PluginAreaModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(PluginAreaModel pluginAreaModel)
    {
        this.RefreshInitialization();

        if (this.ElementViewModel == null || this.AreaViewModel == null)
        {
            return;
        }

        this.AreaViewModel.ShowMatrixControls = pluginAreaModel.ShowMatrixControls;

        var counter = 0;

        foreach (var modelList in pluginAreaModel.Summonees)
        {
            this.AreaViewModel.AddHorizontal();

            var viewModels = new List<SummonerViewModel>();

            foreach (var model in modelList)
            {
                var vm = new SummonerViewModel();

                vm.SetElementViewModel(this.ElementViewModel);

                vm.InsertModel(model);

                viewModels.Add(vm);
            }

            this.AreaViewModel.InsertViewModelList(viewModels, counter);

            counter++;
        }

        this.OnPropertyChanged(nameof(this.AreaViewModel));
>>>>>>> Stashed changes
    }
}

// EOF