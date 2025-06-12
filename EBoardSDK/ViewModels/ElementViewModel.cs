// <copyright file="ElementViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  ElementViewModel
 *
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Views;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ElementViewModel : ObservableObject, IElementSelection, IElementBackgroundImage, IDisposable
{
    [ObservableProperty]
    private SolidColorBrush backgroundColorBrush;

    [ObservableProperty]
    private SolidColorBrush borderColorBrush = new SolidColorBrush(Colors.Black);

    private EBoardViewModel eBoardViewModel;

    private string eID;

    private ElementView elementView;

    [ObservableProperty]
    private SolidColorBrush foregroundColorBrush;

    [ObservableProperty]
    private int heightValue;

    [ObservableProperty]
    private SolidColorBrush highlightColorBrush = new SolidColorBrush(Colors.Orange);

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private bool isSelected = false;

    [ObservableProperty]
    private PlacementManagement placementManager;

    [ObservableProperty]
    private int rotationAngleValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlacementManager))]
    private RotateTransform rotateTransformValue;

    [ObservableProperty]
    private Point transformOriginPoint;

    [ObservableProperty]
    private int widthValue;

    [ObservableProperty]
    private int xMaximumValue;

    [ObservableProperty]
    private double xPosition;

    [ObservableProperty]
    private int xSliderValue;

    [ObservableProperty]
    private int yMaximumValue;

    [ObservableProperty]
    private double yPosition;

    [ObservableProperty]
    private int ySliderValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlacementManager))]
    private int zIndexValue = 0;

    [ObservableProperty]
    private int zMaximumValue;

    [ObservableProperty]
    private int zMinimumValue;

    public ElementViewModel()
    {
    }

    public ElementViewModel(EBoardViewModel eBoardViewModel, IElementDataSet elementDataSet) => this.ApplyData(eBoardViewModel, elementDataSet);

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public Brush BBackground
    {
        get
        {
            if (this.Plugin == null || this.Plugin!.NoDefaultBorders)
            {
                return new SolidColorBrush(Colors.Transparent);
            }

            return this.Plugin.BrushManagement.Background;
        }

        set
        {
            if (this.Plugin != null)
            {
                if (!this.Plugin.NoDefaultBorders)
                {
                    this.Plugin.BrushManagement.Background = value;

                    this.Plugin.ApplyBrush(value, BrushTargets.Background);
                }
            }

            this.OnPropertyChanged(nameof(this.BBackground));
            this.OnPropertyChanged(nameof(this.Plugin.BrushManagement.Background));
            this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
            this.OnPropertyChanged(nameof(this.Plugin));
            this.OnPropertyChanged();
        }
    }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public string EID => this.eID;

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    public ElementView ElementView => this.elementView;

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public IPlugin Plugin { get; set; }

    public bool? PluginNoDefaultBordersSet => !this.Plugin?.NoDefaultBorders;

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public void ApplyBackgroundBrush(Brush brush)
    {
        this.Plugin.ApplyBrush(brush, BrushTargets.Background);
    }

    public void Apply_CornerRadiusValue(QuadValue<int> cornerRadius)
    {
        // in order to apply the value onto every selected element without triggering the value change
        // and ChangeSelection_VALUE everytime in every element insance, use the ApplyVALUE method
        this.CornerRadiusQuadSetup.TopLeft = cornerRadius.Value1;
        this.CornerRadiusQuadSetup.TopRight = cornerRadius.Value2;
        this.CornerRadiusQuadSetup.BottomRight = cornerRadius.Value3;
        this.CornerRadiusQuadSetup.BottomLeft = cornerRadius.Value4;

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    public void ApplyData(EBoardViewModel eBoardViewModel, IElementDataSet elementDataSet)
    {
        this.PlacementManager = new PlacementManagement();

        this.eBoardViewModel = eBoardViewModel;

        this.eID = elementDataSet.EID;

        if (elementDataSet.PlacementDataSet != null)
        {
            this.PlacementManager = new PlacementManagement(elementDataSet.PlacementDataSet);

            this.RotationAngleValue = (int)elementDataSet.PlacementDataSet.Angle;

            this.XPosition = this.PlacementManager.Position.X;
            this.YPosition = this.PlacementManager.Position.Y;
            this.XMaximumValue = eBoardViewModel.Width;
            this.YMaximumValue = eBoardViewModel.Height;

            this.ZIndexValue = this.PlacementManager.Z;
        }

        if (this.eID == null || this.eID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eID = $"Element_{dateTime.Ticks}";
        }

        if (elementDataSet.Plugin != null)
        {
            this.Plugin = elementDataSet.Plugin;

            this.SetElementSizeDisplayValue();

            var bordermanagment = new BorderManagement(elementDataSet.BorderDataSet);
            this.Plugin.BorderManagement = bordermanagment;

            var brushmanagement = new BrushManagement(elementDataSet.BrushDataSet);
            this.Plugin.BrushManagement = brushmanagement;

            this.Plugin.ApplyBrush(brushmanagement.Background, BrushTargets.Background);
            this.Plugin.ApplyBrush(brushmanagement.Foreground, BrushTargets.Foreground);
            this.Plugin.ApplyBrush(brushmanagement.Border, BrushTargets.Border);
            this.Plugin.ApplyBrush(brushmanagement.Highlight, BrushTargets.Highlight);

            var helper = new SharedMethod_UI();

            if (!this.Plugin.NoDefaultBorders)
            {
                this.CornerRadiusQuadSetup = helper.BuildQuadValueSetup(
                    new QuadValue<int>()
                    {
                        Value1 = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft,
                        Value2 = (int)elementDataSet.BorderDataSet.CornerRadius.TopRight,
                        Value3 = (int)elementDataSet.BorderDataSet.CornerRadius.BottomRight,
                        Value4 = (int)elementDataSet.BorderDataSet.CornerRadius.BottomLeft,
                    },
                    this.ResetCorners,
                    this.Plugin.BrushManagement);

                this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged;

                this.PaddingQuadSetup = helper.BuildQuadValueSetup(
                    new QuadValue<int>()
                    {
                        Value1 = (int)elementDataSet.BorderDataSet.Padding.Left,
                        Value2 = (int)elementDataSet.BorderDataSet.Padding.Top,
                        Value3 = (int)elementDataSet.BorderDataSet.Padding.Right,
                        Value4 = (int)elementDataSet.BorderDataSet.Padding.Bottom,
                    },
                    this.ResetPadding,
                    this.Plugin.BrushManagement);

                this.PaddingQuadSetup.PropertyChanged += this.PaddingQuadSetup_PropertyChanged;
            }

            this.MarginQuadSetup = helper.BuildQuadValueSetup(
                new QuadValue<int>()
                {
                    Value1 = (int)elementDataSet.BorderDataSet.Margin.Left,
                    Value2 = (int)elementDataSet.BorderDataSet.Margin.Top,
                    Value3 = (int)elementDataSet.BorderDataSet.Margin.Right,
                    Value4 = (int)elementDataSet.BorderDataSet.Margin.Bottom,
                },
                this.ResetThickness,
                this.Plugin.BrushManagement);

            this.MarginQuadSetup.PropertyChanged += this.MarginQuadSetup_PropertyChanged;

            this.ThicknessQuadSetup = helper.BuildQuadValueSetup(
                new QuadValue<int>()
                {
                    Value1 = (int)elementDataSet.BorderDataSet.BorderThickness.Left,
                    Value2 = (int)elementDataSet.BorderDataSet.BorderThickness.Top,
                    Value3 = (int)elementDataSet.BorderDataSet.BorderThickness.Right,
                    Value4 = (int)elementDataSet.BorderDataSet.BorderThickness.Bottom,
                },
                this.ResetMargin,
                this.Plugin.BrushManagement);

            this.ThicknessQuadSetup.PropertyChanged += this.ThicknessQuadSetup_PropertyChanged;

            this.BackgroundBrushSetup = helper.BuildSolidColorBrushSetup(this.Plugin.BrushManagement, BrushTargets.Background, this.SetColorValueAsBackground);
            this.ForegroundBrushSetup = helper.BuildSolidColorBrushSetup(this.Plugin.BrushManagement, BrushTargets.Foreground, this.SetColorValueAsForeground);
            this.BorderBrushSetup = helper.BuildSolidColorBrushSetup(this.Plugin.BrushManagement, BrushTargets.Border, this.SetColorValueAsBorder);
            this.HighlightBrushSetup = helper.BuildSolidColorBrushSetup(this.Plugin.BrushManagement, BrushTargets.Highlight, this.SetColorValueAsHighlight);

            this.OnPropertyChanged(nameof(this.Plugin));

            this.CalibrateZSliderValues(this.eBoardViewModel.EBoardDepth);
        }

        this.ApplyRotationAngleValue(this.RotationAngleValue);

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    public void Apply_HeightValue(int heightValue)
    {
        this.HeightValue = heightValue;
    }

    public int ApplyRotationAngleValue(int rotationAngleValue)
    {
        this.UpdateRotation(rotationAngleValue);

        this.RotationAngleValue = rotationAngleValue;

        this.OnPropertyChanged(nameof(this.RotationAngleValue));

        return this.RotationAngleValue;
    }

    public int ApplyRotationAngleValueByMouseWheel(int delta)
    {
        if (delta < 0 && this.RotationAngleValue > -180)
        {
            this.RotationAngleValue--;
        }

        if (delta > 0 && this.RotationAngleValue < 180)
        {
            this.RotationAngleValue++;
        }

        this.UpdateRotation(this.RotationAngleValue);

        this.OnPropertyChanged(nameof(this.RotationAngleValue));

        return this.RotationAngleValue;
    }

    public void ApplyWidthValue(int widthValue)
    {
        this.WidthValue = widthValue;
    }

    public void ApplyZIndexValue(int zIndexValue)
    {
        this.ZIndexValue = zIndexValue;

        this.PlacementManager.Z = zIndexValue;

        this.OnPropertyChanged(nameof(this.PlacementManager.Z));
    }

    public int ApplyZIndexValueByMouseWheel(int delta)
    {
        if (delta < 0 && this.ZMinimumValue < this.ZIndexValue)
        {
            this.ZIndexValue--;
        }

        if (delta > 0 && this.ZMaximumValue > this.ZIndexValue)
        {
            this.ZIndexValue++;
        }

        this.PlacementManager.Z = this.ZIndexValue;

        this.OnPropertyChanged(nameof(this.PlacementManager.Z));

        return this.ZIndexValue;
    }

    public void BeginMovement(ElementViewModel elementViewModel)
    {
        this.XPosition = this.ElementView.X;
        this.YPosition = this.ElementView.Y;

        this.PlacementManager.Position = new Point(this.XPosition, this.YPosition);
    }

    public void CalibrateZSliderValues(int eboardDepth)
    {
        if (eboardDepth >= 0)
        {
            this.ZMinimumValue = 0;
            this.ZMaximumValue = eboardDepth;

            if (eboardDepth == 0)
            {
                this.ZMaximumValue = 1;
            }
        }
        else if (eboardDepth < 0)
        {
            this.ZMinimumValue = eboardDepth;
            this.ZMaximumValue = 0;
        }

        this.OnPropertyChanged(nameof(this.ZMinimumValue));
        this.OnPropertyChanged(nameof(this.ZMaximumValue));

        this.OnPropertyChanged(nameof(this.XMaximumValue));
        this.OnPropertyChanged(nameof(this.YMaximumValue));
    }

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        if (this.Plugin == null)
        {
            return;
        }

        this.SetImageToBrushTarget(brushTargets, path);
    }

    public void Dispose()
    {
        this.CornerRadiusQuadSetup.PropertyChanged -= this.CornerRadiusQuadSetup_PropertyChanged;

        this.MarginQuadSetup.PropertyChanged -= this.MarginQuadSetup_PropertyChanged;

        this.PaddingQuadSetup.PropertyChanged -= this.PaddingQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup.PropertyChanged -= this.ThicknessQuadSetup_PropertyChanged;

        GC.SuppressFinalize(this);
    }

    public void MoveXY(ElementViewModel elementViewModel, Point deltaPosition)
    {
        if (this.elementView != null)
        {
            double x, y;

            x = this.XPosition - deltaPosition.X;
            y = this.YPosition - deltaPosition.Y;

            this.PlacementManager.Position = new Point(x, y);

            Canvas.SetLeft(this.ElementView.VisualParent, x);
            Canvas.SetTop(this.ElementView.VisualParent, y);
        }
    }

    public void Redraw()
    {
        this.Plugin.ApplyRedraw();

        this.OnPropertyChanged(nameof(this.Plugin));

        this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));

        this.OnPropertyChanged(nameof(this.Plugin.BorderManagement));
    }

    [RelayCommand]
    public void Select()
    {
        this.IsSelected = !this.IsSelected;

        this.SelectionChange(this.IsSelected);
    }

    public bool SelectionChange(bool isSelected)
    {
        if (isSelected)
        {
            this.Plugin.BrushManagement.SwitchBorderToHighlight();

            this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
            this.OnPropertyChanged(nameof(this.Plugin));

            return true;
        }

        this.Plugin.BrushManagement.SwitchBorderToBorder();

        this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
        this.OnPropertyChanged(nameof(this.Plugin));

        return false;
    }

    public void SetView(ElementView elementView)
    {
        this.elementView = elementView;
    }

    public void StopMovement()
    {
        this.XPosition = Canvas.GetLeft(this.elementView.VisualParent);
        this.YPosition = Canvas.GetTop(this.elementView.VisualParent);

        this.PlacementManager.Position = new Point(this.XPosition, this.YPosition);
    }

    public void WasLastActive()
    {
        this.eBoardViewModel?.MoveLastClickedElement(this);
    }

    private void ChangeSelection_CornerRadiusValue(QuadValue<int> cornerRadius)
    {
        this.eBoardViewModel?.ChangeSelection_CornerRadius(this, cornerRadius);
    }

    private void ChangeSelection_HeightValue(int heightValue)
    {
        this.eBoardViewModel?.ChangeSelection_Height(this, heightValue);
    }

    private void ChangeSelection_RotationAngleValue(int rotationAngleValue)
    {
        this.eBoardViewModel?.ChangeSelection_RotationAngle(this, rotationAngleValue);
    }

    private void ChangeSelection_WidthValue(int widthValue)
    {
        this.eBoardViewModel?.ChangeSelection_WidthValue(this, widthValue);
    }

    private void ChangeSelection_ZIndexValue(int zIndexValue)
    {
        this.eBoardViewModel.ChangeSelection_ZIndex(this, zIndexValue);
    }

    private void CornerRadiusQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Plugin.BorderManagement.CornerRadius = new CornerRadius(
            this.CornerRadiusQuadSetup.QuadValue.Value1,
            this.CornerRadiusQuadSetup.QuadValue.Value2,
            this.CornerRadiusQuadSetup.QuadValue.Value3,
            this.CornerRadiusQuadSetup.QuadValue.Value4);

        this.ChangeSelection_CornerRadiusValue(this.CornerRadiusQuadSetup.QuadValue);

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.eBoardViewModel?.RemoveElement(this);
    }

    private void MarginQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Plugin.BorderManagement.Margin = new Thickness(
            this.MarginQuadSetup.QuadValue.Value1,
            this.MarginQuadSetup.QuadValue.Value2,
            this.MarginQuadSetup.QuadValue.Value3,
            this.MarginQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    partial void OnBackgroundColorBrushChanged(SolidColorBrush value)
    {

    }

    partial void OnImageBorderPathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Border, value);
    }

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            ChangeSelection_RotationAngleValue(newValue);

            UpdateRotation(newValue);
        }
    }

    partial void OnRotateTransformValueChanged(RotateTransform value)
    {
        placementManager.Angle = RotationAngleValue;
    }

    partial void OnHeightValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Plugin is not null)
        {
            Plugin.BorderManagement.Height = value;
        }

        OnPropertyChanged(nameof(Plugin));

        ChangeSelection_HeightValue(value);
    }

    partial void OnWidthValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Plugin is not null)
        {
            Plugin.BorderManagement.Width = value;
        }

        OnPropertyChanged(nameof(Plugin));

        ChangeSelection_WidthValue(value);
    }

    partial void OnXPositionChanged(double value)
    {
        XSliderValue = (int)value;
    }

    partial void OnXSliderValueChanged(int value)
    {
        XPosition = value;
    }

    partial void OnYPositionChanged(double value)
    {
        YSliderValue = (int)value;
    }

    partial void OnYSliderValueChanged(int value)
    {
        YPosition = value;
    }

    partial void OnZIndexValueChanged(int value)
    {
        PlacementManager.Z = value;

        ChangeSelection_ZIndexValue(value);
    }

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Plugin.BorderManagement.Padding = new Thickness(
            this.PaddingQuadSetup.QuadValue.Value1,
            this.PaddingQuadSetup.QuadValue.Value2,
            this.PaddingQuadSetup.QuadValue.Value3,
            this.PaddingQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void ResetCorners()
    {
        this.CornerRadiusQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void ResetImageBorder()
    {
        this.ImageBorderPath = string.Empty;

        this.Plugin.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void ResetImage()
    {
        this.ImagePath = string.Empty;

        this.Plugin.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void ResetMargin()
    {
        this.MarginQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void ResetPadding()
    {
        this.PaddingQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void ResetSize()
    {
        this.Plugin.BorderManagement.Width = double.NaN;
        this.Plugin.BorderManagement.Height = double.NaN;

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        this.Plugin.ApplyBrush(this.BackgroundBrushSetup.ColorBrush, BrushTargets.Background);

        this.OnPropertyChanged(nameof(this.BBackground));
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        this.Plugin.ApplyBrush(this.BorderBrushSetup.ColorBrush, BrushTargets.Border);

        this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void SetColorValueAsForeground()
    {
        this.Plugin.ApplyBrush(this.ForegroundBrushSetup.ColorBrush, BrushTargets.Foreground);
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void SetColorValueAsHighlight()
    {
        this.Plugin.ApplyBrush(this.HighlightBrushSetup.ColorBrush, BrushTargets.Highlight);
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    private void SetElementSizeDisplayValue()
    {
        // need to look into string format again. on implementation, i failed to apply a no digit value to the label stringformat,
        // tried {}{0:F0} and some others, since it didn't work for whatever reason, i made them ints to circumvent the issue for now.
        // better solution for permanent use would be to use doubles and limit the digits on output. gonna try this again sometime, but it has no priority

        if (this.Plugin == null)
        {
            return;
        }

        this.widthValue = new SharedMethod_UI().ResetSizeDisplayValue(this.Plugin.BorderManagement.Width);
        this.heightValue = new SharedMethod_UI().ResetSizeDisplayValue(this.Plugin.BorderManagement.Height);

        this.OnPropertyChanged(nameof(this.WidthValue));
        this.OnPropertyChanged(nameof(this.HeightValue));
    }

    [RelayCommand]
    private void SetImage()
    {
        this.ImagePath = new SharedMethod_UI().SetBackgroundImage(this.ImagePath);

        this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    [RelayCommand]
    private void SetImageBorder()
    {
        this.ImageBorderPath = new SharedMethod_UI().SetBackgroundImage(this.ImageBorderPath);

        this.OnPropertyChanged(nameof(this.Plugin.BrushManagement));
        this.OnPropertyChanged(nameof(this.Plugin));
    }

    private bool SetImageToBrushTarget(BrushTargets brushTargets, string path)
    {
        if (string.IsNullOrWhiteSpace(path) || this.Plugin == null)
        {
            return false;
        }

        Brush brush = new ImageBrush();

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.Plugin.BrushManagement.Background, path);

                if (this.BackgroundBrushSetup != null)
                {
                    this.BackgroundBrushSetup.ColorStringValue = "imagebrush";
                }

                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.Plugin.BrushManagement.Border, path);

                if (this.BorderBrushSetup != null)
                {
                    this.BorderBrushSetup.ColorStringValue = "imagebrush";
                }
                break;
            case BrushTargets.Foreground:
            case BrushTargets.Highlight:
                break;
            default:
                break;
        }

        var applyBrushResult = this.Plugin!.ApplyBrush(brush, brushTargets);
        if (applyBrushResult)
        {
            this.OnPropertyChanged(nameof(this.Plugin));
            this.OnPropertyChanged(nameof(this.BBackground));
        }

        return applyBrushResult;
    }

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Plugin.BorderManagement.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.Plugin));
    }

    private void UpdateRotation(int rotationAngle)
    {
        this.RotateTransformValue = new RotateTransform(rotationAngle * -1);

        this.TransformOriginPoint = new Point(0.5, 0.5);
    }
}

// EOF