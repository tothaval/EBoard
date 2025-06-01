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
using EBoard.Models;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace EBoard.ViewModels;

public partial class EBoardViewModel : ObservableObject, IElementBackgroundImage
{
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private BorderManagement borderManager;

    [ObservableProperty]
    private BrushManagement brushManager;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int cornerRadiusValue;

    [ObservableProperty]
    private bool eBoardActive;

    [ObservableProperty]
    private int eBoardDepth;

    [ObservableProperty]
    private string eBoardName;

    private string eSID;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int height;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private int heightValue;

    partial void OnHeightValueChanged(int value)
    {
        if (this.BorderManager.Height != value)
        {
            this.BorderManager.Height = value;
        }

        OnPropertyChanged(nameof(BorderManager));
        OnPropertyChanged(nameof(BorderManager.Height));
    }

    [ObservableProperty]
    private int widthValue;
    partial void OnWidthValueChanged(int value)
    {
        if (this.BorderManager.Width != value)
        {
            this.BorderManager.Width = value;
        }

        OnPropertyChanged(nameof(BorderManager));
        OnPropertyChanged(nameof(BorderManager.Width));
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int width;

    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    public EBoardViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;
    }

    public EBoardViewModel(EboardDataSet eboardDataSet, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.ApplyData(eboardDataSet);
    }

    /// <summary>
    /// Gets eBoard ID, created upon first creation,
    /// built using $"EBoard_{DateTime().Ticks}".
    /// </summary>
    public string EBID => this.eSID;

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    private static string TxtRemoveEboardQuestion => "Remove Screen?";

    private static string TxtRemoveEboardTitle => "Screen Deletion";

    private static string TxtRemoveElementQuestion => "Remove Element?";

    private static string TxtRemoveElementTitle => "Element Deletion";



    public void AddElement(ElementViewModel elementViewModel)
    {
        if (!this.Elements.Contains(elementViewModel))
        {
            this.Elements.Add(elementViewModel);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void BeginElementSelectionMovement(ElementViewModel elementViewModel)
    {
        foreach (ElementViewModel item in this.Elements)
        {
            if (!item.Equals(elementViewModel))
            {
                item.BeginMovement(elementViewModel);
            }
        }
    }

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        this.SetImageToBrushTarget(brushTargets, path);
    }

    public void ChangeSelection_CornerRadius(ElementViewModel elementViewModel, QuadValue<int> cornerRadius)
    {
        foreach (ElementViewModel item in this.Elements)
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

    public void ChangeSelection_BackgroundBrush(ElementViewModel elementViewModel, Brush brush)
    {
        foreach (ElementViewModel item in this.Elements)
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

    public void ChangeSelection_Height(ElementViewModel elementViewModel, int heightValue)
    {
        foreach (ElementViewModel item in this.Elements)
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

    public void ChangeSelection_RotationAngle(ElementViewModel elementViewModel, int rotationAngleValue)
    {
        foreach (ElementViewModel item in this.Elements)
        {
            if (!item.Equals(elementViewModel) && item.IsSelected)
            {
                item.ApplyRotationAngleValue(rotationAngleValue);
            }
        }
    }

    public void ChangeSelection_WidthValue(ElementViewModel elementViewModel, int widthValue)
    {
        foreach (ElementViewModel item in this.Elements)
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

    public void ChangeSelection_ZIndex(ElementViewModel elementViewModel, int zIndexValue)
    {
        foreach (ElementViewModel item in this.Elements)
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

    public void Clear()
    {
        this.Elements?.Clear();
    }

    public void DeselectElements()
    {
        foreach (ElementViewModel item in this.Elements)
        {
            if (item.IsSelected)
            {
                item.Select();
            }
        }
    }

    public int GetContainerCount()
    {
        return this.Elements.Count;
    }

    public int GetElementCount()
    {
        return this.Elements.Count;
    }

    public int GetShapeCount()
    {
        return this.Elements.Count;
    }

    public DateTime GetCreatedDate()
    {
        string cutEBID = this.eSID.Replace("EBoard_", "");

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
                    this.BrushManager.Background = brush;
                    this.OnPropertyChanged(nameof(this.BrushManager.Background));
                    break;
                case BrushTargets.Border:
                    this.BrushManager.Border = brush;
                    this.OnPropertyChanged(nameof(this.BrushManager.Border));
                    break;
                case BrushTargets.Foreground:
                    this.BrushManager.Foreground = brush;
                    this.OnPropertyChanged(nameof(this.BrushManager.Foreground));
                    break;
                case BrushTargets.Highlight:
                    this.BrushManager.Highlight = brush;
                    this.OnPropertyChanged(nameof(this.BrushManager.Highlight));
                    break;
                default:
                    break;
            }

            this.OnPropertyChanged(nameof(this.BrushManager));

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void MoveElementSelection(ElementViewModel elementViewModel, Point newPosition)
    {
        foreach (ElementViewModel item in this.Elements)
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

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void StopElementSelectionMovement(ElementViewModel elementViewModel)
    {
        foreach (ElementViewModel item in this.Elements)
        {
            if (!item.EID.Equals(elementViewModel.EID) && item.IsSelected)
            {
                item.StopMovement();
            }
        }
    }

    public void MoveLastClickedElement(ElementViewModel elementViewModel)
    {
        this.Elements.Move(this.Elements.IndexOf(elementViewModel), this.Elements.Count - 1);
    }

    public void RemoveElement(ElementViewModel elementViewModel)
    {
        string question = TxtRemoveElementQuestion;
        string title = TxtRemoveElementTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.Elements.Remove(elementViewModel);

        this.OnPropertyChanged(nameof(this.Elements));

        List<ElementViewModel> selectedElements = new List<ElementViewModel>();

        foreach (ElementViewModel item in this.Elements)
        {
            if (item.IsSelected)
            {
                selectedElements.Add(item);
            }
        }

        foreach (ElementViewModel item in selectedElements)
        {
            this.Elements.Remove(item);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }
    public void ApplyData(EboardDataSet eboardDataSet)
    {
        this.BorderManager = new BorderManagement(eboardDataSet.BorderDataSet);
        this.BrushManager = new BrushManagement(eboardDataSet.BrushDataSet);

        this.Elements = eboardDataSet.Elements;

        this.EBoardName = eboardDataSet.EBoardName;
        this.EBoardDepth = eboardDataSet.EBoardDepth;

        this.eSID = eboardDataSet.EBID;

        this.CornerRadiusValue = (int)this.BorderManager.CornerRadius.TopLeft;
        this.Height = (int)this.BorderManager.Height;
        this.Width = (int)this.BorderManager.Width;

        if (this.eSID == null || this.eSID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eSID = $"EBoard_{dateTime.Ticks}";
        }

        this.ApplyBrush(this.BrushManager.Background, BrushTargets.Background);
        this.ApplyBrush(this.BrushManager.Foreground, BrushTargets.Foreground);
        this.ApplyBrush(this.BrushManager.Border, BrushTargets.Border);
        this.ApplyBrush(this.BrushManager.Highlight, BrushTargets.Highlight);

        if (this.BrushManager.Background.GetType().Equals(typeof(SolidColorBrush)))
        {
            this.BackgroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)this.BrushManager.Background, this.SetColorValueAsBackground);
        }

        if (this.BrushManager.Foreground.GetType().Equals(typeof(SolidColorBrush)))
        {
            this.ForegroundBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)this.BrushManager.Foreground, this.SetColorValueAsForeground);
        }

        if (this.BrushManager.Border.GetType().Equals(typeof(SolidColorBrush)))
        {
            this.BorderBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)this.BrushManager.Border, this.SetColorValueAsBorder);
        }

        if (this.BrushManager.Highlight.GetType().Equals(typeof(SolidColorBrush)))
        {
            this.HighlightBrushSetup = new SolidColorBrushSetupViewModel((SolidColorBrush)this.BrushManager.Highlight, this.SetColorValueAsHighlight);
        }

        this.BackgroundBrushSetup ??= new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) }, this.SetColorValueAsBackground);

        this.BorderBrushSetup ??= new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) }, this.SetColorValueAsBorder);

        var helper = new SharedMethod_UI();

        this.CornerRadiusQuadSetup = helper.BuildQuadValueSetup(
                new QuadValue<int>()
                {
                    Value1 = (int)this.BorderManager.CornerRadius.TopLeft,
                    Value2 = (int)this.BorderManager.CornerRadius.TopRight,
                    Value3 = (int)this.BorderManager.CornerRadius.BottomRight,
                    Value4 = (int)this.BorderManager.CornerRadius.BottomLeft,
                },
                this.ResetCorners);

        this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged; ; ;

        this.PaddingQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManager.Padding.Left,
                Value2 = (int)this.BorderManager.Padding.Top,
                Value3 = (int)this.BorderManager.Padding.Right,
                Value4 = (int)this.BorderManager.Padding.Bottom,
            },
            this.ResetPadding);

        this.PaddingQuadSetup.PropertyChanged += this.PaddingQuadSetup_PropertyChanged; ;

        this.MarginQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManager.Margin.Left,
                Value2 = (int)this.BorderManager.Margin.Top,
                Value3 = (int)this.BorderManager.Margin.Right,
                Value4 = (int)this.BorderManager.Margin.Bottom,
            },
            this.ResetThickness);

        this.MarginQuadSetup.PropertyChanged += this.MarginQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManager.BorderThickness.Left,
                Value2 = (int)this.BorderManager.BorderThickness.Top,
                Value3 = (int)this.BorderManager.BorderThickness.Right,
                Value4 = (int)this.BorderManager.BorderThickness.Bottom,
            },
            this.ResetMargin);

        this.ThicknessQuadSetup.PropertyChanged += this.ThicknessQuadSetup_PropertyChanged;
    }

    private void CornerRadiusQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManager.CornerRadius = new CornerRadius(
            this.CornerRadiusQuadSetup.QuadValue.Value1,
            this.CornerRadiusQuadSetup.QuadValue.Value2,
            this.CornerRadiusQuadSetup.QuadValue.Value3,
            this.CornerRadiusQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManager));
    }

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManager.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManager));
    }

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManager.Padding = new Thickness(
            this.PaddingQuadSetup.QuadValue.Value1,
            this.PaddingQuadSetup.QuadValue.Value2,
            this.PaddingQuadSetup.QuadValue.Value3,
            this.PaddingQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManager));
    }

    private void MarginQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManager.Margin = new Thickness(
            this.MarginQuadSetup.QuadValue.Value1,
            this.MarginQuadSetup.QuadValue.Value2,
            this.MarginQuadSetup.QuadValue.Value3,
            this.MarginQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManager));
    }

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManager.CornerRadius = new CornerRadius(value);
    }

    partial void OnEBoardDepthChanged(int value)
    {
        UpdateElementsZIndexProperties(value);
    }

    partial void OnHeightChanged(int value)
    {
        BorderManager.Height = value;
    }

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    partial void OnImageBorderPathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Border, value);
    }

    partial void OnWidthChanged(int value)
    {
        BorderManager.Width = value;
    }

    private bool SetImageToBrushTarget(BrushTargets brushTargets, string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        Brush brush = new ImageBrush();

        switch (brushTargets)
        {
            case BrushTargets.Background:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManager.Background, path);
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManager.Border, path);
                break;
            case BrushTargets.Foreground:
            case BrushTargets.Highlight:
                break;
            default:
                break;
        }

        return (bool)this.ApplyBrush(brush, brushTargets);
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        if (this.mainViewModel is not null)
        {
            MessageBoxResult result = MessageBox.Show(TxtRemoveEboardQuestion, TxtRemoveEboardTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            this.mainViewModel.EBoardBrowserViewModel.RemoveSelectedEBoard();
        }
    }

    [RelayCommand]
    private void LeftClick()
    {
        this.DeselectElements();
    }

    [RelayCommand]
    private void ResetCorners()
    {
        this.CornerRadiusQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    [RelayCommand]
    private void ResetImage()
    {
        this.ImagePath = string.Empty;

        this.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void ResetMargin()
    {
        this.MarginQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    [RelayCommand]
    private void ResetPadding()
    {
        this.PaddingQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    [RelayCommand]
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    [RelayCommand]
    private void ResetSize()
    {
        this.BorderManager.Width = double.NaN;
        this.BorderManager.Height = double.NaN;

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.BorderManager));
    }

    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        this.ApplyBrush(this.BackgroundBrushSetup.ColorBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void SetColorValueAsForeground()
    {
        this.ApplyBrush(this.ForegroundBrushSetup.ColorBrush, BrushTargets.Foreground);
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        this.ApplyBrush(this.BorderBrushSetup.ColorBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void SetColorValueAsHighlight()
    {
        this.ApplyBrush(this.HighlightBrushSetup.ColorBrush, BrushTargets.Highlight);
    }

    [RelayCommand]
    private void SetImage()
    {
        this.ImagePath = new SharedMethod_UI().SetBackgroundImage(this.ImagePath);

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    [RelayCommand]
    private void SetImageBorder()
    {
        this.ImageBorderPath = new SharedMethod_UI().SetBackgroundImage(this.ImageBorderPath);

        this.OnPropertyChanged(nameof(this.BrushManager));
    }

    private void SetElementSizeDisplayValue()
    {
        this.widthValue = new SharedMethod_UI().ResetSizeDisplayValue(this.BorderManager.Width);
        this.heightValue = new SharedMethod_UI().ResetSizeDisplayValue(this.BorderManager.Height);

        this.OnPropertyChanged(nameof(this.WidthValue));
        this.OnPropertyChanged(nameof(this.HeightValue));
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
                    this.SwitchToPrevEboard();
                    break;

                case "Next":
                    this.SwitchToNextEboard();
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
        if (this.Elements != null && this.Elements.Count > 0)
        {
            foreach (ElementViewModel item in this.Elements)
            {
                item.CalibrateZSliderValues(newEBoardDepth);
            }
        }
    }
}

// EOF