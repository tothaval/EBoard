/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContentViewModel 
 * 
 *  view model for usercontrol content elements
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Commands;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Utilities.SharedMethods;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EBoard.ViewModels;

public partial class ContentViewModel : ObservableObject, IElementBackgroundImage, IUIManager
{

    public IElementContent Control { get; }
    

    public ElementDataSet ElementDataSet { get; }
    

    private ElementViewModel _ElementViewModel;        
    public ElementViewModel ElementViewModel => _ElementViewModel;


    public bool IsSelected { get; set; } = false;


    [ObservableProperty]
    private BorderManagement borderManager;


    [ObservableProperty]
    private BrushManagement brushManager;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int cornerRadiusValue;

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManager.CornerRadius = new CornerRadius(value);
    }


    [ObservableProperty]
    private string elementHeaderText;


    /// <summary>
    /// the path to an optional background image for the
    /// element, if empty, the stored brush or a default
    /// solidColorBrush will be used for the background
    /// </summary>
    [ObservableProperty]
    private string imagePath;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage();
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private double height;

    partial void OnHeightChanged(double value)
    {
        BorderManager.Height = value;
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private double width;

    partial void OnWidthChanged(double value)
    {
        BorderManager.Width = value;
    }


    public ContentViewModel(ElementDataSet elementDataSet, ElementViewModel elementViewModel)
    {
        _ElementViewModel = elementViewModel;
        ElementDataSet = elementDataSet;
        BorderManager = new BorderManagement(elementDataSet.BorderDataSet);
        BrushManager = new BrushManagement(elementDataSet.BrushDataSet);
        Control = elementDataSet.ElementContent;

        ElementHeaderText = elementDataSet.ElementHeader;

        ImagePath = BrushManager.ImagePath;

        CornerRadiusValue = (int)elementDataSet.BorderDataSet.CornerRadius.TopLeft;
    }


    public bool ApplyBackgroundBrush(Brush brush)
    {
        try
        {
            BrushManager.Background = brush;

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

            _ElementViewModel.EBoardViewModel.ChangeSelection_BackgroundBrush(_ElementViewModel, BrushManager.Background); 
        }


        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }


    public void DeselectElement()
    {
        IsSelected = false;

        BrushManager.SwitchBorderToBorder();

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

        OnPropertyChanged(nameof(BrushManager));
    }

}
// EOF