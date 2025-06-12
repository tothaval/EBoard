// <copyright file="MainViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainViewModel
 *
 *  view model for MainWindow
 */
namespace EBoardSDK;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.Windows;
using System.Windows.Media;

public partial class MainViewModel : ObservableObject, IElementBackgroundImage
{
    [ObservableProperty]
    private BorderManagement borderManagement;

    [ObservableProperty]
    private BrushManagement brushManagement;

    [ObservableProperty]
    private EBoardBrowserViewModel eBoardBrowserViewModel;

    private EBoardSDK.Models.EboardConfig eboardConfig;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int cornerRadiusValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int height;

    [ObservableProperty]
    private int heightValue;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private MainWindowLogoutBarViewModel mainWindowLogoutBarVM;

    [ObservableProperty]
    private MainWindowMenuBarViewModel mainWindowMenuBarVM;

    [ObservableProperty]
    private PlacementManagement placementManager;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private int widthValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManagement))]
    private int width;

    public MainViewModel(EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.title = "EBoard";

        this.eboardConfig = eboardConfig;

        this.eBoardBrowserViewModel = new EBoardBrowserViewModel(this);

        this.MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this, eboardConfig);
        this.MainWindowLogoutBarVM = new MainWindowLogoutBarViewModel(this.EBoardBrowserViewModel.BrushManagement);

        this.BorderManagement = new BorderManagement(eboardConfig.BorderDataSet);
        this.BrushManagement = new BrushManagement(eboardConfig.BrushDataSet);
        this.PlacementManager = new PlacementManagement(eboardConfig.PlacementDataSet);

        this.CornerRadiusValue = (int)this.BorderManagement.CornerRadius.TopLeft;

        this.brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

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

        this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged; ;

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

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public EboardConfig EBoardConfig => this.eboardConfig;

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

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

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        this.SetImageToBrushTarget(brushTargets, path);
    }

    public void DeselectElements()
    {
        if (this.EBoardBrowserViewModel.SelectedEBoard != null)
        {
            this.EBoardBrowserViewModel.SelectedEBoard.DeselectElements();
        }
    }

    public EBoardSDK.Models.EboardConfig GetEboardConfig()
    {
        this.eboardConfig.EBoardIndex = this.EBoardBrowserViewModel.CurrentSelectionID;
        this.eboardConfig.EBoardCount = this.EBoardBrowserViewModel.EBoardCount;
        this.eboardConfig.EBoardBrowserSwitch = this.MainWindowMenuBarVM.EBoardBrowserSwitch;

        this.eboardConfig.EBVBorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(this.EBoardBrowserViewModel.BorderManagement);
        this.eboardConfig.EBVBrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(this.EBoardBrowserViewModel.BrushManagement);

        this.eboardConfig.BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(this.BorderManagement);
        this.eboardConfig.BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(this.BrushManagement);
        this.eboardConfig.PlacementDataSet = new EBoardSDK.Models.DataSets.PlacementDataSet(this.PlacementManager);

        return this.eboardConfig;
    }

    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        var screenDataList = this.EBoardBrowserViewModel.GetScreenData();

        return screenDataList;
    }

    public EBoardTaskResult SetScreenData(IList<EBoardSDK.Models.EboardScreen> eboardScreens)
    {
        eboardScreens.Select(x => x).ToList().ForEach(
            escreen =>
            {
                var eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(escreen, this);

                if (escreen.ID == this.eboardConfig.EBoardIndex)
                {
                    this.EBoardBrowserViewModel.SelectedEBoard = eBoardViewModel;
                }

                this.EBoardBrowserViewModel.AddEBoardViewModel(eBoardViewModel);
            });

        return EBoardTaskResult.Success;
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
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

    [RelayCommand]
    private void FirstEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("First");
    }

    [RelayCommand]
    private void LastEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Last");
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

    [RelayCommand]
    private void NextEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Next");
    }

    partial void OnCornerRadiusValueChanged(int value)
    {
        BorderManagement.CornerRadius = new CornerRadius(value);
    }

    partial void OnHeightChanged(int value)
    {
        BorderManagement.Height = value;
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

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.Padding = new Thickness(
            this.PaddingQuadSetup.QuadValue.Value1,
            this.PaddingQuadSetup.QuadValue.Value2,
            this.PaddingQuadSetup.QuadValue.Value3,
            this.PaddingQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
    }

    [RelayCommand]
    private void PrevEboard()
    {
        this.EBoardBrowserViewModel?.SelectedEBoard?.SwitchToEboard("Prev");
    }

    [RelayCommand]
    private void ResetCorners()
    {
        this.CornerRadiusQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManagement.CornerRadius));
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

        this.OnPropertyChanged(nameof(this.BorderManagement.Margin));
    }

    [RelayCommand]
    private void ResetPadding()
    {
        this.PaddingQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManagement.Padding));
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
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManagement.BorderThickness));
    }

    [RelayCommand]
    private void SetColorValueAsBackground()
    {
        this.ApplyBrush(this.BackgroundBrushSetup.ColorBrush, BrushTargets.Background);
    }

    [RelayCommand]
    private void SetColorValueAsBorder()
    {
        this.ApplyBrush(this.BorderBrushSetup.ColorBrush, BrushTargets.Border);
    }

    [RelayCommand]
    private void SetColorValueAsForeground()
    {
        this.ApplyBrush(this.ForegroundBrushSetup.ColorBrush, BrushTargets.Foreground);
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

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManagement));
    }
}

// EOF