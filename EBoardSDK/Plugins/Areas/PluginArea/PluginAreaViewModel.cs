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

using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Eboard.Summoner;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class PluginAreaViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Plugin Area";
    private readonly string pluginName = "PluginArea";

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaViewModel"/> class.
    /// </summary>
    public PluginAreaViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public AreaViewModel<SummonerViewModel> AreaViewModel { get; set; } = new ();

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Area;

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
    public override Type? PluginModelType => typeof(PluginAreaModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(PluginAreaViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.AreaViewModel = new AreaViewModel<SummonerViewModel>(this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
            this.OnPropertyChanged(nameof(this.ElementViewModel));
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<PluginAreaModel>(path);

            if (data != null && data.Summonees != null && this.ElementViewModel != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(PluginAreaModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new PluginAreaModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
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
    }
}

// EOF