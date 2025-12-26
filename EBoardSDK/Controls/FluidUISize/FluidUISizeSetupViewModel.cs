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
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System;
using System.Windows;

public class FluidUISizeSetupViewModel : ObservableObject, IFluidUISizeSetup
{
    private EboardFluidUIBaseViewModel viewModel;

    private bool fluidUIContextHasArea = true;

    public FluidUISizeSetupViewModel()
    {
    }

    public FluidUISizeSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, bool fluidUIContextHasArea = true)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        var helper = new SharedMethod_UI();

        this.CornerRadiusQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel.FluidUI.Size,
            eboardFluidUIBaseViewModel.FluidUI.Design,
            eboardFluidUIBaseViewModel.ResetCorners,
            BorderTargets.CornerRadius);
        this.CornerRadiusQuadSetup.PropertyChanged += this.CornerRadiusQuadSetup_PropertyChanged;

        this.MarginQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel.FluidUI.Size,
            eboardFluidUIBaseViewModel.FluidUI.Design,
            eboardFluidUIBaseViewModel.ResetMargin,
            BorderTargets.Margin);
        this.MarginQuadSetup.PropertyChanged += this.MarginQuadSetup_PropertyChanged;

        this.PaddingQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel.FluidUI.Size,
            eboardFluidUIBaseViewModel.FluidUI.Design,
            eboardFluidUIBaseViewModel.ResetPadding,
            BorderTargets.Padding);
        this.PaddingQuadSetup.PropertyChanged += this.PaddingQuadSetup_PropertyChanged;

        this.ThicknessQuadSetup = helper.GetQuadValueSetupViewModel(
            eboardFluidUIBaseViewModel.FluidUI.Size,
            eboardFluidUIBaseViewModel.FluidUI.Design,
            eboardFluidUIBaseViewModel.ResetThickness,
            BorderTargets.Thickness);
        this.ThicknessQuadSetup.PropertyChanged += this.ThicknessQuadSetup_PropertyChanged;

        if (!fluidUIContextHasArea)
        {
            this.fluidUIContextHasArea = fluidUIContextHasArea;
        }

        this.OnPropertyChanged(nameof(this.FluidUIContextHasArea));
    }

    public event Action PropertyChangedEvent;

    public bool FluidUIContextHasArea => this.fluidUIContextHasArea;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public QuadValueSetupViewModel CornerRadiusQuadSetup { get; set; }

    public QuadValueSetupViewModel MarginQuadSetup { get; set; }

    public QuadValueSetupViewModel PaddingQuadSetup { get; set; }

    public QuadValueSetupViewModel ThicknessQuadSetup { get; set; }

    public void Apply_FluidUISizeQuadValue(QuadValue<int> quadValue, BorderTargets borderTargets)
    {
        if (quadValue == null)
        {
            return;
        }

        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                this.CornerRadiusQuadSetup.TopLeft = quadValue.Value1;
                this.CornerRadiusQuadSetup.TopRight = quadValue.Value2;
                this.CornerRadiusQuadSetup.BottomRight = quadValue.Value3;
                this.CornerRadiusQuadSetup.BottomLeft = quadValue.Value4;
                break;
            case BorderTargets.Margin:
                this.MarginQuadSetup.TopLeft = quadValue.Value1;
                this.MarginQuadSetup.TopRight = quadValue.Value2;
                this.MarginQuadSetup.BottomRight = quadValue.Value3;
                this.MarginQuadSetup.BottomLeft = quadValue.Value4;
                break;
            case BorderTargets.Padding:
                this.PaddingQuadSetup.TopLeft = quadValue.Value1;
                this.PaddingQuadSetup.TopRight = quadValue.Value2;
                this.PaddingQuadSetup.BottomRight = quadValue.Value3;
                this.PaddingQuadSetup.BottomLeft = quadValue.Value4;
                break;
            case BorderTargets.Thickness:
                this.ThicknessQuadSetup.TopLeft = quadValue.Value1;
                this.ThicknessQuadSetup.TopRight = quadValue.Value2;
                this.ThicknessQuadSetup.BottomRight = quadValue.Value3;
                this.ThicknessQuadSetup.BottomLeft = quadValue.Value4;
                break;
            default:
                break;
        }

        this.ViewModel.ApplyFluidUISizeChange();
    }

    public void Apply_FluidUISizeHeight(int heightValue)
    {
        this.ViewModel.FluidUI.Size.Height = heightValue;

        this.ViewModel.ApplyFluidUISizeChange();
    }

    public void Apply_FluidUISizeWidth(int widthValue)
    {
        this.ViewModel.FluidUI.Size.Width = widthValue;

        this.ViewModel.ApplyFluidUISizeChange();
    }

    public void Dispose()
    {
        this.CornerRadiusQuadSetup.PropertyChanged -= this.CornerRadiusQuadSetup_PropertyChanged;
        this.MarginQuadSetup.PropertyChanged -= this.MarginQuadSetup_PropertyChanged;
        this.PaddingQuadSetup.PropertyChanged -= this.PaddingQuadSetup_PropertyChanged;
        this.ThicknessQuadSetup.PropertyChanged -= this.ThicknessQuadSetup_PropertyChanged;
    }

    public object Get_FluidUISizeObjectFromQuadValueSetup(QuadValueSetupViewModel quadValueSetupViewModel, BorderTargets borderTargets)
    {
        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                return new CornerRadius(
                    quadValueSetupViewModel.QuadValue.Value1,
                    quadValueSetupViewModel.QuadValue.Value2,
                    quadValueSetupViewModel.QuadValue.Value3,
                    quadValueSetupViewModel.QuadValue.Value4);
            case BorderTargets.Margin:
            case BorderTargets.Padding:
            case BorderTargets.Thickness:
                return new Thickness(
                    quadValueSetupViewModel.QuadValue.Value1,
                    quadValueSetupViewModel.QuadValue.Value2,
                    quadValueSetupViewModel.QuadValue.Value3,
                    quadValueSetupViewModel.QuadValue.Value4);
            default:
                return quadValueSetupViewModel.QuadValue;
        }
    }

    public void Reset_FluidUISizeQuadValue(BorderTargets borderTargets)
    {
        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                this.CornerRadiusQuadSetup.All = 0;
                break;
            case BorderTargets.Margin:
                this.MarginQuadSetup.All = 0;
                break;
            case BorderTargets.Padding:
                this.PaddingQuadSetup.All = 0;
                break;
            case BorderTargets.Thickness:
                this.ThicknessQuadSetup.All = 0;
                break;
            default:
                break;
        }

        this.ViewModel.ApplyFluidUISizeChange();
    }

    public void Reset_FluidUISizeWidthAndHeight()
    {
        this.ViewModel.FluidUI.Size.Width = double.NaN;

        this.ViewModel.FluidUI.Size.Height = double.NaN;

        this.ViewModel.ApplyFluidUISizeChange();
    }

    public void SetInitialValues()
    {
        this.ViewModel.FluidUI.Size.BorderThickness = new Thickness(2, 2, 2, 2);
        this.ViewModel.FluidUI.Size.CornerRadius = new CornerRadius(5, 5, 5, 5);
        this.ViewModel.FluidUI.Size.Height = double.NaN;
        this.ViewModel.FluidUI.Size.Margin = new Thickness(5);
        this.ViewModel.FluidUI.Size.Padding = new Thickness(5);
        this.ViewModel.FluidUI.Size.Width = double.NaN;

        this.ViewModel.ApplyFluidUISizeChange();
    }

    private void MarginQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.ViewModel.FluidUI.Size.Margin = (Thickness)this.Get_FluidUISizeObjectFromQuadValueSetup(this.MarginQuadSetup, BorderTargets.Margin);
        this.ViewModel.ApplyFluidUISizeChange();
    }

    private void PaddingQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.ViewModel.FluidUI.Size.Padding = (Thickness)this.Get_FluidUISizeObjectFromQuadValueSetup(this.PaddingQuadSetup, BorderTargets.Padding);
        this.ViewModel.ApplyFluidUISizeChange();
    }

    private void ThicknessQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.ViewModel.FluidUI.Size.BorderThickness = (Thickness)this.Get_FluidUISizeObjectFromQuadValueSetup(this.ThicknessQuadSetup, BorderTargets.Thickness);
        this.ViewModel.ApplyFluidUISizeChange();
    }

    private void CornerRadiusQuadSetup_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.ViewModel.FluidUI.Size.CornerRadius = (CornerRadius)this.Get_FluidUISizeObjectFromQuadValueSetup(this.CornerRadiusQuadSetup, BorderTargets.CornerRadius);
        this.ViewModel.ApplyFluidUISizeChange();
    }
}

// EOF