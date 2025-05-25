/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementViewModel 
 * 
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;

using EBoard.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EBoardSDK.Controls;

namespace EBoard.ViewModels;

public partial class ElementViewModel : ObservableObject, IElementSelection, IElementBackgroundImage
{

    // Properties & Fields
    #region Properties & Fields

    private ElementView _ElementView;
    
    private EBoardViewModel _EBoardViewModel;

    [ObservableProperty]
    private string imagePath;

    public ElementView ElementView => _ElementView;

    public EBoardViewModel EBoardViewModel => _EBoardViewModel;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage();
    }

    [ObservableProperty]
    private bool isSelected;

    private IPlugin plugin;
    public IPlugin Plugin => plugin;

    [ObservableProperty]
    private PlacementManagement placementManager;

    [ObservableProperty]
    private int rotationAngleValue;

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            ChangeSelection_RotationAngleValue(newValue);

            UpdateRotation(newValue);
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlacementManager))]
    private RotateTransform rotateTransformValue;

    partial void OnRotateTransformValueChanged(RotateTransform value)
    {
        placementManager.Angle = RotationAngleValue;
    }


    [ObservableProperty]
    private Point transformOriginPoint;


    [ObservableProperty]
    private int cornerRadiusValue;

    partial void OnCornerRadiusValueChanged(int value)
    {
        UpdateCornerRadius(value);

        ChangeSelection_CornerRadiusValue(value);
    }


    [ObservableProperty]
    private int heightValue;

    partial void OnHeightValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Plugin is not null)
        {
            Plugin.Height = value;
        }

        OnPropertyChanged(nameof(Plugin));

        ChangeSelection_HeightValue(value);
    }


    [ObservableProperty]
    private int widthValue;

    partial void OnWidthValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Plugin is not null)
        {
            Plugin.Width = value;
        }

        UpdateContentWidth(value);

        ChangeSelection_WidthValue(value);
    }


    private string _EID;
    /// <summary>
    /// Element ID, created upon first creation,
    /// built using $"Element_{DateTime().Ticks}"        
    /// </summary>
    public string EID => _EID;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlacementManager))]
    private int zIndexValue = 0;

    partial void OnZIndexValueChanged(int value)
    {
        PlacementManager.Z = value;

        ChangeSelection_ZIndexValue(value);
    }


    [ObservableProperty]
    private int zMinimumValue;


    [ObservableProperty]
    private int zMaximumValue;

    [ObservableProperty]
    private int xSliderValue;
    partial void OnXSliderValueChanged(int value)
    {
        XPosition = value;
    }

    [ObservableProperty]
    private double xPosition;
    partial void OnXPositionChanged(double value)
    {
        XSliderValue = (int)value;
    }

    [ObservableProperty]
    private int xMaximumValue;

    [ObservableProperty]
    private int ySliderValue;
    partial void OnYSliderValueChanged(int value)
    {
        YPosition = value;
    }

    [ObservableProperty]
    private double yPosition;
    partial void OnYPositionChanged(double value)
    {
        YSliderValue = (int)value;
    }


    [ObservableProperty]
    private int yMaximumValue;


    [ObservableProperty]
    private SolidColorBrush backgroundColorBrush;
    partial void OnBackgroundColorBrushChanged(SolidColorBrush value)
    {

    }

    [ObservableProperty]
    private SolidColorBrush foregroundColorBrush;


    [ObservableProperty]
    private SolidColorBrush borderColorBrush = new SolidColorBrush(Colors.Black);


    [ObservableProperty]
    private SolidColorBrush highlightColorBrush = new SolidColorBrush(Colors.Orange);

    #endregion


    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; } 

    public ElementViewModel()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    /// <param name="elementDataSet"></param>
    public ElementViewModel(EBoardViewModel eBoardViewModel, IElementDataSet elementDataSet) => ApplyData(eBoardViewModel, elementDataSet);


    // Methods
    #region Methods

    internal void ApplyBackgroundBrush(Brush brush)
    {
        Plugin.ApplyBrush(brush, BrushTargets.Background);
    }


    // in order to apply the value onto every selected element without triggering the value change 
    // and ChangeSelection_VALUE everytime in every element insance, use the ApplyVALUE method 
    public void Apply_CornerRadiusValue(int cornerRadius)
    {
        CornerRadiusValue = cornerRadius;

        UpdateCornerRadius(cornerRadius);
    }


    public void Apply_HeightValue(int heightValue)
    {
        HeightValue = heightValue;

        UpdateContentHeight(heightValue);
    }

    public int ApplyRotationAngleValue(int rotationAngleValue)
    {
        UpdateRotation(rotationAngleValue);

        this.RotationAngleValue = rotationAngleValue;

        OnPropertyChanged(nameof(RotationAngleValue));

        return RotationAngleValue;
    }

    public int ApplyRotationAngleValueByMouseWheel(int delta)
    {
        if (delta < 0 && RotationAngleValue > -180)
        {
            RotationAngleValue--;
        }

        if (delta > 0 && RotationAngleValue < 180)
        {
            RotationAngleValue++;
        }

        UpdateRotation(RotationAngleValue);

        OnPropertyChanged(nameof(RotationAngleValue));

        return RotationAngleValue;
    }

    public void ApplyWidthValue(int widthValue)
    {
        WidthValue = widthValue;

        UpdateContentWidth(widthValue);
    }


    public int ApplyZIndexValueByMouseWheel(int delta)
    {
        if (delta < 0 && ZMinimumValue < ZIndexValue)
        {
            ZIndexValue--;
        }

        if (delta > 0 && ZMaximumValue > ZIndexValue)
        {
            ZIndexValue++;
        }

        PlacementManager.Z = ZIndexValue;

        OnPropertyChanged(nameof(PlacementManager.Z));

        return ZIndexValue;
    }


    public void ApplyZIndexValue(int zIndexValue)
    {
        ZIndexValue = zIndexValue;

        PlacementManager.Z = zIndexValue;

        OnPropertyChanged(nameof(PlacementManager.Z));
    }


    internal void BeginMovement(ElementViewModel elementViewModel)
    {
        XPosition = ElementView.X;
        YPosition = ElementView.Y;

        PlacementManager.Position = new Point(XPosition, YPosition);
    }


    public void CalibrateZSliderValues(int eboardDepth)
    {
        if (eboardDepth >= 0)
        {
            ZMinimumValue = 0;
            ZMaximumValue = eboardDepth;

            if (eboardDepth == 0)
            {
                ZMaximumValue = 1;
            }
        }
        else if (eboardDepth < 0)
        {
            ZMinimumValue = eboardDepth;
            ZMaximumValue = 0;
        }

        OnPropertyChanged(nameof(ZMinimumValue));
        OnPropertyChanged(nameof(ZMaximumValue));

        OnPropertyChanged(nameof(XMaximumValue));
        OnPropertyChanged(nameof(YMaximumValue));
    }


    public void ChangeElementBackgroundToImage()
    {
        if (ImagePath != null && ImagePath != string.Empty)
        {
            Plugin?.ApplyBrush(new SharedMethod_UI().ChangeBackgroundToImage(Plugin.BrushManagement.Background, ImagePath), BrushTargets.Background);
        }
    }


    private void ChangeSelection_CornerRadiusValue(int cornerRadius)
    {
        _EBoardViewModel?.ChangeSelection_CornerRadius(this, cornerRadius);
    }


    private void ChangeSelection_HeightValue(int heightValue)
    {
        _EBoardViewModel?.ChangeSelection_Height(this, heightValue);
    }


    private void ChangeSelection_RotationAngleValue(int rotationAngleValue)
    {
        _EBoardViewModel?.ChangeSelection_RotationAngle(this, rotationAngleValue);
    }


    private void ChangeSelection_WidthValue(int widthValue)
    {
        _EBoardViewModel?.ChangeSelection_WidthValue(this, widthValue);
    }


    private void ChangeSelection_ZIndexValue(int zIndexValue)
    {
        _EBoardViewModel.ChangeSelection_ZIndex(this, zIndexValue);
    }


    public void ApplyData(EBoardViewModel eBoardViewModel, IElementDataSet elementDataSet)
    {

        PlacementManager = new PlacementManagement();

        _EBoardViewModel = eBoardViewModel;

        _EID = elementDataSet.EID;

        // need to look into string format again. on implementation, i failed to apply a no digit value to the label stringformat,
        // tried {}{0:F0} and some others, since it didn't work for whatever reason, i made them ints to circumvent the issue for now.
        // better solution for permanent use would be to use doubles and limit the digits on output. gonna try this again sometime, but it has no priority
        HeightValue = (int)elementDataSet.BorderDataSet.Height;
        WidthValue = (int)elementDataSet.BorderDataSet.Width;

        if (elementDataSet.PlacementDataSet != null)
        {
            PlacementManager = new PlacementManagement(elementDataSet.PlacementDataSet);

            RotationAngleValue = (int)elementDataSet.PlacementDataSet.Angle;
            //PlacementManager.Position = elementDataSet.PlacementDataSet.Position;
            //PlacementManager.Z = elementDataSet.PlacementDataSet.Z;

            XPosition = PlacementManager.Position.X;
            YPosition = PlacementManager.Position.Y;
            XMaximumValue = eBoardViewModel.Width;
            YMaximumValue = eBoardViewModel.Height;

            ZIndexValue = PlacementManager.Z;
        }

        if (_EID == null || _EID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            _EID = $"Element_{dateTime.Ticks}";
        }

        if (elementDataSet.Plugin != null)
        {
            plugin = elementDataSet.Plugin;

            Plugin.Height = HeightValue;
            Plugin.Width = WidthValue;

            var brushdataset = new BrushManagement(elementDataSet.BrushDataSet);

            Plugin.ApplyBrush(brushdataset.Background, BrushTargets.Background);
            Plugin.ApplyBrush(brushdataset.Foreground, BrushTargets.Foreground);
            Plugin.ApplyBrush(brushdataset.Border, BrushTargets.Border);
            Plugin.ApplyBrush(brushdataset.Highlight, BrushTargets.Highlight);

            CornerRadiusValue = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft;


            if (Plugin.BrushManagement.Background.GetType().Equals(typeof(SolidColorBrush)))
            {
                BackgroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)Plugin.BrushManagement.Background, SetColorValueAsBackground);
            }

            if (Plugin.BrushManagement.Foreground.GetType().Equals(typeof(SolidColorBrush)))
            {
                ForegroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)Plugin.BrushManagement.Foreground, SetColorValueAsForeground);
            }

            if (Plugin.BrushManagement.Border.GetType().Equals(typeof(SolidColorBrush)))
            {
                BorderBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)Plugin.BrushManagement.Border, SetColorValueAsBorder);
            }

            if (Plugin.BrushManagement.Highlight.GetType().Equals(typeof(SolidColorBrush)))
            {
                HighlightBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)Plugin.BrushManagement.Highlight, SetColorValueAsHighlight);
            }

            if (BackgroundBrushSetup == null)
            {
                BackgroundBrushSetup = new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color=Color.FromArgb(255,0,0,0)}, SetColorValueAsBackground);
            }

            OnPropertyChanged(nameof(Plugin));

            CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);
        }

        ApplyRotationAngleValue(RotationAngleValue);
    }

    public void MoveXY(ElementViewModel elementViewModel, Point deltaPosition)
    {

        if (_ElementView != null)
        {
            double x, y;

            x = XPosition - deltaPosition.X;
            y = YPosition - deltaPosition.Y;

            PlacementManager.Position = new Point(x, y);

            Canvas.SetLeft(ElementView.VisualParent, x);
            Canvas.SetTop(ElementView.VisualParent, y);
        }
    }


    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        Plugin.ApplyBrush(BackgroundBrushSetup.ColorBrush, BrushTargets.Background);
    }

    [RelayCommand]    
    private void SetColorValueAsForeground()
    {
        Plugin.ApplyBrush(ForegroundBrushSetup.ColorBrush, BrushTargets.Foreground);
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        Plugin.ApplyBrush(BorderBrushSetup.ColorBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void SetColorValueAsHighlight()
    {
        Plugin.ApplyBrush(HighlightBrushSetup.ColorBrush, BrushTargets.Highlight);
    }


    [RelayCommand]
    private void DeleteElement(object s)
    {
        _EBoardViewModel?.RemoveElement(this);
    }


    [RelayCommand]
    private void ResetImage()
    {
        ImagePath = string.Empty;

        Plugin.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Background);
    }


    [RelayCommand]
    public void Select()
    {
        IsSelected = !IsSelected;

        Plugin?.SelectionChange(IsSelected);
    }


    [RelayCommand]
    private void SetImage()
    {
        ImagePath = new SharedMethod_UI().SetBackgroundImage(ImagePath);
    }


    public void SetView(ElementView elementView)
    {
        _ElementView = elementView;
    }


    internal void StopMovement()
    {
        XPosition = Canvas.GetLeft(_ElementView.VisualParent);
        YPosition = Canvas.GetTop(_ElementView.VisualParent);

        PlacementManager.Position = new Point(XPosition, YPosition);
    }


    private void UpdateContentHeight(int height)
    {
        if (Plugin is not null)
        {
            Plugin.Height = height;
        }
    }


    private void UpdateContentWidth(int width)
    {
        if (Plugin is not null)
        {
            Plugin.Width = width;
        }
    }


    private void UpdateCornerRadius(int value)
    {
        if (Plugin is not null)
        {
            Plugin.CornerRadiusValue = value;
        }
    }


    private void UpdateRotation(int rotationAngle)
    {
        RotateTransformValue = new RotateTransform(rotationAngle * -1);

        TransformOriginPoint = new Point(0.5, 0.5);
    }

    internal void WasLastActive()
    {
        _EBoardViewModel?.MoveLastClickedElement(this);
    }

    #endregion

}
// EOF