// <copyright file="ImageViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Elements.Image;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.SharedMethods;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ImageViewModel : EBoardElementPluginBaseViewModel
{
    private readonly string imageDataFileName = "imagedata.xml";

    [ObservableProperty]
    private ImageBrush imageBrush = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsImageNotSet))]
    private bool isLinked = false;

    [ObservableProperty]
    private string linkTargetPath;

    public bool IsImageNotSet => !this.IsLinked;

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Image Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "Image";

    public override bool NoDefaultBorders { get; } = false;

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Image";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardSDK;component/Plugins/Elements/Image/ImageResources.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(ImageView);

    public override Type ElementPluginViewModel => typeof(ImageViewModel);

    private void ApplyImage()
    {
        var shared = new SharedMethod_UI();

        this.ImageBrush = (ImageBrush)shared.ChangeBackgroundToImage(this.ImageBrush, this.LinkTargetPath);

        this.IsLinked = true;
    }

    private void LinkFile(string fileName)
    {
        try
        {
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                this.LinkTargetPath = fileInfo.FullName;

                this.ApplyImage();
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    [RelayCommand]
    private void Reset()
    {
        this.IsLinked = false;
    }

    [RelayCommand]
    private void SetImage()
    {
        var shared = new SharedMethod_UI();

        this.LinkTargetPath = shared.SetBackgroundImage(this.LinkTargetPath);

        this.ApplyImage();
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<ImageModel>(path);

            if (data != null)
            {
                this.LinkFile(data.LinkTargetPath);

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

        var model = new ImageModel() { LinkTargetPath = this.LinkTargetPath };

        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }
}
