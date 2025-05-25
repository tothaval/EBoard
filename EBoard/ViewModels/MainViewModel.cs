/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  MainViewModel 
 *  
 *  view model for MainWindow
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.IOProcesses.DataSets;
using EBoard.Navigation;

using EBoardSDK.SharedMethods;
using EBoardSDK.Models;
using EBoardSDK.Interfaces;

using System.Windows;
using System.Windows.Media;
using EBoardSDK;
using EBoard.Utilities.Factories;
using EBoardSDK.Controls;
using EBoardSDK.Enums;

namespace EBoard.ViewModels;

public partial class MainViewModel : ObservableObject, IElementBackgroundImage
{
    private EBoardSDK.Models.EboardConfig eboardConfig;
    public EboardConfig EBoardConfig => eboardConfig;

    // Properties & Fields
    #region Properties & Fields
    private readonly NavigationStore _navigationStore;

    public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;

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

    /// <summary>
    /// the path to an optional background image for the
    /// element, if empty, the stored brush or a default
    /// solidColorBrush will be used for the background
    /// </summary>
    [ObservableProperty]
    private string imagePath;

    partial void OnImagePathChanged(string value)
    {
        if (!value.Equals(string.Empty))
        {
            ChangeElementBackgroundToImage();

            return;
        }
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
    #endregion

    public MainViewModel(NavigationStore navigationStore, EBoardSDK.Models.EboardConfig eboardConfig)
    {
        _navigationStore = navigationStore;

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        this.eboardConfig = eboardConfig;

        eBoardBrowserViewModel = new EBoardBrowserViewModel(_navigationStore, this);

        MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this, eboardConfig);

        BorderManager = new BorderManagement(eboardConfig.BorderDataSet);
        BrushManager = new BrushManagement(eboardConfig.BrushDataSet);
        PlacementManager = new PlacementManagement(eboardConfig.PlacementDataSet);

        CornerRadiusValue = (int)BorderManager.CornerRadius.TopLeft;

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

    public EBoardSDK.Models.EboardConfig GetEboardConfig()
    {
        eboardConfig.EBoardIndex = EBoardBrowserViewModel.CurrentSelectionID;
        eboardConfig.EBoardCount = EBoardBrowserViewModel.EBoardCount;
        eboardConfig.EBoardBrowserSwitch = MainWindowMenuBarVM.EBoardBrowserSwitch;

        eboardConfig.EBVBorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(EBoardBrowserViewModel.BorderManager);
        eboardConfig.EBVBrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(EBoardBrowserViewModel.BrushManager);

        eboardConfig.BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(BorderManager);
        eboardConfig.BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(BrushManager);
        eboardConfig.PlacementDataSet = new EBoardSDK.Models.DataSets.PlacementDataSet(PlacementManager);

        return this.eboardConfig;
    }
    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        var screenDataList = EBoardBrowserViewModel.GetScreenData();

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

                if (escreen.ID == eboardConfig.EBoardIndex)
                {
                    EBoardBrowserViewModel.SelectedEBoard = eBoardViewModel;
                }

                EBoardBrowserViewModel.AddEBoardViewModel(eBoardViewModel);
            });

        return EBoardTaskResult.Success;
    }

    public void ChangeElementBackgroundToImage()
    {
        BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    internal void DeselectElements()
    {
        if (eBoardBrowserViewModel.SelectedEBoard != null)
        {
            eBoardBrowserViewModel.SelectedEBoard.DeselectElements();
        }
    }

    [RelayCommand]
    private void LeftDoubleClick()
    {
        var mainwindow = Application.Current.MainWindow;

        new SharedMethod_UI().MaximizeApplication(mainwindow);
    }

    [RelayCommand]
    private void SetImage()
    {
        ImagePath = new SharedMethod_UI().SetBackgroundImage(ImagePath);
    }

    [RelayCommand]
    private void ResetImage()
    {
        ImagePath = string.Empty;
        BrushManager.Background = new SharedMethod_UI().ImagePathErrorDefaultBrush;

        OnPropertyChanged(nameof(BrushManager));
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
        OnPropertyChanged(nameof(CurrentViewModel));

        OnPropertyChanged(nameof(BrushManager));
        OnPropertyChanged(nameof(BrushManager.Background));
        OnPropertyChanged(nameof(BrushManager.Border));

        OnPropertyChanged(nameof(BorderManager));
        OnPropertyChanged(nameof(BorderManager.CornerRadius));
        OnPropertyChanged(nameof(BorderManager.BorderThickness));
    }
    #endregion

}
// EOF