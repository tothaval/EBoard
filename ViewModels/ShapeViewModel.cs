/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeViewModel 
 * 
 *  viewmodel for shape content elements
 */
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using System.Windows.Media;
using System.Windows.Shapes;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace EBoard.ViewModels;

public partial class ShapeViewModel : ObservableObject, IElementBackgroundImage, IUIManager
{

    public IElementContent Control { get; }

    
    public ElementDataSet ElementDataSet { get; }


    private ElementViewModel _ElementViewModel;        
    public ElementViewModel ElementViewModel => _ElementViewModel;


    private Brush _FallbackBackgroundBrush;
    public Brush FallbackBackgroundBrush => _FallbackBackgroundBrush;
    

    public bool IsSelected { get; set; } = false;


    [ObservableProperty]
    private BorderManagement _BorderManager;


    [ObservableProperty]
    private BrushManagement brushManager;


    [ObservableProperty]
    private string elementHeaderText;
    

    /// <summary>
    /// the path to an optional background image for the
    /// element, if empty, the stored brush or a default
    /// solidColorBrush will be used for the background
    /// </summary>

    [ObservableProperty]
    private string imagePath;
    
    partial void OnImagePathChanged(string imagePath)
        => ChangeElementBackgroundToImage();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]        
    private double height;

    partial void OnHeightChanged(double value)
    => UpdateDimensions();


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private double width;

    partial void OnWidthChanged(double value)
            => UpdateDimensions();


    public ShapeViewModel(ElementDataSet elementDataSet, ElementViewModel elementViewModel)
    {
        _ElementViewModel = elementViewModel;
        ElementDataSet = elementDataSet;
        BorderManager = new BorderManagement(elementDataSet.BorderDataSet);
        BrushManager = new BrushManagement(elementDataSet.BrushDataSet);
        Control = elementDataSet.ElementContent;

        width = BorderManager.Width;
        height = BorderManager.Height;               

        ElementHeaderText = elementDataSet.ElementHeader;

        ImagePath = BrushManager.ImagePath;

        if (ImagePath == null)
        {
            ImagePath = string.Empty;
        }

        if (ElementHeaderText == null)
        {
            ElementHeaderText = "Shape Element";
        }

        ApplyFillAndStrokeValues();
    }


    public bool ApplyBackgroundBrush(Brush brush)
    {
        try
        {
            BrushManager.Background = brush;

            ((Shape)Control.Element).Fill = brush;

            OnPropertyChanged(nameof(BrushManager));

            OnPropertyChanged(nameof(BrushManager.Background));

            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }

    public bool ApplyFillAndStrokeValues(bool isSelectionChange = false)
    {
        if (BrushManager is null || BorderManager is null)
        {
            return false;
        }

        if (isSelectionChange)
        {
        ((Shape)Control.Element).Stroke = BrushManager.Border;
        ((Shape)Control.Element).StrokeThickness = BorderManager.BorderThickness.Left;
        }
            ((Shape)Control.Element).Fill = BrushManager.Background;
        

        return true;
    }


    public void ChangeElementBackgroundToImage()
    {
        if (ImagePath != null && ImagePath != string.Empty)
        {
            BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

            ApplyFillAndStrokeValues();

            _ElementViewModel.EBoardViewModel.ChangeSelection_BackgroundBrush(_ElementViewModel, BrushManager.Background);
        }

        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }


    public void DeselectElement()
    {
        IsSelected = false;

        BrushManager.SwitchBorderToBorder();

        ApplyFillAndStrokeValues(true);

        OnPropertyChanged(nameof(BrushManager));
    }


    [RelayCommand]
    public void Select()
    {
        if (IsSelected)
        {
            DeselectElement();
            return;
        }

        IsSelected = true;

        BrushManager.SwitchBorderToHighlight();

        ApplyFillAndStrokeValues(true);

        OnPropertyChanged(nameof(BrushManager));
    }


    public void UpdateDimensions()
    {
        if (BorderManager != null)
        {
            BorderManager.Height = Height;
            BorderManager.Width = Width;

            if (Control != null && Control.Element != null)
            {
                ((Shape)Control.Element).Width = Width;
                ((Shape)Control.Element).Height = Height;

            }
        }
    }


}
// EOF