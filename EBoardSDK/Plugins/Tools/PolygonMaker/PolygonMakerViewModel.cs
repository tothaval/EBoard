// <copyright file="PolygonMakerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Tools.PolygonMaker;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Eboard.Coordinates;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Shapes.Polygon;
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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public partial class PolygonMakerViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Polygon Maker";
    private readonly string pluginName = "PolygonMaker";

    private object dropItem;

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

    [ObservableProperty]
    private int panelWidth = 250;

    [ObservableProperty]
    private int panelHeight = 250;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonMakerViewModel"/> class.
    /// </summary>
    public PolygonMakerViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public bool BelowMinimumPointCount => this.Points.Count < 3;

    public bool ResetPolygon => !this.PolygonReady;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Tool;

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(PolygonMakerModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(PolygonMakerViewModel);

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<PolygonMakerModel>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(LinkModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new PolygonMakerModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is PolygonMakerModel polygonMakerModel)
        {
            this.ApplyModel(polygonMakerModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<PolygonMakerModel>(json!);

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
        var model = new PolygonMakerModel(this);

        this.SetModel(model);
    }

    internal void AddDropItemToPointsCollection(object? item)
    {
        var itemstring = item as string;

        if (itemstring != null)
        {
            var point = Point.Parse(itemstring);

            if (!this.Points.Contains(point))
            {
                this.Points.Add(point);
            }
        }
    }

    internal void InsertDropItemToPointsCollection(object? item, object? target)
    {
        if (item == target || item == null || target == null)
        {
            return;
        }

        try
        {
            var itempoint = (Point)item;
            var targetpoint = (Point)target;

            int oldIndex = this.Points.IndexOf(itempoint);
            int nextIndex = this.Points.IndexOf(targetpoint);

            if (oldIndex != -1 && nextIndex != -1)
            {
                this.Points.Move(oldIndex, nextIndex);

                this.CreatePolygonPointCollection(this.Points);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            throw;
        }
    }

    internal void RemoveDropItemFromPointsCollection(object? item)
    {
        var itemstring = item as string;

        if (itemstring != null)
        {
            var point = Point.Parse(itemstring);

            if (this.Points.Contains(point))
            {
                this.Points.Remove(point);
            }
        }
    }

    private void ApplyModel(PolygonMakerModel polygonMakerModel)
    {
        this.PanelHeight = polygonMakerModel.PanelHeight;
        this.PanelWidth = polygonMakerModel.PanelWidth;
        this.PolygonReady = polygonMakerModel.PolygonReady;

        if (polygonMakerModel.Points.Count > 0)
        {
            foreach (var item in polygonMakerModel.Points)
            {
                this.Points.Add(item);
            }
        }

        this.CreatePolygonPointCollection(this.Points);
    }

    private void CreatePolygonPointCollection(ObservableCollection<Point>? points)
    {
        if (points == null)
        {
            return;
        }

        this.PolygonPointCollection = new PointCollection();

        foreach (var item in points)
        {
            this.PolygonPointCollection.Add(new Point(item.X, item.Y));
        }

        if (points.Count > 2)
        {
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

            this.CreatePolygonPointCollection(this.Points);

            this.OnPropertyChanged(nameof(this.BelowMinimumPointCount));
        }
    }

    [RelayCommand]
    private void ClearPoints()
    {
        this.Points.Clear();

        this.CreatePolygonPointCollection(this.Points);

        this.OnPropertyChanged(nameof(this.BelowMinimumPointCount));
        this.OnPropertyChanged(nameof(this.PolygonPointCollection));
    }

    [RelayCommand]
    private void Create()
    {
        if (this.ElementViewModel == null)
        {
            return;
        }

        this.CreatePolygonPointCollection(this.Points);
        var pointListCopy = new ObservableCollection<Point>();

        foreach (var item in this.Points)
        {
            pointListCopy.Add(new Point(item.X, item.Y));
        }

        var manager = new SDKPluginManager();

        var copy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.FluidUI);

        var polygon = new PolygonViewModel(pointListCopy);

        var vm = new FluidUIBaseViewModel();
        vm.SetFluidUI(copy);

        polygon.SetFluidUIViewModel(vm);

        manager.InvokePluginOnEboard(polygon, this.ElementViewModel.ScreenViewModel);
    }

    [RelayCommand]
    private void CreateCoordinates()
    {
        if (this.ElementViewModel != null)
        {
            var manager = new SDKPluginManager();

            foreach (var point in this.Points)
            {
                manager.InvokePluginOnEboard(new CoordinatesViewModel(), this.ElementViewModel.ScreenViewModel, new Point(point.X, point.Y));
            }
        }
    }

    [RelayCommand]
    private void DeletePoint()
    {
        this.Points.Remove(this.SelectedPoint);

        this.NewPoint = default(Point);

        this.CreatePolygonPointCollection(this.Points);

        this.OnPropertyChanged(nameof(this.BelowMinimumPointCount));
    }

    [RelayCommand]
    private void Edit()
    {
        this.PolygonPointCollection.Clear();
        this.PolygonReady = false;

        this.CreatePolygonPointCollection(this.Points);
    }

    [RelayCommand]
    private void EditPoint()
    {
        this.SelectedPoint = new Point(this.NewPoint.X, this.NewPoint.Y);

        this.NewPoint = default(Point);

        this.CreatePolygonPointCollection(this.Points);
    }

    [RelayCommand]
    private void NewMousePoint(object? parameter)
    {
        if (parameter != null)
        {
            if (parameter.GetType().Equals(typeof(Border)))
            {
                var border = (Border)parameter;

                if (border != null)
                {
                    var pos = Mouse.GetPosition(border);

                    this.NewPoint = pos;
                    this.AddPoint();
                }
            }
        }
    }

    [RelayCommand]
    private void UseCoordinatesPositions()
    {
        if (this.ElementViewModel != null)
        {
            var coordinates = this.ElementViewModel.ScreenViewModel.GetCoordinates();

            foreach (var item in coordinates)
            {
                var point = item.Point;

                if (!this.Points.Contains(point))
                {
                    this.Points.Add(new Point(point.X, point.Y));
                }
            }

            this.CreatePolygonPointCollection(this.Points);
        }
    }
}

// EOF