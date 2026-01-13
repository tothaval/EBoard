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
namespace EBoardSDK.Plugins.Tools.Coordinates;
using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models.FluidUIStand;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

public partial class CoordinatesViewModel : EBoardElementPluginBaseViewModel, IDisposable
{
    private string pluginHeader = "Coordinates Element";
    private string pluginName = "Coordinates";

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
        this.OnPropertyChanged(nameof(this.AxisLengthHalf));
    }

    public string CoordinatesWrongMessage => "WARNING: rotation currently not supported.\ndisplayed coordinates are wrong.";

    public int AxisLengthHalf => this.AxisLength / 2;

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(CoordinatesView);

    public override Type ElementPluginViewModel => typeof(CoordinatesViewModel);

    public override void Dispose()
    {
        base.Dispose();

        if (this.ElementViewModel != null && this.ElementViewModel.ElementView != null)
        {
            this.ElementViewModel.ElementView.ElementBorder.MouseMove -= this.ElementBorder_MouseMove;
        }
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

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