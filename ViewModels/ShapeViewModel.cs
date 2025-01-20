/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeViewModel 
 * 
 *  viewmodel for shape content elements
 */
using EBoard.Commands;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
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


        if (BorderManager == null)
        {
            BorderManager = new BorderManagement();
        }

        width = BorderManager.Width;
        height = BorderManager.Height;

        if (BrushManager == null)
        {
            BrushManager = new BrushManagement();
        }

        ((Shape)Control.Element).Fill = BrushManager.Background;
        ((Shape)Control.Element).Stroke = BrushManager.Border;
        ((Shape)Control.Element).StrokeThickness = BorderManager.BorderThickness.Left;

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

    public void ChangeElementBackgroundToImage()
    {
        if (ImagePath != null && ImagePath != string.Empty)
        {
            BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

            ((Shape)Control.Element).Fill = BrushManager.Background;
            ((Shape)Control.Element).Stroke = BrushManager.Border;
            ((Shape)Control.Element).StrokeThickness = BorderManager.BorderThickness.Left;

            _ElementViewModel.EBoardViewModel.ChangeSelection_BackgroundBrush(_ElementViewModel, BrushManager.Background);
        }

        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }


    public void DeselectElement()
    {
        IsSelected = false;

        BrushManager.SwitchBorderToBorder();

        ((Shape)Control.Element).Stroke = BrushManager.Border;

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

        ((Shape)Control.Element).Stroke = BrushManager.Border;

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