// <copyright file="EboardFluidUIBaseViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using System.Windows;

public partial class EboardFluidUIBaseViewModel : ObservableObject, IFluidUI
{
    public event Action? PropertyChangedEvent;

    private IFluidUIContext fluidUI;

    protected FluidUIMenuViewModel fluidUIMenuViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.FluidUI))]
    protected int cornerRadiusValue;

    [ObservableProperty]
    protected int fontSizeValue;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.FluidUI))]
    protected int height = -1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.FluidUI))]
    protected int width = -1;

    public EboardFluidUIBaseViewModel()
    {
        if (this.fluidUI == null)
        {
            this.fluidUI = new FluidUIContext();
            this.FluidUI.SetInitialValues();
            this.OnPropertyChanged(nameof(this.FluidUI));
        }

        //this.FluidUI.PropertyChangedEvent += this.FluidUI_PropertyChangedEvent;
        //this.FluidUI.Font.PropertyChangedEvent += this.Font_PropertyChangedEvent;
    }

    //private void Font_PropertyChangedEvent()
    //{

    //    this.OnPropertyChanged(nameof(this.FluidUI.Font.FontFamily));

    //    this.OnPropertyChanged(nameof(this.FluidUI));
    //}

    public IFluidUIContext FluidUI => this.fluidUI;

    public FluidUIMenuViewModel FluidUIMenuViewModel => this.fluidUIMenuViewModel;

    public void ApplyFluidUISizeChange()
    {
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public void Dispose()
    {
        if (this.FluidUI != null)
        {
            //this.FluidUI.PropertyChangedEvent -= this.FluidUI_PropertyChangedEvent;
            this.FluidUI.Dispose();
        }
    }

    public void SetFluidUI(IFluidUIContext? fluidUIContext)
    {
        //this.FluidUI.PropertyChangedEvent -= this.FluidUI_PropertyChangedEvent;

        if (fluidUIContext == null)
        {
            this.fluidUI = new FluidUIContext();
            this.FluidUI.SetInitialValues();
        }
        else
        {
            this.fluidUI = fluidUIContext;
        }

        //this.FluidUI.PropertyChangedEvent += this.FluidUI_PropertyChangedEvent;

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    public void SetFluidUIMenuViewModel(FluidUIMenuViewModel? fluidUIMenuViewModel)
    {
        if (fluidUIMenuViewModel != null)
        {
            this.fluidUIMenuViewModel = fluidUIMenuViewModel;
            //this.FluidUIMenuViewModel.FluidUIFontSetupViewModel.PropertyChangedEvent += this.Font_PropertyChangedEvent;
        }

        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    public void SetInitialValues()
    {
        this.FluidUI.SetInitialValues();
    }

    public virtual void TriggerRedraw()
    {
    }

    partial void OnCornerRadiusValueChanged(int value)
    {
        this.FluidUI.Size.CornerRadius = new CornerRadius(value);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    partial void OnHeightChanged(int value)
    {
        this.FluidUI.Size.Height = value;

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    partial void OnWidthChanged(int value)
    {
        this.FluidUI.Size.Width = value;

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void ResetCorners()
    {
        this.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Reset_FluidUISizeQuadValue(BorderTargets.CornerRadius);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    protected void ResetHighlightImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Reset_FluidUIDesignBrush(BrushTargets.Highlight);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    protected void ResetImageBorder()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Reset_FluidUIDesignBrush(BrushTargets.Border);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    protected void ResetForegroundImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Reset_FluidUIDesignBrush(BrushTargets.Foreground);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    protected void ResetImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Reset_FluidUIDesignBrush(BrushTargets.Background);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void ResetMargin()
    {
        this.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Reset_FluidUISizeQuadValue(BorderTargets.Margin);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void ResetPadding()
    {
        this.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Reset_FluidUISizeQuadValue(BorderTargets.Padding);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void ResetThickness()
    {
        this.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Reset_FluidUISizeQuadValue(BorderTargets.Thickness);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    protected void ResetSize()
    {
        this.FluidUIMenuViewModel.FluidUISizeSetupViewModel.Reset_FluidUISizeWidthAndHeight();

        this.SetElementSizeDisplayValue();

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void SetColorValueAsBackground()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.BackgroundBS!.SolidBrush.ColorBrush, BrushTargets.Background);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void SetColorValueAsForeground()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.ForegroundBS!.SolidBrush.ColorBrush, BrushTargets.Foreground);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void SetColorValueAsBorder()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.BorderBS!.SolidBrush.ColorBrush, BrushTargets.Border);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    public void SetColorValueAsHighlight()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.HighlightBS!.SolidBrush.ColorBrush, BrushTargets.Highlight);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void SetBackgroundImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SetUserChosenImagePath(BrushTargets.Background);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void SetForegroundImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SetUserChosenImagePath(BrushTargets.Foreground);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void SetBorderImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SetUserChosenImagePath(BrushTargets.Border);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void SetHighlightImage()
    {
        this.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.SetUserChosenImagePath(BrushTargets.Highlight);

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    [RelayCommand]
    private void ApplyFontSize()
    {
        this.FluidUIMenuViewModel.FluidUIFontSetupViewModel.ApplyFontSize(this.FontSizeValue);

        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    protected void SetElementSizeDisplayValue()
    {
        this.Height = new SharedMethod_UI().ResetSizeDisplayValue(this.FluidUI.Size.Height);
        this.Width = new SharedMethod_UI().ResetSizeDisplayValue(this.FluidUI.Size.Width);

        this.FluidUI.Size.Height = this.Height;
        this.FluidUI.Size.Width = this.Width;

        this.OnPropertyChanged(nameof(this.Width));
        this.OnPropertyChanged(nameof(this.Height));
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    //private void FluidUI_PropertyChangedEvent()
    //{
    //    this.OnPropertyChanged(nameof(this.FluidUI));
    //}
}

// EOF