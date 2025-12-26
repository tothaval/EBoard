// <copyright file="EBoardViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
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
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.ScreenIntegration;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.SharedMethods;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

public partial class EBoardViewModel : EboardFluidUIBaseViewModel, IEboardIdentity
{
    private readonly MainViewModel mainViewModel;

    private ScreenControlViewModel eBoardSettingsViewModel;

    [ObservableProperty]
    private bool eBoardActive;

    [ObservableProperty]
    private int eBoardDepth;

    [ObservableProperty]
    private string eBoardName;

    private string eSID;

    [ObservableProperty]
    private ObservableCollection<ElementViewModel> elements = new ObservableCollection<ElementViewModel>();

    //public EBoardViewModel(MainViewModel mainViewModel, string escreenId = "-1")
    //    : base()
    //{
    //    this.mainViewModel = mainViewModel;

    //    this.fluidUIMenuViewModel = new FluidUIMenuViewModel(this, fluidUIContextHasStand: false);

    //    this.eBoardSettingsViewModel = new ScreenControlViewModel(this);

    //    this.eSID = escreenId;

    //    if (string.IsNullOrWhiteSpace(this.eSID) || this.eSID.Equals("-1"))
    //    {
    //        DateTime dateTime = DateTime.Now;

    //        this.eSID = $"EBoard_{dateTime.Ticks}";
    //    }

    //    this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

    //    this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    //    this.OnPropertyChanged(nameof(this.FluidUI));
    //}

    public EBoardViewModel(MainViewModel mainViewModel, EboardScreen eboardscreenconfig)
        : base()
    {
        this.mainViewModel = mainViewModel;

        this.eSID = eboardscreenconfig.EBID;
        this.EBoardName = eboardscreenconfig.EBoardName;
        this.EBoardDepth = eboardscreenconfig.EBoardDepth;

        eboardscreenconfig?.EBoardScreenContext?.Design?.LoadBrushesFromColorData();
        this.SetFluidUI(eboardscreenconfig.EBoardScreenContext);

        var helper = new SharedMethod_UI();
        helper.SetupTitleAndText(
            this.FluidUI.DataBlock,
            "Eboard Screen",
            "you can have more than one eboard screen\n\nopen eboard browser to edit eboard screens\n\nyou can add elements to the visible eboard screen\n\nyou can change this description");

        this.SetFluidUIMenuViewModel(new FluidUIMenuViewModel(this, fluidUIContextHasStand: false));

        this.eBoardSettingsViewModel = new ScreenControlViewModel(this);

        this.FontSizeValue = (int)this.FluidUI.Font.FontSize;

        this.OnPropertyChanged(nameof(this.Elements));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public IList<ElementInstantiationPolicy>? InstantiationPolicies => [
        ElementInstantiationPolicy.Unique,
        ElementInstantiationPolicy.Global,
        ElementInstantiationPolicy.OnePerScreen,
        ElementInstantiationPolicy.DefaultScreenTypesOnly,

        // ElementInstantiationPolicy.Unconstrained,
        // ElementInstantiationPolicy.ValueNotSet
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

    public ScreenControlViewModel EBoardSettingsViewModel => this.eBoardSettingsViewModel;

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
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false)
            {
                continue;
            }

            if (!item.Equals(elementViewModel))
            {
                item.BeginMovement(elementViewModel);
            }
        }
    }

    //public void Apply_FluidUIDesignBrushTargetToImage(BrushTargets brushTargets, string path)
    //{
    //    this.SetImageToBrushTarget(brushTargets, path);
    //}

    public void ChangeSelection_CornerRadius(ElementViewModel elementViewModel, QuadValue<int> cornerRadius)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Apply_FluidUISizeQuadValue(cornerRadius, BorderTargets.CornerRadius);
            }
        }
    }

    public void ChangeSelection_BackgroundBrush(ElementViewModel elementViewModel, Brush brush)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(brush, BrushTargets.Background);
            }
        }
    }

    public void ChangeSelection_Height(ElementViewModel elementViewModel, int heightValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            if (item.IsSelected)
            {
                item.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Apply_FluidUISizeHeight(heightValue);
            }
        }
    }

    public void ChangeSelection_RotationAngle(ElementViewModel elementViewModel, int rotationValueDelta)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            item.ApplyRotationToGroupSelectedElement(rotationValueDelta);
        }
    }

    public void ChangeSelection_WidthValue(ElementViewModel elementViewModel, int widthValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            item.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Apply_FluidUISizeWidth(widthValue);
        }
    }

    public void ChangeSelection_ZIndex(ElementViewModel elementViewModel, int zIndexValue)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false || item.EID.Equals(elementViewModel.EID))
            {
                continue;
            }

            item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.ApplyZIndexValue(zIndexValue);
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

    public int GetContainerCount()
    {
        return this.Elements.Count;
    }

    public int GetElementCount()
    {
        var nonshapecount = this.Elements.Where(x => x?.Plugin?.PluginCategory != PluginCategories.Shape)?.ToList().Count;

        if (nonshapecount == null)
        {
            return 0;
        }

        return (int)nonshapecount;
    }

    public int GetShapeCount()
    {
        var nonshapecount = this.Elements.Where(x => x?.Plugin?.PluginCategory == PluginCategories.Shape)?.ToList().Count;

        if (nonshapecount == null)
        {
            return 0;
        }

        return (int)nonshapecount;
    }

    public DateTime GetCreatedDate()
    {
        string cutEBID = this.eSID.Replace("EBoard_", string.Empty);

        long ticks = long.Parse(cutEBID);

        DateTime dateTime = new DateTime(ticks);

        return dateTime;
    }

    public IList<EBoardElementPluginBaseViewModel> GetPlugins()
    {
        return this.mainViewModel.MainWindowMenuBarVM.FoundPlugins;
    }

    public void MoveElementSelection(ElementViewModel elementViewModel, Point newPosition)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false)
            {
                continue;
            }

            if (item.EID != null && elementViewModel.EID != null)
            {
                if (item.EID.Equals(elementViewModel.EID))
                {
                    continue;
                }
            }

            item.MoveXY(elementViewModel, newPosition);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void MoveLastClickedElement(ElementViewModel elementViewModel)
    {
        if (this.Elements.Count > 1)
        {
            this.Elements.Move(this.Elements.IndexOf(elementViewModel), this.Elements.Count - 1);
        }
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

    partial void OnElementsChanged(ObservableCollection<ElementViewModel>? oldValue, ObservableCollection<ElementViewModel> newValue)
    {
        var outdatedbadcrap = oldValue;
        var replacementbadcrapforoldcrap = newValue;
    }

    public void StopElementSelectionMovement(ElementViewModel elementViewModel)
    {
        if (this.Elements == null || elementViewModel == null)
        {
            return;
        }

        foreach (ElementViewModel item in this.Elements)
        {
            if (item == null || item.IsSelected == false)
            {
                continue;
            }

            if (!item.EID.Equals(elementViewModel.EID) && item.IsSelected)
            {
                item.StopMovement();
            }
        }
    }

    private void UpdateElementsZIndexProperties(int newEBoardDepth)
    {
        if (this.Elements != null && this.Elements.Count > 0)
        {
            foreach (ElementViewModel item in this.Elements)
            {
                item.FluidUIMenuViewModel.FluidUIStandSetupViewModel.CalibrateZSliderValues(newEBoardDepth);
            }
        }
    }

    partial void OnEBoardDepthChanged(int value)
    {
        this.UpdateElementsZIndexProperties(value);
    }

    [RelayCommand]
    private void DeleteEBoard()
    {
        if (this.mainViewModel is not null)
        {
            this.mainViewModel.EBoardBrowserViewModel.RemoveSelectedEBoard(this);
        }
    }

    [RelayCommand]
    private void LeftClick()
    {
        this.DeselectElements();
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
}

// EOF