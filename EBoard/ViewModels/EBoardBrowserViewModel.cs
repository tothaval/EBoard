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
using EBoard.Models;
using EBoard.Navigation;
using EBoard.Utilities.Factories;

using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using System.Collections.ObjectModel;
using System.Windows.Media;
using EBoardSDK.Controls;
using EBoardSDK.Enums;

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
    private string imagePath = string.Empty;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage();
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
        RefreshEBoardParameters();
    }


    public SolidColorBrushSetupViewModel BackgroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel ForegroundBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel BorderBrushSetup { get; set; }
    public SolidColorBrushSetupViewModel HighlightBrushSetup { get; set; }
    #endregion

    // Collections
    #region Collections

    private ObservableCollection<EBoardViewModel> _eboards;
    public ObservableCollection<EBoardViewModel> EBoards
    {
        get { return _eboards; }
        set { _eboards = value; }
    }
    #endregion

    // Constructors
    #region Constructors

    public EBoardBrowserViewModel() => InstantiateProperties();

    public EBoardBrowserViewModel(NavigationStore navigationStore, MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        InstantiateProperties(navigationStore);
    }

    #endregion

    // Methods
    #region Methods

    public IList<EBoardSDK.Models.EboardScreen> GetScreenData()
    {
        IList<EBoardSDK.Models.EboardScreen> eboardScreens = [];

        EBoards.Select(x => x).ToList().ForEach(
            escreen =>
            {
                IList<ElementConfig> elementConfigs = [];

                escreen.Elements.Select(x => x).ToList().ForEach(
                    element =>
                    {
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
                    ID = EBoards.IndexOf(escreen),
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
        if (EBoardName == null)
        {
            EBoardName = "";
        }

        var eboardScreen = EBoardFactory.GetNewEboardScreen(EBoardName, EBoardDepth, NewEBoardWidth, newEBoardHeight);

        EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(eboardScreen, mainViewModel);

        EBoards.Add(eBoardViewModel);

        SelectedEBoard = EBoards.Last();
        RefreshEBoardParameters();
    }

    public void ChangeElementBackgroundToImage()
    {
        BrushManager.Background = new SharedMethod_UI().ChangeBackgroundToImage(BrushManager.Background, ImagePath);

        if (ImagePath.Equals(string.Empty))
        {
            BrushManager.Background = new SolidColorBrush(Colors.White);
        }

        OnPropertyChanged(nameof(BrushManager));

        OnPropertyChanged(nameof(BrushManager.Background));
    }


    public Task AddEBoardViewModel(EBoardViewModel eBoardViewModel)
    {
        if (eBoardViewModel != null)
        {
            EBoards.Add(eBoardViewModel);
        }

        return Task.CompletedTask;
    }


    [RelayCommand]
    private void DeleteEBoard()
    {
        if (SelectedEBoard != null && EBoards.Count > 0)
        {
            EBoards.Remove(SelectedEBoard);

            if (EBoards.Count > 0)
            {
                SelectedEBoard = EBoards.Last();
                RefreshEBoardParameters();

                OnPropertyChanged(nameof(EBoards));
            }
            else
            {
                _NavigationStore.CurrentViewModel = null;
            }
        }
    }


    [RelayCommand]
    private void EditEBoardParameters()
    {
        if (SelectedEBoard != null)
        {
            SelectedEBoard.EBoardName = EBoardName;
            SelectedEBoard.EBoardDepth = EBoardDepth;
            SelectedEBoard.Width = (int)NewEBoardWidth;
            SelectedEBoard.Height = (int)NewEBoardHeight;
        }
    }


    private void InstantiateProperties()
    {
        BrushManager = new BrushManagement();
        BrushManager.Background = new SolidColorBrush(Colors.White);
        BrushManager.Foreground = new SolidColorBrush(Colors.Orange);
        BrushManager.Border = new SolidColorBrush(Colors.Blue);
        BrushManager.Highlight = new SolidColorBrush(Colors.Red);

        if (mainViewModel?.EBoardConfig?.EBVBorderDataSet != null)
        {
            BorderManager = new BorderManagement(mainViewModel.EBoardConfig.EBVBorderDataSet);
        }

        if (mainViewModel?.EBoardConfig?.EBVBrushDataSet != null)
        {
            BrushManager = new BrushManagement(mainViewModel.EBoardConfig.EBVBrushDataSet);
        }
        
        EBoards = new ObservableCollection<EBoardViewModel>();

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

    private void InstantiateProperties(NavigationStore navigationStore)
    {
        _NavigationStore = navigationStore;

        InstantiateProperties();

        if (EBoards.Count > 0)
        {
            navigationStore.CurrentViewModel = EBoards[0];
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

    private void RefreshEBoardParameters()
    {
        if (SelectedEBoard != null)
        {
            _NavigationStore.CurrentViewModel = SelectedEBoard;

            EBoardName = SelectedEBoard.EBoardName;
            EBoardDepth = SelectedEBoard.EBoardDepth;
            NewEBoardHeight = SelectedEBoard.BorderManager.Height;
            NewEBoardWidth = SelectedEBoard.BorderManager.Width;
            SelectedEBoard.EBoardActive = true;

            CurrentSelectionID = EBoards.IndexOf(SelectedEBoard) + 1;
            EBoardContainerCount = SelectedEBoard.GetContainerCount();
            EBoardCount = EBoards.Count;
            EBoardCreatedDate = SelectedEBoard.GetCreatedDate();
            EBoardElementCount = SelectedEBoard.GetElementCount();
            EBoardShapeCount = SelectedEBoard.GetShapeCount();
        }
        else
        {
            EBoardName = "no board selected";
            EBoardDepth = 10;
            NewEBoardHeight = 620;
            NewEBoardWidth = 1240;

            CurrentSelectionID = 0;
            EBoardContainerCount = 0;
            EBoardCount = 0;
            EBoardCreatedDate = DateTime.Now;
            EBoardElementCount = 0;
            EBoardShapeCount = 0;
        }

    }


    public void RemoveSelectedEBoard()
    {
        EBoards.Remove(SelectedEBoard);

        if (EBoards.Count > 0)
        {
            SelectedEBoard = EBoards.Last();
        }
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


    #endregion

}
// EOF