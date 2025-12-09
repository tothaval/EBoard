// <copyright file="TwoXThreeImageAreaViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;

using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Elements.Image;
using EBoardSDK.SharedMethods;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class TwoXThreeImageAreaViewModel : EBoardElementPluginBaseViewModel
{
    //public AreaViewModel<ImageViewModel> AreaViewModel { get; }

    public ImageViewModel ImageViewModel1 { get; } = new();

    public ImageViewModel ImageViewModel2 { get; } = new();

    public ImageViewModel ImageViewModel3 { get; } = new();

    public ImageViewModel ImageViewModel4 { get; } = new();

    public ImageViewModel ImageViewModel5 { get; } = new();

    public ImageViewModel ImageViewModel6 { get; } = new();

    public override PluginCategories PluginCategory => PluginCategories.Area;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "2X3V Image Area";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "TwoX3VImageArea";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "TwoX3VImageArea"; // mal konsolidieren, was benötigt wird und was doppelt ist.

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(TwoXThreeImageAreaView);

    public override Type ElementPluginViewModel => typeof(TwoXThreeImageAreaViewModel);

    public TwoXThreeImageAreaViewModel()
    {
        //this.AreaViewModel = new AreaViewModel<ImageViewModel>(); // ma kucken ob das noch genutzt wird.

        //this.AreaViewModel.CreateMatrix(2, 3);
    }

    [RelayCommand]
    private void Reset()
    {
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<TwoXThreeImageAreaModel>(path);

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

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }
}

// EOF