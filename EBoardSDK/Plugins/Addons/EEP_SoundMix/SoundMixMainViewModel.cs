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

using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Addons.EEP_SoundMix;
using EBoardSDK.Plugins.Elements.BasicAV;
<<<<<<< Updated upstream
=======
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
>>>>>>> Stashed changes
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class SoundMixMainViewModel : EBoardElementPluginBaseViewModel
{
    public AreaViewModel<BasicAVMainViewModel> AreaViewModel { get; }

    private string pluginHeader = "SoundMix Addon Element";

    private string pluginName = "SoundMix";

    private SoundMixModel? data;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundMixMainViewModel"/> class.
    /// </summary>
    public SoundMixMainViewModel()
    {
        this.AreaViewModel = new AreaViewModel<BasicAVMainViewModel>(this.ElementViewModel);
    }

    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override bool NoDefaultBorders { get; } = false;

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(SoundMixMainView);

    public override Type ElementPluginViewModel => typeof(SoundMixMainViewModel);

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.AreaViewModel.SetElementViewModel(this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<SoundMixModel>(path);

            if (data != null)
            {
                this.data = data;

                var counter = 0;

                // TODO stuff
                foreach (var basicAVModelList in this.data.Links)
                {
                    this.AreaViewModel.AddHorizontal();

                    var basicAVViewModels = new List<BasicAVMainViewModel>();

                    foreach (var basicAVModel in basicAVModelList)
                    {
                        // ggf ueber Konstruktor refaktoring verkuerzen
                        var basicAVWM = new BasicAVMainViewModel();
                        basicAVWM.SetEBoardAndElementViewModel(this.EBoardViewModel, this.ElementViewModel);

                        var result = basicAVWM.InsertBasicAVModel(basicAVModel);

                        basicAVViewModels.Add(basicAVWM);
                    }

                    this.AreaViewModel.InsertViewModelList(basicAVViewModels, counter);

                    counter++;
                }

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

        var model = new SoundMixModel(this);

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
        this.RefreshInitialization();

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

        this.OnPropertyChanged(nameof(this.AreaViewModel));
>>>>>>> Stashed changes
    }
}

// EOF