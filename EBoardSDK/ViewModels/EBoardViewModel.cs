// <copyright file="EBoardViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  EBoardViewModel
 *
 *  view model class for EBoardView
 *
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored and loaded
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.ScreenIntegration;
using EBoardSDK.SharedMethods;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using EBoardSDK.DataSets;

public partial class EBoardViewModel : ObservableObject, IElementBackgroundImage, IEboardIdentity, IDisposable
{
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private BorderManagement borderManagement;

    [ObservableProperty]
    private BrushManagement brushManagement;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int cornerRadiusValue;

    [ObservableProperty]
    private bool eBoardActive;

    [ObservableProperty]
    private int eBoardDepth;

    [ObservableProperty]
    private string eBoardName;

    private string eSID;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int height;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private int heightValue;

    [ObservableProperty]
    private int widthValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int width;

    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    public EBoardViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.brushManagement = new BrushManagement();

        this.brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    public EBoardViewModel(EboardDataSet eboardDataSet, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.ApplyData(eboardDataSet);

        this.brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    public IList<ElementInstantiationPolicy>? InstantiationPolicies => [
        ElementInstantiationPolicy.Unique,
        ElementInstantiationPolicy.Global,
        ElementInstantiationPolicy.OnePerScreen,
        ElementInstantiationPolicy.DefaultScreenTypesOnly,
        //ElementInstantiationPolicy.Unconstrained,
        //ElementInstantiationPolicy.ValueNotSet
    ];

    public IList<EboardScreenType>? ScreenTypes => [
        EboardScreenType.EBoardDefault,
        EboardScreenType.EboardSDKDefault,
        ];

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

    private static string TxtRemoveAllElementsQuestion => "Clear all elements?";

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
        string question = TxtRemoveAllElementsQuestion;
        string title = TxtRemoveElementTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

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

    public void Dispose()
    {
        this.BrushManagement.PropertyChangedEvent -= this.BrushManagement_PropertyChangedEvent;

        this.MarginQuadSetup.PropertyChanged -= this.MarginQuadSetup_PropertyChanged;

        this.PaddingQuadSetup.PropertyChanged -= this.PaddingQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup.PropertyChanged -= this.ThicknessQuadSetup_PropertyChanged;

        GC.SuppressFinalize(this);
    }

    public int GetContainerCount()
    {
        return this.Elements.Count;
    }

    public int GetElementCount()
    {
        return this.Elements.Where(x => x.Plugin.PluginCategory != PluginCategories.Shape).ToList().Count;
    }

    public int GetShapeCount()
    {
        return this.Elements.Where(x => x.Plugin.PluginCategory == PluginCategories.Shape).ToList().Count;
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
                    this.BrushManagement.Background = brush;

                    if (brush.GetType() == typeof(ImageBrush) && this.BackgroundBrushSetup != null)
                    {
                        this.BackgroundBrushSetup.ColorStringValue = "imagebrush";
                    }

                    this.OnPropertyChanged(nameof(this.BrushManagement.Background));
                    break;

                case BrushTargets.Border:
                    this.BrushManagement.Border = brush;

                    if (brush.GetType() == typeof(ImageBrush) && this.BorderBrushSetup != null)
                    {
                        this.BorderBrushSetup.ColorStringValue = "imagebrush";
                    }

                    this.OnPropertyChanged(nameof(this.BrushManagement.Border));
                    break;
                case BrushTargets.Foreground:
                    this.BrushManagement.Foreground = brush;

                    this.OnPropertyChanged(nameof(this.BrushManagement.Foreground));
                    break;
                case BrushTargets.Highlight:
                    this.BrushManagement.Highlight = brush;

                    this.OnPropertyChanged(nameof(this.BrushManagement.Highlight));
                    break;
                default:
                    break;
            }

            this.OnPropertyChanged(nameof(this.BrushManagement));

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void ApplyData(EboardDataSet eboardDataSet)
    {
        this.BorderManagement = new BorderManagement(eboardDataSet.BorderDataSet);
        this.BrushManagement = new BrushManagement(eboardDataSet.BrushDataSet);

        this.Elements = eboardDataSet.Elements;

        this.EBoardName = eboardDataSet.EBoardName;
        this.EBoardDepth = eboardDataSet.EBoardDepth;

        this.eSID = eboardDataSet.EBID;

        this.CornerRadiusValue = (int)this.BorderManagement.CornerRadius.TopLeft;
        this.Height = (int)this.BorderManagement.Height;
        this.Width = (int)this.BorderManagement.Width;

        if (this.eSID == null || this.eSID.Equals("-1"))
        {
            DateTime dateTime = DateTime.Now;

            this.eSID = $"EBoard_{dateTime.Ticks}";
        }

        this.ApplyBrush(this.BrushManagement.Background, BrushTargets.Background);
        this.ApplyBrush(this.BrushManagement.Foreground, BrushTargets.Foreground);
        this.ApplyBrush(this.BrushManagement.Border, BrushTargets.Border);
        this.ApplyBrush(this.BrushManagement.Highlight, BrushTargets.Highlight);

        var helper = new SharedMethod_UI();

        this.BackgroundBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Background, this.SetColorValueAsBackground);
        this.ForegroundBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Foreground, this.SetColorValueAsForeground);
        this.BorderBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Border, this.SetColorValueAsBorder);
        this.HighlightBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Highlight, this.SetColorValueAsHighlight);

        this.CornerRadiusQuadSetup = helper.BuildQuadValueSetup(
                new QuadValue<int>()
                {
                    Value1 = (int)this.BorderManagement.CornerRadius.TopLeft,
                    Value2 = (int)this.BorderManagement.CornerRadius.TopRight,
                    Value3 = (int)this.BorderManagement.CornerRadius.BottomRight,
                    Value4 = (int)this.BorderManagement.CornerRadius.BottomLeft,
                },
                this.ResetCorners,
                this.BrushManagement);

        this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged;

        this.PaddingQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManagement.Padding.Left,
                Value2 = (int)this.BorderManagement.Padding.Top,
                Value3 = (int)this.BorderManagement.Padding.Right,
                Value4 = (int)this.BorderManagement.Padding.Bottom,
            },
            this.ResetPadding,
            this.BrushManagement);

        this.PaddingQuadSetup.PropertyChanged += this.PaddingQuadSetup_PropertyChanged;

        this.MarginQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManagement.Margin.Left,
                Value2 = (int)this.BorderManagement.Margin.Top,
                Value3 = (int)this.BorderManagement.Margin.Right,
                Value4 = (int)this.BorderManagement.Margin.Bottom,
            },
            this.ResetThickness,
            this.BrushManagement);

        this.MarginQuadSetup.PropertyChanged += this.MarginQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup = helper.BuildQuadValueSetup(
            new QuadValue<int>()
            {
                Value1 = (int)this.BorderManagement.BorderThickness.Left,
                Value2 = (int)this.BorderManagement.BorderThickness.Top,
                Value3 = (int)this.BorderManagement.BorderThickness.Right,
                Value4 = (int)this.BorderManagement.BorderThickness.Bottom,
            },
            this.ResetMargin,
            this.BrushManagement);

        this.ThicknessQuadSetup.PropertyChanged += this.ThicknessQuadSetup_PropertyChanged;
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

    private void CornerRadiusQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.CornerRadius = new CornerRadius(
            this.CornerRadiusQuadSetup.QuadValue.Value1,
            this.CornerRadiusQuadSetup.QuadValue.Value2,
            this.CornerRadiusQuadSetup.QuadValue.Value3,
            this.CornerRadiusQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
    }

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManagement.CornerRadius = new CornerRadius(value);
    }

    partial void OnHeightValueChanged(int value)
    {
        if (this.BorderManagement.Height != value)
        {
            this.BorderManagement.Height = value;
        }

        OnPropertyChanged(nameof(BorderManagement));
        OnPropertyChanged(nameof(BorderManagement.Height));
    }

    partial void OnEBoardDepthChanged(int value)
    {
        UpdateElementsZIndexProperties(value);
    }

    partial void OnHeightChanged(int value)
    {
        BorderManagement.Height = value;
    }

    partial void OnImageBorderPathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Border, value);
    }

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    partial void OnWidthChanged(int value)
    {
        BorderManagement.Width = value;
    }

    partial void OnWidthValueChanged(int value)
    {
        if (this.BorderManagement.Width != value)
        {
            this.BorderManagement.Width = value;
        }

        OnPropertyChanged(nameof(BorderManagement));
        OnPropertyChanged(nameof(BorderManagement.Width));
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    private void MarginQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.Margin = new Thickness(
            this.MarginQuadSetup.QuadValue.Value1,
            this.MarginQuadSetup.QuadValue.Value2,
            this.MarginQuadSetup.QuadValue.Value3,
            this.MarginQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
    }

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.Padding = new Thickness(
            this.PaddingQuadSetup.QuadValue.Value1,
            this.PaddingQuadSetup.QuadValue.Value2,
            this.PaddingQuadSetup.QuadValue.Value3,
            this.PaddingQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
    }

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
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
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManagement.Background, path);
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManagement.Border, path);
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

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void ResetImageBorder()
    {
        this.ImageBorderPath = string.Empty;

        this.ApplyBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, BrushTargets.Border);
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

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void ResetPadding()
    {
        this.PaddingQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void ResetSize()
    {
        this.BorderManagement.Width = double.NaN;
        this.BorderManagement.Height = double.NaN;

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.BorderManagement));
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

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void SetImageBorder()
    {
        this.ImageBorderPath = new SharedMethod_UI().SetBackgroundImage(this.ImageBorderPath);

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    private void SetElementSizeDisplayValue()
    {
        this.widthValue = new SharedMethod_UI().ResetSizeDisplayValue(this.BorderManagement.Width);
        this.heightValue = new SharedMethod_UI().ResetSizeDisplayValue(this.BorderManagement.Height);

        this.OnPropertyChanged(nameof(this.WidthValue));
        this.OnPropertyChanged(nameof(this.HeightValue));
    }

    [RelayCommand]
    public void SwitchToEboard(object? parameter)
    {
        string? commandParameter = parameter as string;

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