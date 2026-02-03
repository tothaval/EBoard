// <copyright file="RadialBrushSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.BrushSetup.RadialBrushSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.BrushSetup.SolidBrushSetup;
using EBoardSDK.Enums;
using EBoardSDK.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

public partial class RadialBrushSetupViewModel : ObservableObject, IDisposable
{
    private readonly BrushSetupViewModel brushSetupViewModel;
    private readonly FluidUIBaseViewModel viewModel;
    private readonly BrushTargets brushTargets;

    [ObservableProperty]
    private RadialGradientBrush brush = new();

    [ObservableProperty]
    private Point centerPoint = new Point(0.5, 0.5);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GradientStopsExist))]
    private ObservableCollection<GradientStop> gradientStops = new();

    [ObservableProperty]
    private GradientStop selectedGradientStop = new();

    [ObservableProperty]
    private SolidBrushSetupViewModel solidBrush;

    [ObservableProperty]
    private Point originPoint = new Point(0.0, 0.0);

    /// <summary>
    /// Initializes a new instance of the <see cref="RadialBrushSetupViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="brushTargets"></param>
    /// <param name="brushSetupViewModel"></param>
    public RadialBrushSetupViewModel(FluidUIBaseViewModel viewModel, BrushTargets brushTargets, BrushSetupViewModel brushSetupViewModel)
    {
        this.viewModel = viewModel;
        this.brushTargets = brushTargets;
        this.brushSetupViewModel = brushSetupViewModel;

        this.ApplyViewModelDataOrDefault();
    }

    public bool GradientStopsExist => this.GradientStops.Count > 0;

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    partial void OnSelectedGradientStopChanged(GradientStop value)
    {
        this.SetupSolidBrushViewModel();
    }

    private void ApplyViewModelDataOrDefault()
    {
        if (this.ViewModel.FluidUI.Design == null)
        {
            return;
        }

        switch (this.brushTargets)
        {
            case BrushTargets.Background:

                if (this.ViewModel.FluidUI.Design.BackgroundColor.BrushTypeEnum == BrushTypes.RadialGradientBrush)
                {
                    this.GradientStops.Clear();

                    var brush = this.ViewModel.FluidUI.Design.Background as RadialGradientBrush;

                    if (brush != null)
                    {
                        this.CenterPoint = brush.Center;
                        this.OriginPoint = brush.GradientOrigin;

                        foreach (var item in brush.GradientStops)
                        {
                            var color = Color.FromArgb(item.Color.A, item.Color.R, item.Color.G, item.Color.B);
                            var offset = item.Offset;

                            this.GradientStops.Add(new GradientStop(color, offset));
                        }
                    }
                }

                break;
            case BrushTargets.Border:
                break;
            case BrushTargets.Foreground:
                break;
            case BrushTargets.Highlight:
                break;
            default:
                this.AddGradientPoint();
                break;
        }

        if (this.GradientStops.Count > 0)
        {
            this.SelectedGradientStop = this.GradientStops.First();
        }

        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    private void BuildRadialGradientBrush()
    {
        var gradientStops = new GradientStopCollection();

        foreach (var item in this.GradientStops)
        {
            gradientStops.Add(new GradientStop(item.Color, item.Offset));
        }

        var brush = new RadialGradientBrush();
        brush.GradientStops = gradientStops;
        brush.Center = this.CenterPoint;
        brush.GradientOrigin = this.OriginPoint;

        this.Brush = brush;

        this.OnPropertyChanged(nameof(this.GradientStopsExist));
        this.OnPropertyChanged(nameof(this.GradientStops));
        this.OnPropertyChanged(nameof(this.Brush));
    }

    private void SetupSolidBrushViewModel()
    {
        this.SolidBrush = new SolidBrushSetupViewModel(this.ViewModel, this.brushTargets, this.SetSolidBrushToSelectedGradient);

        if (this.SelectedGradientStop != null)
        {
            this.SolidBrush.SetColorValue(this.SelectedGradientStop.Color);
        }

        this.OnPropertyChanged(nameof(this.SolidBrush));
    }

    partial void OnCenterPointChanged(Point value)
    {
        this.BuildRadialGradientBrush();
    }

    partial void OnGradientStopsChanged(ObservableCollection<GradientStop> value)
    {
        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    partial void OnOriginPointChanged(Point value)
    {
        this.BuildRadialGradientBrush();
    }

    [RelayCommand]
    private void AddGradientPoint()
    {
        this.GradientStops.Add(new GradientStop() { Color = Colors.White, Offset = 0.0 });

        this.SelectedGradientStop = this.GradientStops.Last();

        this.OnPropertyChanged(nameof(this.GradientStops));
        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    [RelayCommand]
    private void ApplyRadialGradientBrush()
    {
        if (this.GradientStops.Count > 0)
        {
            this.brushSetupViewModel.SetBrush(this.Brush);
        }
    }

    [RelayCommand]
    private void ClearGradientPoints()
    {
        this.GradientStops.Clear();

        this.OnPropertyChanged(nameof(this.GradientStops));
        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    [RelayCommand]
    private void RemoveGradientPoint()
    {
        var index = this.GradientStops.IndexOf(this.SelectedGradientStop);

        this.GradientStops.RemoveAt(index);

        if (this.GradientStops.Count > 0)
        {
            this.SelectedGradientStop = this.GradientStops.Last();
        }

        this.OnPropertyChanged(nameof(this.GradientStops));
        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    [RelayCommand]
    private void RestoreRadialGradientBrush()
    {
        this.ApplyViewModelDataOrDefault();
    }

    [RelayCommand]
    private void SetSolidBrushToSelectedGradient()
    {
        if (this.SelectedGradientStop == null)
        {
            return;
        }

        var index = this.GradientStops.IndexOf(this.SelectedGradientStop);
        var offset = this.SelectedGradientStop.Offset;

        var color = Color.FromArgb(this.SolidBrush.AlphaValue, this.SolidBrush.RedValue, this.SolidBrush.GreenValue, this.SolidBrush.BlueValue);
        var gradientStop = new GradientStop(color, offset);

        this.GradientStops.RemoveAt(index);

        if (this.GradientStops.Count < index)
        {
            this.GradientStops.Add(gradientStop);
        }
        else
        {
            this.GradientStops.Insert(index, gradientStop);
        }

        this.BuildRadialGradientBrush();
    }
}

// EOF