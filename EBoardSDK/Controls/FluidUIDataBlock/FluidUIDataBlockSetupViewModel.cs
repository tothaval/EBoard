// <copyright file="FluidUIDataBlockSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIDataBlock;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.FluidUIDataBlock.FluidUIIndexText;
using EBoardSDK.Controls.FluidUIDataBlock.FluidUIKeyText;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System;
using System.Collections.ObjectModel;

/// <summary>
/// TODO: refactor into several views and viewmodels.
/// </summary>
public partial class FluidUIDataBlockSetupViewModel : ObservableObject, IFluidUIDataBlockSetup
{
    private FluidUIBaseViewModel viewModel;
    private FluidUIDataBlockManager fluidUIDataBlockManager;

    private FluidUIStandSettings fluidUIStandSettings = FluidUIStandSettings.NoStandContextArea;

    private bool templateCreation;

    private FluidUISelectionViewModel loadSelectionViewModel;
    private FluidUISelectionViewModel saveSelectionViewModel;

    [ObservableProperty]
    private FluidUIIndexTextViewModel? fluidUIIndexText;

    [ObservableProperty]
    private FluidUIKeyTextViewModel? fluidUIKeyText;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string text = string.Empty;

    [ObservableProperty]
    private bool showToolTip = true;

    [ObservableProperty]
    private int indexCount = 0;

    [ObservableProperty]
    private ObservableCollection<FluidUIIndexTextViewModel> fluidUIIndexTexts = new();

    [ObservableProperty]
    private ObservableCollection<FluidUIKeyTextViewModel> fluidUIKeyTexts = new();

    [ObservableProperty]
    private ObservableCollection<QuadFluidUIIndexTextViewModel> quadIndexTexts = new();

    [ObservableProperty]
    private ObservableCollection<QuadFluidUIKeyTextViewModel> quadKeyTexts = new();

    [ObservableProperty]
    private QuadFluidUIIndexTextViewModel selectedIndexTextQuadValue;

    [ObservableProperty]
    private QuadFluidUIKeyTextViewModel selectedKeyTextQuadValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDataBlockSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="fluidUIStandSettings"></param>
    /// <param name="templateCreation"></param>
    public FluidUIDataBlockSetupViewModel(FluidUIBaseViewModel eboardFluidUIBaseViewModel, FluidUIStandSettings fluidUIStandSettings, bool templateCreation)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.fluidUIStandSettings = fluidUIStandSettings;
        this.templateCreation = templateCreation;

        this.fluidUIDataBlockManager = new FluidUIDataBlockManager(this.ViewModel);

        this.loadSelectionViewModel = new FluidUISelectionViewModel(this.ViewModel, this.LoadFluidUIConfiguration);
        this.saveSelectionViewModel = new FluidUISelectionViewModel(this.ViewModel, this.SaveFluidUIConfiguration);

        this.Setup();
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    public bool HasStand => !(this.fluidUIStandSettings == FluidUIStandSettings.NoStandContextArea);

    public bool TemplateCreation => this.templateCreation;

    public FluidUISelectionViewModel LoadSelectionViewModel => this.loadSelectionViewModel;

    public FluidUISelectionViewModel SaveSelectionViewModel => this.saveSelectionViewModel;

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public bool InverseIndexTextListIsEmpty => this.FluidUIIndexTexts.Count != 0;

    public bool InverseKeyTextListIsEmpty => this.FluidUIKeyTexts.Count != 0;

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.Setup();
    }

    private async void LoadFluidUIConfiguration()
    {
        var manager = new FluidUIContextManager(this.ViewModel);

        await manager.LoadFluidUIConfiguration(this.LoadSelectionViewModel);
    }

    private async void SaveFluidUIConfiguration()
    {
        var manager = new FluidUIContextManager(this.ViewModel);

        await manager.SaveFluidUIConfiguration(this.SaveSelectionViewModel);
    }

    private void Setup()
    {
        this.Text = this.fluidUIDataBlockManager.GetText();
        this.Title = this.fluidUIDataBlockManager.GetTitle();

        var indexTextViewModelCollection = this.fluidUIDataBlockManager.GetFluidUIIndexTextViewModelObservableCollection();

        this.ShowToolTip = this.fluidUIDataBlockManager.GetShowToolTip();

        if (indexTextViewModelCollection != null)
        {
            this.FluidUIIndexTexts = indexTextViewModelCollection;
            this.FluidUIIndexText = this.FluidUIIndexTexts.FirstOrDefault()!;
        }

        var keyTextViewModelCollection = this.fluidUIDataBlockManager.GetFluidUIKeyTextViewModelObservableCollection();

        if (keyTextViewModelCollection != null)
        {
            this.FluidUIKeyTexts = keyTextViewModelCollection;
            this.FluidUIKeyText = this.FluidUIKeyTexts.FirstOrDefault()!;
        }

        this.QuadIndexTexts = this.fluidUIDataBlockManager.GetIndexTextQuadViewModels();

        this.SelectedIndexTextQuadValue = this.QuadIndexTexts.FirstOrDefault()!;

        this.QuadKeyTexts = this.fluidUIDataBlockManager.GetKeyTextQuadViewModels();

        this.SelectedKeyTextQuadValue = this.QuadKeyTexts.FirstOrDefault()!;

        this.OnPropertyChanged(nameof(this.InverseIndexTextListIsEmpty));
        this.OnPropertyChanged(nameof(this.InverseKeyTextListIsEmpty));
        this.OnPropertyChanged(nameof(this.QuadIndexTexts));
        this.OnPropertyChanged(nameof(this.QuadKeyTexts));

        this.OnPropertyChanged(nameof(this.LoadSelectionViewModel));
        this.OnPropertyChanged(nameof(this.SaveSelectionViewModel));

        this.OnPropertyChanged(nameof(this.HasStand));
        this.OnPropertyChanged(nameof(this.TemplateCreation));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    partial void OnShowToolTipChanged(bool value)
    {
        this.fluidUIDataBlockManager.SetShowToolTip(value);
    }

    partial void OnTextChanged(string value)
    {
        this.fluidUIDataBlockManager?.SetText(value);
    }

    partial void OnTitleChanged(string value)
    {
        this.fluidUIDataBlockManager?.SetTitle(value);
    }

    [RelayCommand]
    private void AddIndexText()
    {
        this.IndexCount = this.fluidUIDataBlockManager.GetNewIndexCount();
        this.FluidUIIndexText = this.fluidUIDataBlockManager.GetFluidUIIndexTextViewModel();

        if (this.FluidUIIndexText != null)
        {
            this.FluidUIIndexTexts.Add(this.FluidUIIndexText);
        }

        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.FluidUIIndexText));

        this.OnPropertyChanged(nameof(this.InverseIndexTextListIsEmpty));
    }

    [RelayCommand]
    private void CopyConfiguration()
    {
        var mainViewModel = this.ViewModel as MainViewModel;
        var navigation = this.ViewModel as NavigationContextViewModel;
        var screen = this.ViewModel as ScreenViewModel;
        var element = this.ViewModel as ElementViewModel;

        if (navigation != null)
        {
            mainViewModel = navigation.MainViewModel;
        }

        if (screen != null)
        {
            mainViewModel = screen.MainViewModel;
        }

        if (element != null)
        {
            mainViewModel = element.ScreenViewModel.MainViewModel;
        }

        var manager = new FluidUIContextManager(this.ViewModel);
        var copy = manager.GetCustomFluidUIConfiguration(this.LoadSelectionViewModel);

        mainViewModel?.LoadIFluidUIContextCopy(copy);
    }

    [RelayCommand]
    private void PasteConfiguration(object? parameter)
    {
        var mainViewModel = this.ViewModel as MainViewModel;
        var navigation = this.ViewModel as NavigationContextViewModel;
        var screen = this.ViewModel as ScreenViewModel;
        var element = this.ViewModel as ElementViewModel;

        if (navigation != null)
        {
            mainViewModel = navigation.MainViewModel;
        }

        if (screen != null)
        {
            mainViewModel = screen.MainViewModel;
        }

        if (element != null)
        {
            mainViewModel = element.ScreenViewModel.MainViewModel;
        }

        if (mainViewModel != null && mainViewModel.FluidUIContextCopy != null)
        {
            var manager = new FluidUIContextManager(this.ViewModel);
            var copy = manager.GetCustomFluidUIContextCopy(this.LoadSelectionViewModel, mainViewModel.FluidUIContextCopy);

            this.ViewModel.SetFluidUIByUser(copy);
        }
    }

    [RelayCommand]
    private void DeleteIndexText()
    {
        if (this.FluidUIIndexText == null)
        {
            return;
        }

        var changedCollection = this.fluidUIDataBlockManager.DeleteFluidUIIndexText(this.FluidUIIndexText);

        if (changedCollection != null)
        {
            this.FluidUIIndexTexts.Clear();
            this.FluidUIIndexTexts = changedCollection;
        }

        if (this.FluidUIIndexTexts != null && this.FluidUIIndexTexts.Count > 0)
        {
            this.FluidUIIndexText = this.FluidUIIndexTexts.LastOrDefault()!;
        }

        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.FluidUIIndexText));
        this.OnPropertyChanged(nameof(this.InverseIndexTextListIsEmpty));
    }

    [RelayCommand]
    private void ClearIndexTextList()
    {
        this.fluidUIDataBlockManager.ClearIndexTextList();
        this.FluidUIIndexTexts.Clear();

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.InverseIndexTextListIsEmpty));
    }

    [RelayCommand]
    private void AddKeyText()
    {
        this.FluidUIKeyText = this.fluidUIDataBlockManager.GetFluidUIKeyTextViewModel();

        if (this.FluidUIKeyText != null)
        {
            this.FluidUIKeyTexts.Add(this.FluidUIKeyText);
        }

        this.OnPropertyChanged(nameof(this.FluidUIKeyTexts));
        this.OnPropertyChanged(nameof(this.FluidUIKeyText));
        this.OnPropertyChanged(nameof(this.InverseKeyTextListIsEmpty));
    }

    [RelayCommand]
    private void DeleteKeyText()
    {
        if (this.FluidUIKeyText == null)
        {
            return;
        }

        var changedCollection = this.fluidUIDataBlockManager.DeleteFluidUKeyText(this.FluidUIKeyText);

        if (changedCollection != null)
        {
            this.FluidUIKeyTexts.Clear();
            this.FluidUIKeyTexts = changedCollection;
        }

        if (this.FluidUIKeyTexts != null && this.FluidUIKeyTexts.Count > 0)
        {
            this.FluidUIKeyText = this.FluidUIKeyTexts.LastOrDefault()!;
        }

        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.FluidUIIndexText));
        this.OnPropertyChanged(nameof(this.InverseKeyTextListIsEmpty));
    }

    [RelayCommand]
    private void ClearKeyTextList()
    {
        this.fluidUIDataBlockManager.ClearKeyTextList();
        this.FluidUIKeyTexts.Clear();

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIIndexTexts));
        this.OnPropertyChanged(nameof(this.InverseKeyTextListIsEmpty));
    }

    [RelayCommand]
    private void ResetSelectedIndexTextQuadValue()
    {
        this.SelectedIndexTextQuadValue?.Reset();
        this.fluidUIDataBlockManager.SetInitialIndexTextQuadValues();
    }

    [RelayCommand]
    private void ResetSelectedKeyTextQuadValue()
    {
        this.SelectedKeyTextQuadValue?.Reset();
        this.fluidUIDataBlockManager.SetInitialKeyTextQuadValues();
    }

    [RelayCommand]
    private void ClearDataBlockTextAndTitle()
    {
        this.fluidUIDataBlockManager.ClearTextAndTitle();
    }

    [RelayCommand]
    private void ResetDataBlockTextAndTitle()
    {
        this.fluidUIDataBlockManager.SetupTitleAndText("fluid ui context", "fluid ui context description");
    }

    [RelayCommand]
    private void ResetConfigurationToPrevious()
    {
        this.fluidUIDataBlockManager.UndoLastFluidUIChange();
    }
}

// EOF