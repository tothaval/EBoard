// <copyright file="TextShapeViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Shapes.TextShape;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class TextShapeViewModel : ShapeBaseViewModel
{
    private string pluginHeader = "Text Shape";
    private string pluginName = "TextShape";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ResetText))]
    private bool textEntered = false;

    [ObservableProperty]
    private string textString = "enter text";

    [ObservableProperty]
    private double scaleX = 1.0;

    [ObservableProperty]
    private double scaleY = 1.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeViewModel"/> class.
    /// </summary>
    public TextShapeViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeViewModel"/> class.
    /// </summary>
    /// <param name="text"></param>
    public TextShapeViewModel(string text)
    {
        this.TextString = text;
        this.TextEntered = true;
    }

    public bool ResetText => !this.TextEntered;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Shape;

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
    public override Type? PluginModelType => typeof(TextShapeModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(TextShapeViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<TextShapeModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                this.RefreshInitialization();

                this.OnPropertyChanged(nameof(this.TextEntered));
                this.OnPropertyChanged(nameof(this.TextString));
                this.OnPropertyChanged(nameof(this.ScaleX));
                this.OnPropertyChanged(nameof(this.ScaleY));

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(TextShapeModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new TextShapeModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is TextShapeModel textShapeModel)
        {
            this.ApplyModel(textShapeModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<TextShapeModel>(json!);

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
        var model = new TextShapeModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(TextShapeModel textShapeModel)
    {
        var fluidUIViewModel = new FluidUIBaseViewModel();
        fluidUIViewModel.SetFluidUI(textShapeModel.FluidUIContext);

        this.SetFluidUIViewModel(fluidUIViewModel);

        this.TextString = textShapeModel.TextString;
        this.ScaleX = textShapeModel.ScaleX;
        this.ScaleY = textShapeModel.ScaleY;
        this.TextEntered = textShapeModel.TextEntered;
    }

    [RelayCommand]
    private void CreateTextShape()
    {
        this.TextEntered = true;
    }

    [RelayCommand]
    private void EditText()
    {
        this.TextEntered = false;
    }
}

// EOF