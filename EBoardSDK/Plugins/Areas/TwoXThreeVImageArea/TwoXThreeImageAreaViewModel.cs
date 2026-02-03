// <copyright file="TwoXThreeImageAreaViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;

using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.Image;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class TwoXThreeImageAreaViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "2X3V Image Area";
    private readonly string pluginName = "TwoX3VImageArea";

    /// <summary>
    /// Initializes a new instance of the <see cref="TwoXThreeImageAreaViewModel"/> class.
    /// </summary>
    public TwoXThreeImageAreaViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public ImageViewModel ImageViewModel1 { get; } = new ();

    public ImageViewModel ImageViewModel2 { get; } = new ();

    public ImageViewModel ImageViewModel3 { get; } = new ();

    public ImageViewModel ImageViewModel4 { get; } = new ();

    public ImageViewModel ImageViewModel5 { get; } = new ();

    public ImageViewModel ImageViewModel6 { get; } = new ();

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
    public override Type? PluginModelType => typeof(TwoXThreeImageAreaModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(TwoXThreeImageAreaViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<TwoXThreeImageAreaModel>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(TwoXThreeImageAreaModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new TwoXThreeImageAreaModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is TwoXThreeImageAreaModel twoXThreeImageAreaModel)
        {
            this.ApplyModel(twoXThreeImageAreaModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<TwoXThreeImageAreaModel>(json!);

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
        var model = new TwoXThreeImageAreaModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(TwoXThreeImageAreaModel twoXThreeImageAreaModel)
    {
        this.ImageViewModel1.SetLinkedFile(twoXThreeImageAreaModel.Link1);
        this.ImageViewModel2.SetLinkedFile(twoXThreeImageAreaModel.Link2);
        this.ImageViewModel3.SetLinkedFile(twoXThreeImageAreaModel.Link3);
        this.ImageViewModel4.SetLinkedFile(twoXThreeImageAreaModel.Link4);
        this.ImageViewModel5.SetLinkedFile(twoXThreeImageAreaModel.Link5);
        this.ImageViewModel6.SetLinkedFile(twoXThreeImageAreaModel.Link6);
    }

    [RelayCommand]
    private void Reset()
    {
        this.ImageViewModel1.ResetImage();
        this.ImageViewModel2.ResetImage();
        this.ImageViewModel3.ResetImage();
        this.ImageViewModel4.ResetImage();
        this.ImageViewModel5.ResetImage();
        this.ImageViewModel6.ResetImage();
    }
}

// EOF