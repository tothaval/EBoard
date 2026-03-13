// <copyright file="PolygonViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Shapes.Polygon;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class PolygonViewModel : ShapeBaseViewModel
{
    private readonly string pluginHeader = "Polygon Shape";
    private readonly string pluginName = "Polygon";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ResetPolygon))]
    private bool polygonReady = false;

    [ObservableProperty]
    private Point newPoint = default(Point);

    [ObservableProperty]
    private Point selectedPoint = default(Point);

    [ObservableProperty]
    private PointCollection polygonPointCollection = new PointCollection();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BelowMinimumPointCount))]
    private ObservableCollection<Point> points = new ObservableCollection<Point>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonViewModel"/> class.
    /// </summary>
    public PolygonViewModel()
    {
        //this.SetMenuItemViewModel(new PolygonMenuItemViewModel(this));
        //this.SetMenuItem(new PolygonMenuItem(this.MenuItemViewModel!));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonViewModel"/> class.
    /// </summary>
    /// <param name="points"></param>
    public PolygonViewModel(ObservableCollection<Point>? points)
    {
        if (points != null)
        {
            this.Points = points;

            this.CreatePolygonPointCollection(points);
        }
    }

    public bool BelowMinimumPointCount => this.Points.Count < 3;

    public bool ResetPolygon => !this.PolygonReady;

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
    public override Type? PluginModelType => typeof(PolygonModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(PolygonViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<PolygonModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                this.RefreshInitialization();

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(PolygonModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new PolygonModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is PolygonModel polygonModel)
        {
            this.ApplyModel(polygonModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<PolygonModel>(json!);

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
        var model = new PolygonModel(this);

        this.SetModel(model);
    }

    /// <inheritdoc/>
    public override void Duplicate()
    {
        if (this.ElementViewModel != null)
        {
            var screen = this.ElementViewModel.ScreenViewModel;

            var manager = new SDKPluginManager();
            var copy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.FluidUI);

            var polygon = new PolygonViewModel(this.Points);

            this.SetFluidUI(copy);

            manager.InvokePluginNTimes(polygon, this.ElementViewModel.DuplicationCount, screen);
        }
    }

    private void ApplyModel(PolygonModel polygonModel)
    {
        if (polygonModel.Points != null)
        {
            this.Points = polygonModel.Points;
        }

        this.PolygonReady = polygonModel.PolygonReady;

        this.CreatePolygonPointCollection(this.Points);

        this.ApplyShapeModel(polygonModel);
    }

    private void CreatePolygonPointCollection(ObservableCollection<Point>? points)
    {
        if (points != null && points.Count > 2)
        {
            this.PolygonPointCollection = new PointCollection();

            foreach (var item in points)
            {
                this.PolygonPointCollection.Add(new Point(item.X, item.Y));
            }

            this.PolygonReady = true;
        }
    }

    partial void OnSelectedPointChanged(Point value)
    {
        this.NewPoint = value;
    }

    [RelayCommand]
    private void AddPoint()
    {
        if (!this.Points.Contains(this.NewPoint))
        {
            this.Points.Add(new Point(this.NewPoint.X, this.NewPoint.Y));

            this.NewPoint = default(Point);
        }
    }

    [RelayCommand]
    private void ClearPoints()
    {
        this.Points.Clear();
    }

    [RelayCommand]
    private void Create()
    {
        this.CreatePolygonPointCollection(this.Points);
    }

    [RelayCommand]
    private void DeletePoint()
    {
        this.Points.Remove(this.SelectedPoint);

        this.NewPoint = default(Point);
    }

    [RelayCommand]
    private void Edit()
    {
        this.PolygonPointCollection.Clear();
        this.PolygonReady = false;
    }

    [RelayCommand]
    private void EditPoint()
    {
        this.SelectedPoint = new Point(this.NewPoint.X, this.NewPoint.Y);

        this.NewPoint = default(Point);
    }
}

// EOF