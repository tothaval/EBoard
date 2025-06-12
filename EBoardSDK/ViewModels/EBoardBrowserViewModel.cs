// <copyright file="EBoardBrowserViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  EBoardBrowserViewModel
 *
 *  view model class for EBoardBrowserView
 *
 *  EBoardBrowserView presents the EBoardViewModels currently existent within the application,
 *  as well as functionality to create, edit or delete eboard instances.
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.ViewModels;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities.Factories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

public partial class EBoardBrowserViewModel : ObservableObject, IElementBackgroundImage
{
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
    private BorderManagement borderManagement;

    [ObservableProperty]
    private BrushManagement brushManagement;

    [ObservableProperty]
    private int eBoardCount;

    [ObservableProperty]
    private int eBoardDepth = 100;

    [ObservableProperty]
    private string eBoardName = "new";

    private ObservableCollection<EBoardViewModel> eboards;

    [ObservableProperty]
    private string imagePath;

    [ObservableProperty]
    private string imageBorderPath;

    [ObservableProperty]
    private EBoardViewModel selectedEBoard;

    public EBoardBrowserViewModel() => this.InstantiateProperties();

    public EBoardBrowserViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.InstantiateProperties();
    }

    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }

    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public ObservableCollection<EBoardViewModel> EBoards
    {
        get { return this.eboards; }

        set
        {
            this.eboards = value;

            if (this.eboards.Count == 0)
            {
                this.eboards.Clear();
            }
        }
    }

    private static string TxtRemoveAllEboardScreensQuestion => "Delete all screens?";

    private static string TxtRemoveEboardQuestion => "Remove Screen?";

    private static string TxtRemoveEboardTitle => "Screen Deletion";

    public Task AddEBoardViewModel(EBoardViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            this.EBoards.Add(eBoardViewModel);
        }

        return Task.CompletedTask;
    }

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        this.SetImageToBrushTarget(brushTargets, path);
    }

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
                    BorderDataSet = new EBoardSDK.Models.DataSets.BorderDataSet(escreen.BorderManagement),
                    BrushDataSet = new EBoardSDK.Models.DataSets.BrushDataSet(escreen.BrushManagement),
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

        var eboardScreen = EBoardFactory.GetNewEboardScreen(this.EBoardName, this.EBoardDepth, this.NewEBoardWidth, this.NewEBoardHeight);

        EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(eboardScreen, this.mainViewModel);

        this.EBoards.Add(eBoardViewModel);

        this.SelectedEBoard = this.EBoards.Last();
        this.RefreshEBoardParameters();
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

                this.OnPropertyChanged(nameof(this.BrushManagement));
                break;
            case BrushTargets.Border:
                brush = new SharedMethod_UI().ChangeBackgroundToImage(this.BrushManagement.Border, path);

                this.OnPropertyChanged(nameof(this.BrushManagement));
                this.OnPropertyChanged(nameof(this.BrushManagement.Border));
                break;
            case BrushTargets.Foreground:
            case BrushTargets.Highlight:
                break;
            default:
                break;
        }

        return this.ApplyBrush(brush, brushTargets);
    }

    partial void OnImageBorderPathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Border, value);
    }

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    partial void OnSelectedEBoardChanging(EBoardViewModel? oldValue, EBoardViewModel newValue)
    {
        if (oldValue == null || oldValue == newValue || oldValue.Equals(newValue))
        {
            return;
        }

        oldValue.EBoardActive = false;

        oldValue.Elements.CollectionChanged -= SelectedEBoardElements_CollectionChanged;

        if (newValue == null)
        {
            return;
        }

        newValue.Elements.CollectionChanged += SelectedEBoardElements_CollectionChanged;
    }

    partial void OnSelectedEBoardChanged(EBoardViewModel value)
    {
        if (value == null)
        {
            return;
        }

        RefreshEBoardParameters();
    }

    private void SelectedEBoardElements_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (this.SelectedEBoard.Elements.Count != this.EBoardContainerCount)
        {
            this.RefreshSelectedEboardElementData();
        }
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        if (this.SelectedEBoard != null && this.EBoards.Count > 0)
        {
            this.RemoveSelectedEBoard();

            if (this.EBoards.Count > 0)
            {
                this.RefreshEBoardParameters();

                this.OnPropertyChanged(nameof(this.EBoards));
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
        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();

        if (this.mainViewModel?.EBoardConfig?.EBVBorderDataSet != null)
        {
            this.BorderManagement = new BorderManagement(this.mainViewModel.EBoardConfig.EBVBorderDataSet);
        }

        if (this.mainViewModel?.EBoardConfig?.EBVBrushDataSet != null)
        {
            this.BrushManagement = new BrushManagement(this.mainViewModel.EBoardConfig.EBVBrushDataSet);
        }

        this.EBoards = new ObservableCollection<EBoardViewModel>();

        var helper = new SharedMethod_UI();

        this.BackgroundBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Background, this.SetColorValueAsBackground);
        this.ForegroundBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Foreground, this.SetColorValueAsForeground);
        this.BorderBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Border, this.SetColorValueAsBorder);
        this.HighlightBrushSetup = helper.BuildSolidColorBrushSetup(this.BrushManagement, BrushTargets.Highlight, this.SetColorValueAsHighlight);

        this.ApplyBrush(this.BrushManagement.Background, BrushTargets.Background);
        this.ApplyBrush(this.BrushManagement.Foreground, BrushTargets.Foreground);
        this.ApplyBrush(this.BrushManagement.Border, BrushTargets.Border);
        this.ApplyBrush(this.BrushManagement.Highlight, BrushTargets.Highlight);

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

        this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
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

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.BorderManagement.BorderThickness = new Thickness(
            this.ThicknessQuadSetup.QuadValue.Value1,
            this.ThicknessQuadSetup.QuadValue.Value2,
            this.ThicknessQuadSetup.QuadValue.Value3,
            this.ThicknessQuadSetup.QuadValue.Value4);

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

    [RelayCommand]
    private void DeleteAllScreens()
    {
        string question = TxtRemoveAllEboardScreensQuestion;
        string title = TxtRemoveEboardTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.SelectedEBoard?.Dispose();
        this.EBoards?.Clear();
    }

    public void DeleteAllElements()
    {
        this.SelectedEBoard?.Clear();
    }

    public void RemoveSelectedEBoard()
    {
        string question = TxtRemoveEboardQuestion;
        string title = TxtRemoveEboardTitle;

        MessageBoxResult result = MessageBox.Show(question, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.No)
        {
            return;
        }

        this.SelectedEBoard.Dispose();
        this.EBoards.Remove(this.SelectedEBoard);

        if (this.EBoards.Count > 0 && result == MessageBoxResult.Yes)
        {
            this.SelectedEBoard = this.EBoards.Last();
        }
    }

    private void RefreshEBoardParameters()
    {
        if (this.SelectedEBoard != null)
        {
            this.EBoardName = this.SelectedEBoard.EBoardName;
            this.EBoardDepth = this.SelectedEBoard.EBoardDepth;
            this.NewEBoardHeight = this.SelectedEBoard.BorderManagement.Height;
            this.NewEBoardWidth = this.SelectedEBoard.BorderManagement.Width;
            this.SelectedEBoard.EBoardActive = true;

            this.CurrentSelectionID = this.EBoards.IndexOf(this.SelectedEBoard) + 1;
            this.EBoardCount = this.EBoards.Count;
            this.EBoardCreatedDate = this.SelectedEBoard.GetCreatedDate();

            this.RefreshSelectedEboardElementData();
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

    private void RefreshSelectedEboardElementData()
    {
        this.EBoardContainerCount = this.SelectedEBoard.GetContainerCount();
        this.EBoardElementCount = this.SelectedEBoard.GetElementCount();
        this.EBoardShapeCount = this.SelectedEBoard.GetShapeCount();
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
}

// EOF