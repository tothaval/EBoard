/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardViewModel 
 * 
 *  view model class for EBoardView
 *  
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored and loaded
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoardSDK.Controls;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins;
using EBoardSDK.SharedMethods;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace EBoard.ViewModels;

public partial class EBoardViewModel : ObservableObject, IElementBackgroundImage
{

    private readonly MainViewModel mainViewModel;

    // Properties & Fields
    #region Properties & Fields

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
    private bool eBoardActive;


    [ObservableProperty]
    private int eBoardDepth;

    partial void OnEBoardDepthChanged(int value)
    {
        UpdateElementsZIndexProperties(value);
    }


    [ObservableProperty]
    private string eBoardName;


    private string _EBID;
    /// <summary>
    /// EBoard ID, created upon first creation,
    /// built using $"EBoard_{DateTime().Ticks}"        
    /// </summary>
    public string EBID => _EBID;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int height;

    partial void OnHeightChanged(int value)
    {
        BorderManager.Height = value;
    }


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


    /// <summary>
    /// property to set width for eboard instance, somehow it seemed to work better with
    /// a separate value for width besides BorderManager model and there was a problem
    /// with StringFormat in XAML that didn't work as intended, which would have been to
    /// cut all or almost all digits on the value label left to the slider, since the slider
    /// must not set or define every space between integer values, it is intended as a fast
    /// regulation tool. i intend to change the value label with textboxes, to allow the
    /// user to input a detailed value
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int width;

    partial void OnWidthChanged(int value)
    {
        BorderManager.Width = value;
    }

    #endregion


    // Collections
    #region Collections

    /// <summary>
    /// i tested an approach with a separate collection for a selection, but 
    /// since it got instanced a second time somehow, i removed it. if a list
    /// is necessary or means a lot of simplification in maintainance or development, 
    /// make sure to check if it is only one active instance, maybe via singleton,
    /// maybe via ref keyword. this would have been my next approaches to solve the issue.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    #endregion

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }
    // Constructors
    #region Constructors

    public EBoardViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
    }

    public EBoardViewModel(EboardDataSet eboardDataSet, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        ApplyData(eboardDataSet);
    }


    #endregion


    // Methods
    #region Methods

    internal void AddElement(ElementViewModel elementViewModel)
    {
        if (!Elements.Contains(elementViewModel))
        {
            Elements.Add(elementViewModel);
        }

        OnPropertyChanged(nameof(Elements));
    }


    internal void BeginElementSelectionMovement(ElementViewModel elementViewModel)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (!item.Equals(elementViewModel))
            {
                item.BeginMovement(elementViewModel);
            }

        }
    }


    public void ChangeElementBackgroundToImage()
    {
        BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }


    internal void ChangeSelection_CornerRadius(ElementViewModel elementViewModel, int cornerRadius)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.Equals(elementViewModel))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.Apply_CornerRadiusValue(cornerRadius);
            }
        }
    }


    internal void ChangeSelection_BackgroundBrush(ElementViewModel elementViewModel, Brush brush)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.Equals(elementViewModel))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.ApplyBackgroundBrush(brush);
            }
        }
    }


    internal void ChangeSelection_Height(ElementViewModel elementViewModel, int heightValue)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.Equals(elementViewModel))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.Apply_HeightValue(heightValue);
            }
        }
    }

    internal void ChangeSelection_RotationAngle(ElementViewModel elementViewModel, int rotationAngleValue)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (!item.Equals(elementViewModel) && item.IsSelected)
            {
                item.ApplyRotationAngleValue(rotationAngleValue);
            }
        }
    }
    internal void ChangeSelection_WidthValue(ElementViewModel elementViewModel, int widthValue)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.Equals(elementViewModel))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.ApplyWidthValue(widthValue);
            }
        }
    }
    internal void ChangeSelection_ZIndex(ElementViewModel elementViewModel, int zIndexValue)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.Equals(elementViewModel))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.ApplyZIndexValue(zIndexValue);
            }
        }
    }


    [RelayCommand]
    private void DeleteEBoard()
    {
        MainViewModel mainViewModel = Application.Current.MainWindow.DataContext as MainViewModel;

        if (mainViewModel is not null)
        {
            mainViewModel.EBoardBrowserViewModel.RemoveSelectedEBoard();
        }
    }


    internal void DeselectElements()
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.IsSelected)
            {
                item.Select();
            }
        }
    }


    // simplify to element count
    // later implement a statistic or live logging to show all instance types
    public int GetContainerCount()
    {
        //int containerCount = 0;

        //foreach (ElementViewModel item in Elements)
        //{
        //    containerCount++;            
        //}

        return Elements.Count;
    }


    // simplify to element count
    // later implement a statistic or live logging to show all instance types
    public int GetElementCount()
    {
        return Elements.Count;
    }


    // simplify to element count
    // later implement a statistic or live logging to show all instance types
    public int GetShapeCount()
    {
        //int containerCount = 0;

        //foreach (ElementViewModel item in Elements)
        //{
        //    if (item.IsShape)
        //    {
        //        containerCount++;
        //    }
        //}


        return Elements.Count;
    }


    public DateTime GetCreatedDate()
    {
        string cutEBID = _EBID.Replace("EBoard_", "");

        long ticks = long.Parse(cutEBID);

        DateTime dateTime = new DateTime(ticks);

        return dateTime;
    }

    public bool ApplyBrush(Brush brush, BrushTargets brushTargets)
    {
        try
        {
            switch (brushTargets)
            {
                case BrushTargets.Background:
                    BrushManager.Background = brush;
                    OnPropertyChanged(nameof(BrushManager.Background));
                    break;
                case BrushTargets.Border:
                    BrushManager.Border = brush;
                    OnPropertyChanged(nameof(BrushManager.Border));
                    break;
                case BrushTargets.Foreground:
                    BrushManager.Foreground = brush;
                    OnPropertyChanged(nameof(BrushManager.Foreground));
                    break;
                case BrushTargets.Highlight:
                    BrushManager.Highlight = brush;
                    OnPropertyChanged(nameof(BrushManager.Highlight));
                    break;
                default:
                    break;
            }

            OnPropertyChanged(nameof(BrushManager));

            return true;
        }
        catch (Exception)
        {

            return false;
        }
    }


    public void ApplyData(EboardDataSet eboardDataSet)
    {
        BorderManager = new BorderManagement(eboardDataSet.BorderDataSet);
        BrushManager = new BrushManagement(eboardDataSet.BrushDataSet);

        elements = eboardDataSet.Elements;

        EBoardName = eboardDataSet.EBoardName;
        EBoardDepth = eboardDataSet.EBoardDepth;

        _EBID = eboardDataSet.EBID;

        CornerRadiusValue = (int)BorderManager.CornerRadius.TopLeft;
        Height = (int)BorderManager.Height;
        Width = (int)BorderManager.Width;


        if (_EBID == null || _EBID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            _EBID = $"EBoard_{dateTime.Ticks}";
        }

        ApplyBrush(BrushManager.Background, BrushTargets.Background);
        ApplyBrush(BrushManager.Foreground, BrushTargets.Foreground);
        ApplyBrush(BrushManager.Border, BrushTargets.Border);
        ApplyBrush(BrushManager.Highlight, BrushTargets.Highlight);


        if (BrushManager.Background.GetType().Equals(typeof(SolidColorBrush)))
        {
            BackgroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)BrushManager.Background, SetColorValueAsBackground);
        }

        if (BrushManager.Foreground.GetType().Equals(typeof(SolidColorBrush)))
        {
            ForegroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)BrushManager.Foreground, SetColorValueAsForeground);
        }

        if (BrushManager.Border.GetType().Equals(typeof(SolidColorBrush)))
        {
            BorderBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)BrushManager.Border, SetColorValueAsBorder);
        }

        if (BrushManager.Highlight.GetType().Equals(typeof(SolidColorBrush)))
        {
            HighlightBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)BrushManager.Highlight, SetColorValueAsHighlight);
        }

        if (BackgroundBrushSetup == null)
        {
            BackgroundBrushSetup = new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) }, SetColorValueAsBackground);
        }
    }

    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        ApplyBrush(BackgroundBrushSetup.ColorBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void SetColorValueAsForeground()
    {
        ApplyBrush(ForegroundBrushSetup.ColorBrush, BrushTargets.Foreground);
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        ApplyBrush(BorderBrushSetup.ColorBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void SetColorValueAsHighlight()
    {
        ApplyBrush(HighlightBrushSetup.ColorBrush, BrushTargets.Highlight);
    }


    public void MoveElementSelection(ElementViewModel elementViewModel, Point newPosition)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }


            if (item.IsSelected)
            {
                item.MoveXY(elementViewModel, newPosition);
            }
        }

        OnPropertyChanged(nameof(Elements));
    }


    internal void MoveLastClickedElement(ElementViewModel elementViewModel)
    {
        Elements.Move(Elements.IndexOf(elementViewModel), Elements.Count - 1);
    }


    internal void RemoveElement(ElementViewModel elementViewModel)
    {
        Elements.Remove(elementViewModel);

        OnPropertyChanged(nameof(Elements));

        List<ElementViewModel> selectedElements = new List<ElementViewModel>();

        foreach (ElementViewModel item in Elements)
        {
            if (item.IsSelected)
            {
                selectedElements.Add(item);
            }
        }

        foreach (ElementViewModel item in selectedElements)
        {
            Elements.Remove(item);
        }


        OnPropertyChanged(nameof(Elements));
    }


    [RelayCommand]
    private void ResetImage()
    {
        ImagePath = string.Empty;
        BrushManager.Background = new SharedMethod_UI().ImagePathErrorDefaultBrush;

        OnPropertyChanged(nameof(BrushManager));
    }


    [RelayCommand]
    private void SetImage()
    {
        ImagePath = new SharedMethod_UI().SetBackgroundImage(ImagePath);
    }


    internal void StopElementSelectionMovement(ElementViewModel elementViewModel)
    {
        foreach (ElementViewModel item in Elements)
        {
            if (!item.EID.Equals(elementViewModel.EID) && item.IsSelected)
            {
                item.StopMovement();
            }
        }

        //OnPropertyChanged(nameof(Elements));
    }


    [RelayCommand]
    private void SwitchToEboard(object? parameter)
    {
        string commandParameter = parameter as string;

        if (this.mainViewModel != null && this.mainViewModel.EBoardBrowserViewModel.EBoards.Count > 1)
        {

            switch (commandParameter)
            {
                case "First":
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards.First();
                    break;
                case "Prev":
                    SwitchToPrevEboard();
                    break;

                case "Next":
                    SwitchToNextEboard();
                    break;

                case "Last":
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards.Last();
                    break;

                default:
                    break;
            }
        }
    }

    private void SwitchToNextEboard()
    {
        for (int i = 0; i < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count; i++)
        {
            if (this.mainViewModel.EBoardBrowserViewModel.EBoards[i] == this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard)
            {
                if (i + 1 < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count)
                {

                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards[i + 1];

                    break;
                }
            }
        }
    }

    private void SwitchToPrevEboard()
    {
        for (int i = 0; i < this.mainViewModel.EBoardBrowserViewModel.EBoards.Count; i++)
        {
            if (this.mainViewModel.EBoardBrowserViewModel.EBoards[i] == this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard)
            {
                if (i - 1 >= 0)
                {
                    this.mainViewModel.EBoardBrowserViewModel.SelectedEBoard = this.mainViewModel.EBoardBrowserViewModel.EBoards[i - 1];

                    break;
                }
            }
        }
    }

    private void UpdateElementsZIndexProperties(int newEBoardDepth)
    {
        if (Elements != null && Elements.Count > 0)
        {
            foreach (ElementViewModel item in Elements)
            {
                item.CalibrateZSliderValues(newEBoardDepth);
            }
        }
    }

    //[RelayCommand]
    //private void CollectiveClick()
    //{
    //    /// TODO rethink
    //    //foreach (ElementViewModel item in Elements)
    //    //{
    //    //    var it = item.Plugin.Plugin.DataContext;

    //    //    if (it != null)
    //    //    {
    //    //    if (item.Plugin.Plugin.DataContext.GetType().IsAssignableFrom(typeof(ICollectiveClickableObject)))
    //    //        {
    //    //            var doofdunuss = item.Plugin.Plugin.DataContext as ICollectiveClickableObject;

    //    //        //if (item.GetType().IsAssignableFrom(typeof(ICollectiveClickableObject)))
    //    //        //{
    //    //        //    var collectiveClicker = item.
    //    //        //}

    //    //    }

    //    //    }

    //    //}
    //}

    #endregion


}
// EOF