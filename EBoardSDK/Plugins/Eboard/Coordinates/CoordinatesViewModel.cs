// <copyright file="CoordinatesViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.Coordinates;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public partial class CoordinatesViewModel : PluginBaseViewModel, IDisposable
{
    private readonly string pluginHeader = "Coordinates Element";
    private readonly string pluginName = "Coordinates";

    [ObservableProperty]
    private bool isRotated;

    [ObservableProperty]
    private int xCoord;

    [ObservableProperty]
    private int xmouse;

    [ObservableProperty]
    private int yCoord;

    [ObservableProperty]
    private int ymouse;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AxisLengthHalf))]
    private int axisLength = 40;

    /// <summary>
    /// Initializes a new instance of the <see cref="CoordinatesViewModel"/> class.
    /// </summary>
    public CoordinatesViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.SetMenuItemViewModel(new CoordinatesMenuItemViewModel(this));
        this.SetMenuItem(new CoordinatesMenuItem(this.MenuItemViewModel!));

        this.OnPropertyChanged(nameof(this.AxisLengthHalf));
    }

    public Point Point => new Point(this.XCoord, this.YCoord);

    public string CoordinatesWrongMessage => "WARNING: rotation currently not supported.\ndisplayed coordinates are wrong.";

    public int AxisLengthHalf => this.AxisLength / 2;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Eboard;

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
    public override Type? PluginModelType => typeof(CoordinatesModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(CoordinatesViewModel);

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();

        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            this.ElementViewModel.ElementView.ElementBorder.MouseMove -= this.ElementBorder_MouseMove;
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<CoordinatesModel>(path);

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
        var model = new CoordinatesModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is CoordinatesModel coordinatesModel)
        {
            this.ApplyModel(coordinatesModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<CoordinatesModel>(json!);

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
        var model = new CoordinatesModel(this);

        this.SetModel(model);
    }

    /// <inheritdoc/>
    public override void ViewWasSet()
    {
        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            this.ElementViewModel.ElementView.ElementBorder.MouseMove += this.ElementBorder_MouseMove;
        }

        this.ChangeXYCoord();
    }

    internal void ChangeXYCoord()
    {
        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            this.XCoord = (int)Canvas.GetLeft(this.ElementViewModel.ElementView.VisualParent) + (int)this.GetDeltaLeft();
            this.YCoord = (int)Canvas.GetTop(this.ElementViewModel.ElementView.VisualParent) + (int)this.GetDeltaTop();
        }
    }

    internal void ChangeMouseCoords(Point p)
    {
        int x = (int)p.X;
        int y = (int)p.Y;

        if (x != this.Xmouse)
        {
            this.Xmouse = x;
        }

        if (y != this.Ymouse)
        {
            this.Ymouse = y;
        }
    }

    private void ApplyModel(CoordinatesModel coordinatesModel)
    {
        this.AxisLength = coordinatesModel.AxisLength;
    }

    private double GetDeltaLeft()
    {
        if (this.ElementViewModel != null && this.ElementViewModel.FluidUI.Size != null)
        {
            return this.AxisLengthHalf + this.ElementViewModel.FluidUI.Size.Margin.Left + this.ElementViewModel.FluidUI.Size.Padding.Left + this.ElementViewModel.FluidUI.Size.BorderThickness.Left;
        }

        return this.AxisLengthHalf;
    }

    private double GetDeltaTop()
    {
        if (this.ElementViewModel != null && this.ElementViewModel.FluidUI.Size != null)
        {
            return this.AxisLengthHalf + this.ElementViewModel.FluidUI.Size.Margin.Top + this.ElementViewModel.FluidUI.Size.Padding.Top + this.ElementViewModel.FluidUI.Size.BorderThickness.Top;
        }

        return this.AxisLengthHalf;
    }

    private void ElementBorder_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.ChangeXYCoord();
        }

        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            this.ChangeMouseCoords(e.GetPosition(this.ElementViewModel.ElementView.Canvas));

            var manager = new FluidUIStandManager(this.ElementViewModel);

            this.IsRotated = manager.GetAngle() != 0;
        }

        e.Handled = true;
    }

    partial void OnXCoordChanged(int value)
    {
        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            var x = (int)Canvas.GetLeft(this.ElementViewModel.ElementView.VisualParent) + (int)this.GetDeltaLeft();

            if (x != value)
            {
                var manager = new FluidUIStandManager(this.ElementViewModel);

                manager.SetX(value - this.GetDeltaLeft());
            }
        }
    }

    partial void OnYCoordChanged(int value)
    {
        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            var y = (int)Canvas.GetTop(this.ElementViewModel.ElementView.VisualParent) + (int)this.GetDeltaTop();

            if (y != value)
            {
                var manager = new FluidUIStandManager(this.ElementViewModel);

                manager.SetY(value - this.GetDeltaTop());
            }
        }
    }
}

// EOF