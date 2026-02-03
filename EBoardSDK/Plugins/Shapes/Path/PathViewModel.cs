// <copyright file="PathViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Shapes.Path;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class PathViewModel : ShapeBaseViewModel
{
    private string pluginHeader = "Path Shape";

    private string pluginName = "Path";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ResetPath))]
    private bool pathEntered = false;

    [ObservableProperty]
    private Geometry pathGeometry = Geometry.Empty;

    [ObservableProperty]
    private string pathString = "enter path geometry";

    /// <summary>
    /// Initializes a new instance of the <see cref="PathViewModel"/> class.
    /// </summary>
    public PathViewModel()
    {
    }

    public bool ResetPath => !this.PathEntered;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Shape;

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
    public override Type? PluginModelType => typeof(PathModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(PathViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<PathModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                this.RefreshInitialization();

                this.OnPropertyChanged(nameof(this.PathEntered));
                this.OnPropertyChanged(nameof(this.PathGeometry));
                this.OnPropertyChanged(nameof(this.PathString));

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(PathModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new PathModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is PathModel pathModel)
        {
            this.ApplyModel(pathModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<PathModel>(json!);

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
        var model = new PathModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(PathModel pathModel)
    {
        var fluidUIViewModel = new FluidUIBaseViewModel();
        fluidUIViewModel.SetFluidUI(pathModel.FluidUIContext);

        this.PathString = pathModel.PathString;
        this.PathEntered = pathModel.PathEntered;

        this.SetFluidUIViewModel(fluidUIViewModel);
    }

    partial void OnPathStringChanged(string value)
    {
        Geometry geometry = Geometry.Empty;

        try
        {
            geometry = Geometry.Parse(value);
        }
        catch (Exception ex)
        {
            var s = $"parsing geometry string >>>{value}<<< failed\n{ex}";

            Log.Error(s);
        }

        if (!geometry.IsEmpty())
        {
            this.PathGeometry = geometry;
            this.PathEntered = true;
        }
    }

    [RelayCommand]
    private void EditPath()
    {
        this.PathGeometry = Geometry.Empty;
        this.PathEntered = false;
    }
}

// EOF