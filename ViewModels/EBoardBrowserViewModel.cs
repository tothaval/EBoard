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
using EBoard.Commands;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Navigation;
using EBoard.Utilities.Factories;
using EBoard.Utilities.SharedMethods;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace EBoard.ViewModels;

public partial class EBoardBrowserViewModel : ObservableObject, IElementBackgroundImage
{

    // Properties & Fields
    #region Properties & Fields

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
    private string eBoardName  = "new";


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

    public EBoardBrowserViewModel()
    {

        BorderManager = new BorderManagement();

        BrushManager = new BrushManagement();
        BrushManager.Background = new SolidColorBrush(Colors.White);
        BrushManager.Foreground = new SolidColorBrush(Colors.Orange);
        BrushManager.Border = new SolidColorBrush(Colors.Blue);
        BrushManager.Highlight = new SolidColorBrush(Colors.Red);

        EBoards = new ObservableCollection<EBoardViewModel>(); 
    }


    public EBoardBrowserViewModel(NavigationStore navigationStore)
    {

        _NavigationStore = navigationStore;

        BorderManager = new BorderManagement();

        BrushManager = new BrushManagement();
        BrushManager.Background = new SolidColorBrush(Colors.White);
        BrushManager.Foreground = new SolidColorBrush(Colors.Black);
        BrushManager.Border = new SolidColorBrush(Colors.Blue);
        BrushManager.Highlight = new SolidColorBrush(Colors.Red);

        EBoards = new ObservableCollection<EBoardViewModel>();

        if (EBoards.Count > 0)
        {
            navigationStore.CurrentViewModel = EBoards[0];
        }

    }

    #endregion


    // Methods
    #region Methods

    [RelayCommand]
    private void AddEBoard()
    {
        if (EBoardName == null)
        {
            EBoardName = "";
        }

        EboardDataSet eboardDataSet = EBoardFactory.GetNewEBoardDataSetDefined(EBoardName, EBoardDepth, NewEBoardWidth, newEBoardHeight );

        EBoardViewModel eBoardViewModel = EBoardFactory.GetEBoardViewModelByEBoardDataSet(eboardDataSet);

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