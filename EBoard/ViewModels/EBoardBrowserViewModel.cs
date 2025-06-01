/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardBrowserViewModel 
 * 
 *  view model class for EBoardBrowserView
 *  
 *  EBoardBrowserView presents the EBoardViewModels currently existent within the application,
 *  as well as functionality to create, edit or delete eboard instances.
 */
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoard.Navigation;
using EBoard.Utilities.Factories;
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

public partial class EBoardBrowserViewModel : ObservableObject, IElementBackgroundImage
{
    // Properties & Fields
    #region Properties & Fields

    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private int currentSelectionID;

    [ObservableProperty]
    private DateTime eBoardCreatedDate;

    [ObservableProperty]
    private int eBoardContainerCount;

    [ObservableProperty]
    private int eBoardElementCount;

    [ObservableProperty]
    private int eBoardShapeCount;

    [ObservableProperty]
    private double newEBoardHeight = 480;

    [ObservableProperty]
    private double newEBoardWidth = 720;

    [ObservableProperty]
    private BorderManagement borderManager;

    [ObservableProperty]
    private BrushManagement brushManager;

    [ObservableProperty]
    private int eBoardCount;

    [ObservableProperty]
    private int _eBoardDepth = 100;

    [ObservableProperty]
    private string eBoardName = "new";

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

    private NavigationStore _NavigationStore;
    //public NavigationStore NavigationStore
    //{
    //    get { return _NavigationStore; }
    //    set
    //    {
    //        _NavigationStore = value;
    //        OnPropertyChanged(nameof(NavigationStore));
    //    }
    //}

    [ObservableProperty]
    private EBoardViewModel selectedEBoard;

    partial void OnSelectedEBoardChanged(EBoardViewModel value)
    {
        if (selectedEBoard != null)
        {
            selectedEBoard.EBoardActive = false; // changing the active value of the old one, should do this in a different way
        }

        selectedEBoard = value;

        if (selectedEBoard != null)
        {
            selectedEBoard.Elements.CollectionChanged += SelectedEBoardElements_CollectionChanged;
        }

        RefreshEBoardParameters();
    }

    private void SelectedEBoardElements_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        this.RefreshEBoardParameters();
    }

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }
    #endregion

    // Collections
    #region Collections

    private ObservableCollection<EBoardViewModel> _eboards;

    public ObservableCollection<EBoardViewModel> EBoards
    {
        get { return this._eboards; }
        set { this._eboards = value; }
    }
    #endregion

    // Constructors
    #region Constructors

    public EBoardBrowserViewModel() => this.InstantiateProperties();

    public EBoardBrowserViewModel(NavigationStore navigationStore, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.InstantiateProperties(navigationStore);
    }

    #endregion

    // Methods
    #region Methods

    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        IList<EBoardSDK.Models.EboardScreen> eboardScreens = [];

        this.EBoards.Select(x => x).ToList().ForEach(
            escreen =>
            {
                IList<ElementConfig> elementConfigs = [];

                escreen.Elements.Select(x => x).ToList().ForEach(
                    element =>
                    {
                        if (element.Plugin == null)
                        {
                            return;
                        }

                        elementConfigs.Add(new ElementConfig()
                        {
                            EID = element.EID,
                            ID = escreen.Elements.IndexOf(element),
                            PluginHeader = element.Plugin.PluginHeader,
                            PluginName = element.Plugin.PluginName,
                            Plugin = element.Plugin,
                            PluginType = element.Plugin.GetType().FullName,
                            AssemblyName = element.Plugin.GetType().AssemblyQualifiedName,

                            BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(element.Plugin.BorderManagement),
                            BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(element.Plugin.BrushManagement),
                            PlacementDataSet = new EBoardSDK.Models.DataSets.PlacementDataSet(element.PlacementManager),
                        });
                    });

                eboardScreens.Add(new EboardScreen()
                {
                    BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(escreen.BorderManager),
                    BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(escreen.BrushManager),
                    EBID = escreen.EBID,
                    ID = this.EBoards.IndexOf(escreen),
                    EBoardDepth = escreen.EBoardDepth,
                    EBoardName = escreen.EBoardName,

                    Elements = elementConfigs,
                });
            });

        return eboardScreens;
    }

    [RelayCommand]
    private void AddEBoard()
    {
        if (this.EBoardName == null)
        {
            this.EBoardName = "";
        }

        var eboardScreen = EBoardFactory.GetNewEboardScreen(this.EBoardName, this.EBoardDepth, this.NewEBoardWidth, this.newEBoardHeight);

        EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(eboardScreen, this.mainViewModel);

        this.EBoards.Add(eBoardViewModel);

        this.SelectedEBoard = this.EBoards.Last();
        this.RefreshEBoardParameters();
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

                this.OnPropertyChanged(nameof(this.BrushManager));
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManager.Border, path);

                this.OnPropertyChanged(nameof(this.BrushManager));
                this.OnPropertyChanged(nameof(this.BrushManager.Border));
                break;
            case BrushTargets.Foreground:
            case BrushTargets.Highlight:
                break;
            default:
                break;
        }

        return this.ApplyBrush(brush, brushTargets);
    }

    public Task AddEBoardViewModel(EBoardViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            this.EBoards.Add(eBoardViewModel);
        }

        return Task.CompletedTask;
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        if (this.SelectedEBoard != null && this.EBoards.Count > 0)
        {
            this.EBoards.Remove(this.SelectedEBoard);

            if (this.EBoards.Count > 0)
            {
                this.SelectedEBoard = this.EBoards.Last();
                this.RefreshEBoardParameters();

                this.OnPropertyChanged(nameof(this.EBoards));
            }
            else
            {
                this._NavigationStore.CurrentViewModel = null;
            }
        }
    }

    [RelayCommand]
    private void EditEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this.SelectedEBoard.EBoardName = this.EBoardName;
            this.SelectedEBoard.EBoardDepth = this.EBoardDepth;
            this.SelectedEBoard.Width = (int)this.NewEBoardWidth;
            this.SelectedEBoard.Height = (int)this.NewEBoardHeight;
        }
    }

    private void InstantiateProperties()
    {
        this.BorderManager = new BorderManagement();
        this.BrushManager = new BrushManagement();

        if (this.mainViewModel?.EBoardConfig?.EBVBorderDataSet != null)
        {
            this.BorderManager = new BorderManagement(this.mainViewModel.EBoardConfig.EBVBorderDataSet);
        }

        if (this.mainViewModel?.EBoardConfig?.EBVBrushDataSet != null)
        {
            this.BrushManager = new BrushManagement(this.mainViewModel.EBoardConfig.EBVBrushDataSet);
        }

        this.EBoards = new ObservableCollection<EBoardViewModel>();

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

        this.ApplyBrush(this.BrushManager.Background, BrushTargets.Background);
        this.ApplyBrush(this.BrushManager.Foreground, BrushTargets.Foreground);
        this.ApplyBrush(this.BrushManager.Border, BrushTargets.Border);
        this.ApplyBrush(this.BrushManager.Highlight, BrushTargets.Highlight);

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

    private void InstantiateProperties(NavigationStore navigationStore)
    {
        this._NavigationStore = navigationStore;

        this.InstantiateProperties();

        if (this.EBoards.Count > 0)
        {
            navigationStore.CurrentViewModel = this.EBoards[0];
        }
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

    //[RelayCommand]
    //private void ResetSize()
    //{
    //    Plugin.BorderManagement.Width = double.NaN;
    //    Plugin.BorderManagement.Height = double.NaN;

    //    this.SetElementSizeDisplayValue();

    //    OnPropertyChanged(nameof(Plugin));
    //}

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

                    var s = Application.Current.FindResource("EBoardEboardBrowserBrushBackground");

                    Application.Current.Resources["EBoardEboardBrowserBrushBackground"] = brush;

                    //s = brush;

                    this.OnPropertyChanged(nameof(this.BrushManager.Background));
                    break;

                case BrushTargets.Border:
                    this.BrushManager.Border = brush;

                    Application.Current.Resources["EBoardEboardBrowserBrushBorder"] = brush;

                    this.OnPropertyChanged(nameof(this.BrushManager.Border));
                    break;
                case BrushTargets.Foreground:
                    this.BrushManager.Foreground = brush;

                    Application.Current.Resources["EBoardEboardBrowserBrushForeground"] = brush;

                    this.OnPropertyChanged(nameof(this.BrushManager.Foreground));
                    break;
                case BrushTargets.Highlight:
                    this.BrushManager.Highlight = brush;

                    Application.Current.Resources["EBoardEboardBrowserBrushHighlight"] = brush;

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

    private void RefreshEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this._NavigationStore.CurrentViewModel = this.SelectedEBoard;

            this.EBoardName = this.SelectedEBoard.EBoardName;
            this.EBoardDepth = this.SelectedEBoard.EBoardDepth;
            this.NewEBoardHeight = this.SelectedEBoard.BorderManager.Height;
            this.NewEBoardWidth = this.SelectedEBoard.BorderManager.Width;
            this.SelectedEBoard.EBoardActive = true;

            this.CurrentSelectionID = this.EBoards.IndexOf(this.SelectedEBoard) + 1;
            this.EBoardContainerCount = this.SelectedEBoard.GetContainerCount();
            this.EBoardCount = this.EBoards.Count;
            this.EBoardCreatedDate = this.SelectedEBoard.GetCreatedDate();
            this.EBoardElementCount = this.SelectedEBoard.GetElementCount();
            this.EBoardShapeCount = this.SelectedEBoard.GetShapeCount();
        }
        else
        {
            this.EBoardName = "no board selected";
            this.EBoardDepth = 10;
            this.NewEBoardHeight = 620;
            this.NewEBoardWidth = 1240;

            this.CurrentSelectionID = 0;
            this.EBoardContainerCount = 0;
            this.EBoardCount = 0;
            this.EBoardCreatedDate = DateTime.Now;
            this.EBoardElementCount = 0;
            this.EBoardShapeCount = 0;
        }
    }

    public void RemoveSelectedEBoard()
    {
        this.EBoards.Remove(this.SelectedEBoard);

        if (this.EBoards.Count > 0)
        {
            this.SelectedEBoard = this.EBoards.Last();
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

    internal void DeleteAllElements()
    {
        this.SelectedEBoard?.Clear();
    }

    #endregion

}
// EOF