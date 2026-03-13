// <copyright file="FluidUISizeSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUISize;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System;
using System.Windows;

public partial class FluidUISizeSetupViewModel : ObservableObject, IFluidUISizeSetup
{
    private EboardFluidUIBaseViewModel viewModel;
    private FluidUISizeManager fluidUISizeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISizeSetupViewModel"/> class.
    /// </summary>
    public FluidUISizeSetupViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUISizeSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    public FluidUISizeSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.fluidUISizeManager = new FluidUISizeManager(this.ViewModel);

        var helper = new SharedMethod_UI();

        this.CornerRadiusQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel,
            this.ResetCorners,
            BorderTargets.CornerRadius);
        this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged;

        this.MarginQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel,
            this.ResetMargin,
            BorderTargets.Margin);
        this.MarginQuadSetup.PropertyChanged += this.MarginQuadSetup_PropertyChanged;

        this.PaddingQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel,
            this.ResetPadding,
            BorderTargets.Padding);
        this.PaddingQuadSetup.PropertyChanged += this.PaddingQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel,
            this.ResetThickness,
            BorderTargets.Thickness);
        this.ThicknessQuadSetup.PropertyChanged += this.ThicknessQuadSetup_PropertyChanged;
    }

    public event Action? PropertyChangedEvent;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public void Dispose()
    {
        this.CornerRadiusQuadSetup.PropertyChanged -= this.CornerRadiusQuadSetup_PropertyChanged;
        this.MarginQuadSetup.PropertyChanged -= this.MarginQuadSetup_PropertyChanged;
        this.PaddingQuadSetup.PropertyChanged -= this.PaddingQuadSetup_PropertyChanged;
        this.ThicknessQuadSetup.PropertyChanged -= this.ThicknessQuadSetup_PropertyChanged;

        this.PropertyChangedEvent = null;
    }

    public void SetInitialValues()
    {
        this.fluidUISizeManager.Reset();
    }

<<<<<<< Updated upstream
=======
    public void UpdateValues()
    {
        this.OnPropertyChanged(nameof(this.CornerRadiusQuadSetup));
        this.OnPropertyChanged(nameof(this.MarginQuadSetup));
        this.OnPropertyChanged(nameof(this.PaddingQuadSetup));
        this.OnPropertyChanged(nameof(this.ThicknessQuadSetup));
    }

    [RelayCommand]
    public void ResetCorners()
    {
        this.fluidUISizeManager.ResetFluidUISizeTarget(BorderTargets.CornerRadius);
        this.CornerRadiusQuadSetup?.Reset();
    }

    [RelayCommand]
    public void ResetMargin()
    {
        this.fluidUISizeManager.ResetFluidUISizeTarget(BorderTargets.Margin);
        this.MarginQuadSetup?.Reset();
    }

    [RelayCommand]
    public void ResetPadding()
    {
        this.fluidUISizeManager.ResetFluidUISizeTarget(BorderTargets.Padding);
        this.PaddingQuadSetup?.Reset();
    }

    [RelayCommand]
    public void ResetThickness()
    {
        this.fluidUISizeManager.ResetFluidUISizeTarget(BorderTargets.Thickness);
        this.ThicknessQuadSetup?.Reset();
    }

    [RelayCommand]
    protected void ResetSize()
    {
        this.fluidUISizeManager.Reset_FluidUISizeWidthAndHeight();
    }

>>>>>>> Stashed changes
    private void MarginQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var value = (Thickness)this.MarginQuadSetup.GetQuadValueObject(BorderTargets.Margin);

        if (value != this.fluidUISizeManager.GetMargin())
        {
            this.fluidUISizeManager.SetMargin(value);
        }
    }

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var value = (Thickness)this.PaddingQuadSetup.GetQuadValueObject(BorderTargets.Padding);

        if (value != this.fluidUISizeManager.GetPadding())
        {
            this.fluidUISizeManager.SetPadding(value);
        }
    }

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var value = (Thickness)this.ThicknessQuadSetup.GetQuadValueObject(BorderTargets.Thickness);

        if (value != this.fluidUISizeManager.GetBorderThickness())
        {
            this.fluidUISizeManager.SetBorderThickness(value);
        }
    }

    private void CornerRadiusQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var value = (CornerRadius)this.CornerRadiusQuadSetup.GetQuadValueObject(BorderTargets.CornerRadius);

        if (value != this.fluidUISizeManager.GetCornerRadius())
        {
            this.fluidUISizeManager.SetCornerRadius(value);
        }
    }
}

// EOF