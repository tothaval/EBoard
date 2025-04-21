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

namespace EBoard.ViewModels;

public partial class MainViewModel : ObservableObject, IElementBackgroundImage
{

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
    #endregion


    public MainViewModel(NavigationStore navigationStore, EboardConfig eboardConfig)
    {
        _navigationStore = navigationStore;

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        eBoardBrowserViewModel = new EBoardBrowserViewModel(_navigationStore);

        MainWindowMenuBarVM = new MainWindowMenuBarViewModel(this, eboardConfig);

            BorderManager = new BorderManagement(eboardConfig.BorderDataSet);
            BrushManager = new BrushManagement(eboardConfig.BrushDataSet);
            PlacementManager = new PlacementManagement(eboardConfig.PlacementDataSet);
        
        CornerRadiusValue = (int)BorderManager.CornerRadius.TopLeft;
    }


    // Methods
    /// <summary>
    /// Methods region holds all non async private and public methods
    /// within the viewmodel, ordered alphabetically
    /// </summary>
    #region Methods


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
    private void Maximize()
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
    private void Minimize()
    {
        var mainwindow = Application.Current.MainWindow;

        new SharedMethod_UI().MinimizeApplication(mainwindow);
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