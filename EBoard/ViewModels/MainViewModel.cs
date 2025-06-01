/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel 
 *  
 *  view model for MainWindow
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Navigation;
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

namespace EBoard.ViewModels;

public partial class MainViewModel : ObservableObject, IElementBackgroundImage
{
    private EBoardSDK.Models.EboardConfig eboardConfig;

    public EboardConfig EBoardConfig => this.eboardConfig;

    private readonly NavigationStore _navigationStore;

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

    public ObservableObject CurrentViewModel => this._navigationStore.CurrentViewModel;

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
    private string imagePath;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    [ObservableProperty]
    private string imageBorderPath;

    partial void OnImageBorderPathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Border, value);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int height;

    partial void OnHeightChanged(int value)
    {
        BorderManager.Height = value;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BorderManager))]
    private int width;

    partial void OnWidthChanged(int value)
    {
        BorderManager.Width = value;
    }

    [ObservableProperty]
    private PlacementManagement placementManager;

    [ObservableProperty]
    private EBoardBrowserViewModel eBoardBrowserViewModel;

    [ObservableProperty]
    private MainWindowMenuBarViewModel mainWindowMenuBarVM;

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public MainViewModel(NavigationStore navigationStore, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        this._navigationStore = navigationStore;

        this._navigationStore.CurrentViewModelChanged += this.OnCurrentViewModelChanged;

        this.eboardConfig = eboardConfig;

        this.eBoardBrowserViewModel = new EBoardBrowserViewModel(this._navigationStore, this);

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

        //BMarginValue = (int)elementDataSet.BorderDataSet.Margin.Left;

        //BPaddingValue = (int)elementDataSet.BorderDataSet.Padding.Left;

        //BThicknessValue = (int)BorderManager.BorderThickness.Left;

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
    private void ResetThickness()
    {
        this.ThicknessQuadSetup.All = 0;

        this.OnPropertyChanged(nameof(this.BorderManager.BorderThickness));
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

    // Methods
    /// <summary>
    /// Methods region holds all non async private and public methods
    /// within the viewmodel, ordered alphabetically
    /// </summary>
    #region Methods

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

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        this.SetImageToBrushTarget(brushTargets, path);
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

        return (bool)(this.ApplyBrush(brush, brushTargets));
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    internal void DeselectElements()
    {
        if (this.eBoardBrowserViewModel.SelectedEBoard != null)
        {
            this.eBoardBrowserViewModel.SelectedEBoard.DeselectElements();
        }
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

    #endregion

    // Events
    /// <summary>
    /// Events region holds all event methods within the viewmodel
    /// alphabetically ordered
    /// </summary>
    #region Events
    private void OnCurrentViewModelChanged()
    {
        this.OnPropertyChanged(nameof(this.CurrentViewModel));

        this.OnPropertyChanged(nameof(this.BrushManager));
        this.OnPropertyChanged(nameof(this.BrushManager.Background));
        this.OnPropertyChanged(nameof(this.BrushManager.Border));

        this.OnPropertyChanged(nameof(this.BorderManager));
        this.OnPropertyChanged(nameof(this.BorderManager.CornerRadius));
        this.OnPropertyChanged(nameof(this.BorderManager.BorderThickness));
    }
    #endregion

}
// EOF