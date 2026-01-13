// <copyright file="ImageViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.Image;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
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
    private ImageBrush? imageBrush = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsImageNotSet))]
    private bool isLinked = false;

    [ObservableProperty]
    private string linkTargetPath = string.Empty;

    [ObservableProperty]
    private double opacityValue = 1.0;

    private string pluginHeader = "Image Element";
    private string pluginName = "Image";

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageViewModel"/> class.
    /// </summary>
    public ImageViewModel()
    {
    }

    public bool IsImageNotSet => !this.IsLinked;

    public override PluginCategories PluginCategory => PluginCategories.Element;

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

    public override Type ElementPluginView => typeof(ImageView);

    public override Type ElementPluginViewModel => typeof(ImageViewModel);

    public void SetLinkedFile(string path)
    {
        this.LinkFile(path);
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        try
        {
            var data = await Loader.LoadJsonFile<ImageModel>(path);

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

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = string.Empty };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new ImageModel() { LinkTargetPath = this.LinkTargetPath };

        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    private void ApplyImage()
    {
        var shared = new SharedMethod_UI();
        try
        {
            this.ImageBrush = (ImageBrush)shared.ChangeBackgroundToImage(this.ImageBrush, this.LinkTargetPath);

            this.IsLinked = true;
        }
        catch (Exception)
        {
            this.IsLinked = false;
        }
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

    partial void OnOpacityValueChanged(double value)
    {
        this.ImageBrush.Opacity = value;
    }

    [RelayCommand]
    private void Reset()
    {
        this.IsLinked = false;
        this.ImageBrush = null;
    }

    [RelayCommand]
    private void SetImage()
    {
        var shared = new SharedMethod_UI();

        this.LinkTargetPath = shared.UserSelectImage(this.LinkTargetPath);

        this.LinkFile(this.LinkTargetPath);
    }
}

// EOF