// <copyright file="SoundMixMainViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Addons.SoundMix;

using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Addons.EEP_SoundMix;
using EBoardSDK.Plugins.Areas.FileLinkArea;
using EBoardSDK.Plugins.Elements.BasicAV;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.SharedMethods;
using System;
using System.Diagnostics.Metrics;
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

    public SoundMixMainViewModel()
    {
        this.BrushManagement ??= new BrushManagement();

        this.BorderManagement ??= new BorderManagement();

        this.PluginLogo ??= new ImageBrush();

        this.AreaViewModel = new AreaViewModel<BasicAVMainViewModel>();
    }

    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override ImageBrush PluginLogo { get; set; }

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

    public override string ElementPluginName => "SoundMix";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardSDK;component/Plugins/Addons/EEP_SoundMix/SoundMixResources.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(SoundMixMainView);

    public override Type ElementPluginViewModel => typeof(SoundMixMainViewModel);

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<SoundMixModel>(path);

            if (data != null)
            {
                var counter = 0;

                // TODO stuff
                foreach (var basicAVModelList in data.Links)
                {
                    this.AreaViewModel.AddHorizontal();

                    var basicAVViewModels = new List<BasicAVMainViewModel>();

                    foreach (var basicAVModel in basicAVModelList)
                    {
                        // ggf ueber Konstruktor refaktoring verkuerzen
                        var basicAVWM = new BasicAVMainViewModel();

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

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }
}

// EOF