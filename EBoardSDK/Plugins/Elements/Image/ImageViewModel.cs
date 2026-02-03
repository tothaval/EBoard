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
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ImageViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Image Element";
    private readonly string pluginName = "Image";

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

    [ObservableProperty]
    private Point ratio = new Point(1.0, 1.0);

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageViewModel"/> class.
    /// </summary>
    public ImageViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.SetMenuItemViewModel(new ImageMenuItemViewModel(this));
        this.SetMenuItem(new ImageMenuItem(this.MenuItemViewModel!));
    }

    public bool IsImageNotSet => !this.IsLinked;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(ImageModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(ImageViewModel);

    public void SetLinkedFile(string path)
    {
        this.LinkFile(path);
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        try
        {
            var data = await new SDKDataManager().LoadPluginContent<ImageModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(ImageModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new ImageModel() { LinkTargetPath = this.LinkTargetPath };

        if (new DirectoryInfo(path).Exists)
        {
            path = System.IO.Path.Combine(path, this.imageDataFileName);
        }

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is ImageModel imageModel)
        {
            this.ApplyModel(imageModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<ImageModel>(json!);

            if (jsonParsed != null)
            {
                this.ApplyModel(jsonParsed);
            }
        }
        catch (JsonException jsonEx)
        {
            Log.Error(jsonEx.Message);

            this.Reset();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);

            this.Reset();
        }
    }

    public override void PrepareCopy()
    {
        var model = new ImageModel(this);

        this.SetModel(model);
    }

    internal void ResetImage()
    {
        this.IsLinked = false;
        this.ImageBrush = null;
    }

    private void ApplyImage()
    {
        try
        {
            var brush = FluidUIDesignDefaultPropertyFactory.GetImageBrush(this.LinkTargetPath);

            if (brush is ImageBrush)
            {
                this.ImageBrush = brush as ImageBrush;

                this.IsLinked = true;
            }

            if (this.ElementViewModel == null)
            {
                return;
            }

            var manager = new FluidUISizeManager(this.ElementViewModel);

            manager.SetWidth(50);
            manager.SetHeight(50);
        }
        catch (Exception)
        {
            this.IsLinked = false;
        }
    }

    private void ApplyModel(ImageModel imageModel)
    {
        this.LinkFile(imageModel.LinkTargetPath ?? string.Empty);
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
        if (this.ImageBrush == null)
        {
            return;
        }

        this.ImageBrush.Opacity = value;
    }

    partial void OnRatioChanged(Point value)
    {
        if (this.ElementViewModel == null || this.ElementViewModel.ElementView == null)
        {
            return;
        }

        var manager = new FluidUISizeManager(this.ElementViewModel);

        var width = manager.GetWidth();
        var height = manager.GetHeight();

        if (width == -1)
        {
            var actual = this.ElementViewModel.ElementView.ActualWidth;

            width = (int)actual;
        }

        if (height == -1)
        {
            var actual = this.ElementViewModel.ElementView.ActualHeight;

            height = (int)actual;
        }

        var sizeX = width * value.X;

        var sizeY = height * value.Y;

        manager.SetWidth(sizeX);
        manager.SetHeight(sizeY);

        this.Ratio = new Point(1, 1);
    }

    [RelayCommand]
    private void Reset()
    {
        this.ResetImage();
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