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
using EBoardSDK.Enums;
using EBoardSDK.ViewModels;
using System;

public partial class FluidUIMenuViewModel : ObservableObject, IDisposable
{
    private FluidUIDataBlockSetupMenuItemViewModel? fluidUIDataBlockSetupViewModel;
    private FluidUIDesignSetupMenuItemViewModel? fluidUIDesignSetupViewModel;
    private FluidUIFontSetupMenuItemViewModel? fluidUIFontSetupViewModel;
    private FluidUISizeSetupMenuItemViewModel? fluidUISizeSetupViewModel;
    private FluidUIStandSetupMenuItemViewModel? fluidUIStandSetupViewModel;

    private FluidUIBaseViewModel viewModel;
    private FluidUIBaseViewModel? outerViewModel;

    private FluidUIStandSettings fluidUIStandSettings = FluidUIStandSettings.NoStandContextArea;

    private ScreenViewModel? eBoardViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIMenuViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="fluidUIStandSettings"></param>
    /// <param name="screenViewModel"></param>
    public FluidUIMenuViewModel(FluidUIBaseViewModel eboardFluidUIBaseViewModel, FluidUIStandSettings fluidUIStandSettings, ScreenViewModel? screenViewModel = null)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        this.eBoardViewModel = screenViewModel;

        this.fluidUIStandSettings = fluidUIStandSettings;

        this.OnPropertyChanged(nameof(this.FluidUIContextHasStand));

        this.CreateViewModels();
    }

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public bool FluidUIContextHasStand => !(this.fluidUIStandSettings == FluidUIStandSettings.NoStandContextArea);

    public FluidUIDataBlockSetupMenuItemViewModel FluidUIDataBlockSetupViewModel => this.fluidUIDataBlockSetupViewModel;

    public FluidUIDesignSetupMenuItemViewModel FluidUIDesignSetupViewModel => this.fluidUIDesignSetupViewModel;

    public FluidUIFontSetupMenuItemViewModel FluidUIFontSetupViewModel => this.fluidUIFontSetupViewModel;

    public FluidUISizeSetupMenuItemViewModel FluidUISizeSetupViewModel => this.fluidUISizeSetupViewModel;

    public FluidUIStandSetupMenuItemViewModel FluidUIStandSetupViewModel => this.fluidUIStandSetupViewModel;

    /// <inheritdoc/>
    public void Dispose()
    {
        this.DeleteViewModels();
    }

    internal void CreateFluidUIDataBlockSetupViewModel()
    {
        this.fluidUIDataBlockSetupViewModel = new FluidUIDataBlockSetupMenuItemViewModel(this.ViewModel, this.fluidUIStandSettings);
        this.OnPropertyChanged(nameof(this.FluidUIDataBlockSetupViewModel));
    }

    internal void CreateFluidUIDesignSetupViewModel()
    {
        this.fluidUIDesignSetupViewModel = new FluidUIDesignSetupMenuItemViewModel(this.ViewModel);
        this.OnPropertyChanged(nameof(this.FluidUIDesignSetupViewModel));
    }

    internal void CreateFluidUIFontSetupViewModel()
    {
        this.fluidUIFontSetupViewModel = new FluidUIFontSetupMenuItemViewModel(this.ViewModel);
        this.OnPropertyChanged(nameof(this.FluidUIFontSetupViewModel));
    }

    internal void CreateFluidUISizeSetupViewModel()
    {
        if (this.outerViewModel == null)
        {
            this.fluidUISizeSetupViewModel = new FluidUISizeSetupMenuItemViewModel(this.ViewModel);
        }
        else
        {
            // TODO: neues View und ViewModel für Shapes
            this.fluidUISizeSetupViewModel = new FluidUISizeSetupMenuItemViewModel(this.ViewModel);
        }

        this.OnPropertyChanged(nameof(this.FluidUISizeSetupViewModel));
    }

    internal void CreateFluidUIStandSetupViewModel()
    {
        if (!this.FluidUIContextHasStand)
        {
            return;
        }

        this.fluidUIStandSetupViewModel = new FluidUIStandSetupMenuItemViewModel(this.ViewModel, this.fluidUIStandSettings, this.eBoardViewModel!);

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