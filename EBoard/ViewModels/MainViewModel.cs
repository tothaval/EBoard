// <copyright file="MainViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainViewModel
 *
 *  view model for MainWindow
 */
namespace EBoard.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Utilities.Factories;
using EBoardSDK;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using System.Windows;
using System.Windows.Media;

public partial class MainViewModel : ObservableObject, IElementBackgroundImage
{
    [ObservableProperty]
    private BorderManagement borderManager;

    [ObservableProperty]
    private BrushManagement brushManager;

    [ObservableProperty]
    private EBoardBrowserViewModel eBoardBrowserViewModel;

    private EBoardSDK.Models.EboardConfig eboardConfig;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int cornerRadiusValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int height;

    [ObservableProperty]
    private int heightValue;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private MainWindowMenuBarViewModel mainWindowMenuBarVM;

    [ObservableProperty]
    private PlacementManagement placementManager;

    [ObservableProperty]
    private int widthValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int width;

    public MainViewModel(EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this.eboardConfig = eboardConfig;

        this.eBoardBrowserViewModel = new EBoardBrowserViewModel(this);

        this.MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this, eboardConfig);

        this.BorderManager = new BorderManagement(eboardConfig.BorderDataSet);
        this.BrushManager = new BrushManagement(eboardConfig.BrushDataSet);
        this.PlacementManager = new PlacementManagement(eboardConfig.PlacementDataSet);

        this.CornerRadiusValue = (int)this.BorderManager.CornerRadius.TopLeft;

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

        if (this.BackgroundBrushSetup == null)
        {
            this.BackgroundBrushSetup = new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) }, this.SetColorValueAsBackground);
        }

        if (this.BorderBrushSetup == null)
        {
            this.BorderBrushSetup = new SolidColorBrushSetupViewModel(new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) }, this.SetColorValueAsBorder);
        }

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

        this.eboardConfig.EBVBorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(this.EBoardBrowserViewModel.BorderManager);
        this.eboardConfig.EBVBrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(this.EBoardBrowserViewModel.BrushManager);

        this.eboardConfig.BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(this.BorderManager);
        this.eboardConfig.BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(this.BrushManager);
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
        this.BorderManager.CornerRadius = new CornerRadius(
            this.CornerRadiusQuadSetup.QuadValue.Value1,
            this.CornerRadiusQuadSetup.QuadValue.Value2,
            this.CornerRadiusQuadSetup.QuadValue.Value3,
            this.CornerRadiusQuadSetup.QuadValue.Value4);

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

    partial void OnHeightChanged(int value)
    {
        BorderManager.Height = value;
    }

    partial void OnHeightValueChanged(int value)
    {
        if (this.BorderManager.Height != value)
        {
            this.BorderManager.Height = value;
        }

        OnPropertyChanged(nameof(BorderManager));
        OnPropertyChanged(nameof(BorderManager.Height));
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

    partial void OnWidthValueChanged(int value)
    {
        if (this.BorderManager.Width != value)
        {
            this.BorderManager.Width = value;
        }

        OnPropertyChanged(nameof(BorderManager));
        OnPropertyChanged(nameof(BorderManager.Width));
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

    [RelayCommand]
    private void ResetCorners()
    {
        this.CornerRadiusQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManager.CornerRadius));
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

        this.OnPropertyChanged(nameof(this.BorderManager.Margin));
    }

    [RelayCommand]
    private void ResetPadding()
    {
        this.PaddingQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManager.Padding));
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
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManager.BorderThickness));
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

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManager.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

        this.OnPropertyChanged(nameof(this.BorderManager));
    }
}

// EOF