// <copyright file="ShapeBaseViewModel.cs" company=".">
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
using EBoardSDK.Plugins;
using System;
using System.Windows;

public abstract partial class ShapeBaseViewModel : EBoardElementPluginBaseViewModel, IDisposable
{
    private EboardFluidUIBaseViewModel fluidUIViewModel;

    public EboardFluidUIBaseViewModel FluidUIViewModel => this.fluidUIViewModel;

    [ObservableProperty]
    private double strokeThickness = 1.0;

    public void SetFluidUIViewModel(EboardFluidUIBaseViewModel fluidUIViewModel)
    {
        this.fluidUIViewModel = fluidUIViewModel;

        this.FluidUIViewModel.FluidUI.Apply_FluidUI();

        if (this.ElementViewModel != null)
        {
            this.ElementViewModel.FluidUI.Size.Margin = new Thickness(0);
            this.ElementViewModel.FluidUI.Size.Padding = new Thickness(0);
            this.ElementViewModel.Redraw();
        }

        this.FluidUIViewModel.FluidUI.Size.Margin = new Thickness(0);
        this.FluidUIViewModel.FluidUI.Size.Padding = new Thickness(0);

        //this.FluidUIViewModel.FluidUI.Size.PropertyChangedEvent += this.Size_PropertyChangedEvent; ;
        //this.FluidUIViewModel.FluidUI.Design.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;

        this.OnPropertyChanged(nameof(this.FluidUIViewModel.FluidUI.Size));
    }

    //private void Size_PropertyChangedEvent()
    //{
    //    this.StrokeThickness = this.FluidUIViewModel.FluidUI.Size.BorderThickness.Top;
    //}

    //private void BrushManagement_PropertyChangedEvent()
    //{
    //    this.ElementViewModel.FluidUI.Design.Highlight = this.FluidUIViewModel.FluidUI.Design.Highlight;

    //    this.ElementViewModel.FluidUI.Size.Margin = new Thickness(0);
    //    this.ElementViewModel.FluidUI.Size.Padding = new Thickness(0);

    //    this.ElementViewModel.Redraw();
    //}

    public void Dispose()
    {
        //this.FluidUIViewModel.FluidUI.Design.PropertyChangedEvent -= this.BrushManagement_PropertyChangedEvent;
        //this.FluidUIViewModel.FluidUI.Size.PropertyChangedEvent -= this.Size_PropertyChangedEvent;
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        if (this.ElementViewModel != null)
        {
            this.ElementViewModel.EBoardViewModel.RemoveElement(this.ElementViewModel);
        }
    }
}

// EOF