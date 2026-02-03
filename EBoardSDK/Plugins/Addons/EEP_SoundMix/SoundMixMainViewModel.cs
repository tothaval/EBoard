// <copyright file="SoundMixMainViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Addons.SoundMix;

using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Addons.EEP_SoundMix;
using EBoardSDK.Plugins.Elements.BasicAV;
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

public partial class SoundMixMainViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "SoundMix Addon Element";
    private readonly string pluginName = "SoundMix";

    private SoundMixModel? data;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundMixMainViewModel"/> class.
    /// </summary>
    public SoundMixMainViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public AreaViewModel<BasicAVMainViewModel>? AreaViewModel { get; set; }

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Addon;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(SoundMixModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(SoundMixMainViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            if (this.AreaViewModel == null)
            {
                this.AreaViewModel = new AreaViewModel<BasicAVMainViewModel>(this.ElementViewModel);
            }

            if (this.AreaViewModel.ElementViewModel == null)
            {
                this.AreaViewModel.SetElementViewModel(this.ElementViewModel);
            }

            this.OnPropertyChanged(nameof(this.AreaViewModel));
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<SoundMixModel>(path);

            if (data != null && this.ElementViewModel != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(SoundMixModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new SoundMixModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is SoundMixModel soundMixModel)
        {
            this.ApplyModel(soundMixModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<SoundMixModel>(json!);

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
        var model = new SoundMixModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(SoundMixModel soundMixModel)
    {
        if (this.ElementViewModel == null || this.AreaViewModel == null)
        {
            return;
        }

        this.AreaViewModel.ShowMatrixControls = soundMixModel.ShowMatrixControls;

        this.data = soundMixModel;

        var counter = 0;

        foreach (var basicAVModelList in this.data.Links)
        {
            this.AreaViewModel?.AddHorizontal();

            var basicAVViewModels = new List<BasicAVMainViewModel>();

            foreach (var basicAVModel in basicAVModelList)
            {
                var basicAVWM = new BasicAVMainViewModel();
                basicAVWM.SetElementViewModel(this.ElementViewModel);

                basicAVWM.InsertModel(basicAVModel);

                basicAVViewModels.Add(basicAVWM);
            }

            this.AreaViewModel?.InsertViewModelList(basicAVViewModels, counter);

            counter++;
        }
    }
}

// EOF