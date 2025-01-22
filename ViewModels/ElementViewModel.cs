/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementViewModel 
 * 
 *  view model for ElementView, which offers some basic dragmove functionality,
 *  element placement properties within EBoardView canvas and basic content management
 */

using EBoard.Commands;
using EBoard.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using EBoard.Interfaces;
using EBoard.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Utilities.SharedMethods;
using System.CodeDom;

namespace EBoard.ViewModels;

public partial class ElementViewModel : ObservableObject
{

    // Properties & Fields
    #region Properties & Fields

    private ElementView _ElementView;
    public ElementView ElementView => _ElementView;

    public bool IsSender { get; set; } = false;


    private EBoardViewModel _EBoardViewModel;
    public EBoardViewModel EBoardViewModel => _EBoardViewModel;

    private IUIManager _ContentContainer;
    public IUIManager ContentContainer => _ContentContainer;

    private ContentViewModel _ContentViewModel;
    public ContentViewModel ContentViewModel => _ContentViewModel;

    private ShapeViewModel _ShapeViewModel;
    public ShapeViewModel ShapeViewModel => _ShapeViewModel;


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

        if (IsContent && ContentViewModel != null)
        {
            ContentViewModel.Height = value;
            OnPropertyChanged(nameof(ContentViewModel));
        }

        if (IsShape && ShapeViewModel != null)
        {
            ShapeViewModel.Height = value;
            OnPropertyChanged(nameof(ShapeViewModel));
        }

        ChangeSelection_HeightValue(value);
    }


    [ObservableProperty]
    private int widthValue;

    partial void OnWidthValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

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
    private bool isShape = false;


    [ObservableProperty]
    private bool isContent = false;


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
    private double xPosition;


    [ObservableProperty]
    private double yPosition;

    public Point MoveDiff { get; set; }

    #endregion


    /// <summary>
    /// 
    /// </summary>
    /// <param name="eBoardViewModel"></param>
    /// <param name="x">x axis</param>
    /// <param name="y">y axis</param>
    /// <param name="z">depth</param>
    /// <param name="elementHeaderText">element header</param>
    /// <param name="brush">background brush</param>
    /// <param name="control">element content</param>
    public ElementViewModel(
        EBoardViewModel eBoardViewModel,
        ElementDataSet elementDataSet
        )
    {

        IsContent = false;
        IsShape = false;

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
            RotationAngleValue = (int)elementDataSet.PlacementDataSet.Angle;
            PlacementManager.Position = elementDataSet.PlacementDataSet.Position;
            PlacementManager.Z = elementDataSet.PlacementDataSet.Z;

            XPosition = PlacementManager.Position.X;
            YPosition = PlacementManager.Position.Y;

            ZIndexValue = PlacementManager.Z;
        }

        if (_EID == null || _EID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            _EID = $"Element_{dateTime.Ticks}";
        }


        if (elementDataSet.ElementContent != null)
        {
            if (elementDataSet.ElementContent.ContentIsUserControlAndNotShape)
            {
                _ContentViewModel = new ContentViewModel(elementDataSet, this);
                IsContent = true;

                _ContentContainer = _ContentViewModel;

                CornerRadiusValue = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft;

                OnPropertyChanged(nameof(ContentViewModel));
            }
            else
            {
                _ShapeViewModel = new ShapeViewModel(elementDataSet, this);
                IsShape = true;

                _ContentContainer = _ShapeViewModel;

                OnPropertyChanged(nameof(ShapeViewModel));
            }


            CalibrateZSliderValues(_EBoardViewModel.EBoardDepth);
        }
    }

    // Methods
    #region Methods


    internal void ApplyBackgroundBrush(Brush brush)
    {
        ContentContainer.ApplyBackgroundBrush(brush);
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

        this.rotationAngleValue = rotationAngleValue;

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
    private void DeleteElement(object s)
    {
        _EBoardViewModel?.RemoveElement(this);
    }


    [RelayCommand]
    private void ResetImage()
    {
        if (IsContent)
        {
            ContentViewModel.ImagePath = string.Empty;
            ContentViewModel.ApplyBackgroundBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush);
        }

        if (IsShape)
        {
            ShapeViewModel.ImagePath = string.Empty;
            ShapeViewModel.ApplyBackgroundBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush);

            OnPropertyChanged(nameof(ShapeViewModel.BrushManager.Background));
            OnPropertyChanged(nameof(ContentViewModel.BrushManager));
        }
    }


    [RelayCommand]
    private void SetImage()
    {
        if (IsContent)
        {
            ContentViewModel.ImagePath = new SharedMethod_UI().SetBackgroundImage(ContentViewModel.ImagePath);
        }

        if (IsShape)
        {
            ShapeViewModel.ImagePath = new SharedMethod_UI().SetBackgroundImage(ShapeViewModel.ImagePath);
        }
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
        if (IsContent && ContentViewModel != null)
        {
            ContentViewModel.Height = height;

            OnPropertyChanged(nameof(ContentViewModel));
        }

        if (IsShape && ShapeViewModel != null)
        {
            ShapeViewModel.Height = height;
            OnPropertyChanged(nameof(ShapeViewModel));
        }
    }


    private void UpdateContentWidth(int width)
    {
        if (IsContent && ContentViewModel != null)
        {
            ContentViewModel.Width = width;

            OnPropertyChanged(nameof(ContentViewModel));
        }

        if (IsShape && ShapeViewModel != null)
        {
            ShapeViewModel.Width = width;
            OnPropertyChanged(nameof(ShapeViewModel));
        }
    }


    private void UpdateCornerRadius(int value)
    {
        if (IsContent && ContentViewModel != null)
        {
            ContentViewModel.CornerRadiusValue = value;
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