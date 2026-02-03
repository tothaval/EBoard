// <copyright file="LinearBrushSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.BrushSetup.LinearBrushSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.BrushSetup.SolidBrushSetup;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

public partial class LinearBrushSetupViewModel : ObservableObject, IDisposable
{
    private readonly BrushSetupViewModel brushSetupViewModel;
    private readonly FluidUIBaseViewModel viewModel;
    private readonly BrushTargets brushTargets;

    [ObservableProperty]
    private LinearGradientBrush brush = new();

    [ObservableProperty]
    private Point endPoint = new Point(1.0, 0.5);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GradientStopsExist))]
    private ObservableCollection<GradientStop> gradientStops = new();

    [ObservableProperty]
    private GradientStop selectedGradientStop;

    [ObservableProperty]
    private SolidBrushSetupViewModel solidBrush;

    [ObservableProperty]
    private Point startPoint = new Point(0.0, 0.5);

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearBrushSetupViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="brushTargets"></param>
    /// <param name="brushSetupViewModel"></param>
    public LinearBrushSetupViewModel(FluidUIBaseViewModel viewModel, BrushTargets brushTargets, BrushSetupViewModel brushSetupViewModel)
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

    private void ApplyViewModelDataOrDefault()
    {
        if (this.ViewModel.FluidUI.Design == null)
        {
            return;
        }

        switch (this.brushTargets)
        {
            case BrushTargets.Background:

                if (this.ViewModel.FluidUI.Design.BackgroundColor.BrushTypeEnum == BrushTypes.LinearGradientBrush)
                {
                    this.GradientStops.Clear();

                    var brush = this.ViewModel.FluidUI.Design.Background as LinearGradientBrush;

                    if (brush != null)
                    {
                        this.StartPoint = brush.StartPoint;
                        this.EndPoint = brush.EndPoint;

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

    private void BuildLinearGradientBrush()
    {
        var gradientStops = new GradientStopCollection();

        foreach (var item in this.GradientStops)
        {
            gradientStops.Add(new GradientStop(item.Color, item.Offset));
        }

        var brush = new LinearGradientBrush();
        brush.GradientStops = gradientStops;
        brush.StartPoint = this.StartPoint;
        brush.EndPoint = this.EndPoint;

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

    partial void OnEndPointChanged(Point value)
    {
        this.BuildLinearGradientBrush();
    }

    partial void OnGradientStopsChanged(ObservableCollection<GradientStop> value)
    {
        this.OnPropertyChanged(nameof(this.GradientStopsExist));
    }

    partial void OnSelectedGradientStopChanged(GradientStop value)
    {
        this.SetupSolidBrushViewModel();
    }

    partial void OnStartPointChanged(Point value)
    {
        this.BuildLinearGradientBrush();
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
    private void ApplyLinearGradientBrush()
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
    private void RestoreLinearGradientBrush()
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

        this.BuildLinearGradientBrush();
    }
}

// EOF