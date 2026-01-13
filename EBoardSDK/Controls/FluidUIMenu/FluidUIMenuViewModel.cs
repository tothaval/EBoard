// <copyright file="FluidUIMenuViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIMenu;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Controls.FluidUIDataBlock;
using EBoardSDK.Controls.FluidUIDesign;
using EBoardSDK.Controls.FluidUIFont;
using EBoardSDK.Controls.FluidUISize;
using EBoardSDK.Controls.FluidUIStand;
using EBoardSDK.ViewModels;
using System;

/// <summary>
/// TODO: benötigte ViewModel für ValueChanges aus den FluidUI Models rausholen und hier mit rein?.
/// </summary>
public partial class FluidUIMenuViewModel : ObservableObject, IDisposable
{
    private FluidUIDataBlockSetupViewModel? fluidUIDataBlockSetupViewModel;
    private FluidUIDesignSetupViewModel? fluidUIDesignSetupViewModel;
    private FluidUIFontSetupViewModel? fluidUIFontSetupViewModel;
    private FluidUISizeSetupViewModel? fluidUISizeSetupViewModel;
    private FluidUIStandSetupViewModel? fluidUIStandSetupViewModel;

    private EboardFluidUIBaseViewModel viewModel;
    private EboardFluidUIBaseViewModel outerViewModel;

    private EBoardViewModel? eBoardViewModel;
    private bool fluidUIContextHasStand;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIMenuViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="eBoardViewModel"></param>
    /// <param name="fluidUIContextHasStand"></param>
    public FluidUIMenuViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIContextHasStand = fluidUIContextHasStand;

        this.CreateViewModels();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIMenuViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="outerViewModel"></param>
    /// <param name="eBoardViewModel"></param>
    /// <param name="fluidUIContextHasStand"></param>
    public FluidUIMenuViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EboardFluidUIBaseViewModel outerViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.outerViewModel = outerViewModel;

        this.eBoardViewModel = eBoardViewModel;
        this.fluidUIContextHasStand = fluidUIContextHasStand;

        this.CreateViewModels();
    }

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public FluidUIDataBlockSetupViewModel FluidUIDataBlockSetupViewModel => this.fluidUIDataBlockSetupViewModel;

    public FluidUIDesignSetupViewModel FluidUIDesignSetupViewModel => this.fluidUIDesignSetupViewModel;

    public FluidUIFontSetupViewModel FluidUIFontSetupViewModel => this.fluidUIFontSetupViewModel;

    public FluidUISizeSetupViewModel FluidUISizeSetupViewModel => this.fluidUISizeSetupViewModel;

    public FluidUIStandSetupViewModel FluidUIStandSetupViewModel => this.fluidUIStandSetupViewModel;

    public void Dispose()
    {
        this.DeleteViewModels();
    }

    internal void CreateFluidUIDataBlockSetupViewModel()
    {
        this.fluidUIDataBlockSetupViewModel = new FluidUIDataBlockSetupViewModel(this.ViewModel);
        this.OnPropertyChanged(nameof(this.FluidUIDataBlockSetupViewModel));
    }

    internal void CreateFluidUIDesignSetupViewModel()
    {
        this.fluidUIDesignSetupViewModel = new FluidUIDesignSetupViewModel(this.ViewModel);
        this.OnPropertyChanged(nameof(this.FluidUIDesignSetupViewModel));
    }

    internal void CreateFluidUIFontSetupViewModel()
    {
        this.fluidUIFontSetupViewModel = new FluidUIFontSetupViewModel(this.ViewModel);
        this.OnPropertyChanged(nameof(this.FluidUIFontSetupViewModel));
    }

    internal void CreateFluidUISizeSetupViewModel()
    {
        if (this.outerViewModel == null)
        {
            this.fluidUISizeSetupViewModel = new FluidUISizeSetupViewModel(this.ViewModel);
        }
        else
        {
            // TODO: neues View und ViewModel für Shapes
            this.fluidUISizeSetupViewModel = new FluidUISizeSetupViewModel(this.ViewModel);
        }

        this.OnPropertyChanged(nameof(this.FluidUISizeSetupViewModel));
    }

    internal void CreateFluidUIStandSetupViewModel()
    {
        if (this.outerViewModel == null)
        {
            this.fluidUIStandSetupViewModel = new FluidUIStandSetupViewModel(this.ViewModel, this.eBoardViewModel, this.fluidUIContextHasStand);
        }
        else
        {
            this.fluidUIStandSetupViewModel = new FluidUIStandSetupViewModel(this.ViewModel, this.outerViewModel, this.eBoardViewModel, this.fluidUIContextHasStand);
        }

        this.OnPropertyChanged(nameof(this.FluidUIStandSetupViewModel));
    }

    internal void DeleteFluidUIDataBlockSetupViewModel()
    {
        this.fluidUIDataBlockSetupViewModel?.Dispose();
        this.fluidUIDataBlockSetupViewModel = null;
    }

    internal void DeleteFluidUIDesignSetupViewModel()
    {
        this.fluidUIDesignSetupViewModel?.Dispose();
        this.fluidUIDesignSetupViewModel = null;
    }

    internal void DeleteFluidUIFontSetupViewModel()
    {
        this.fluidUIFontSetupViewModel?.Dispose();
        this.fluidUIFontSetupViewModel = null;
    }

    internal void DeleteFluidUISizeSetupViewModel()
    {
        this.fluidUISizeSetupViewModel?.Dispose();
        this.fluidUISizeSetupViewModel = null;
    }

    internal void DeleteFluidUIStandSetupViewModel()
    {
        this.fluidUIStandSetupViewModel?.Dispose();
        this.fluidUIStandSetupViewModel = null;
    }

    internal void CreateViewModels()
    {
        this.CreateFluidUIDataBlockSetupViewModel();
        this.CreateFluidUIDesignSetupViewModel();
        this.CreateFluidUIFontSetupViewModel();
        this.CreateFluidUISizeSetupViewModel();
        this.CreateFluidUIStandSetupViewModel();

        this.OnPropertyChanged(nameof(this.FluidUIDataBlockSetupViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIDesignSetupViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIFontSetupViewModel));
        this.OnPropertyChanged(nameof(this.FluidUISizeSetupViewModel));
        this.OnPropertyChanged(nameof(this.FluidUIStandSetupViewModel));
    }

    private void DeleteViewModels()
    {
        this.DeleteFluidUIDataBlockSetupViewModel();
        this.DeleteFluidUIDesignSetupViewModel();
        this.DeleteFluidUIFontSetupViewModel();
        this.DeleteFluidUISizeSetupViewModel();
        this.DeleteFluidUIStandSetupViewModel();
    }
}

// EOF