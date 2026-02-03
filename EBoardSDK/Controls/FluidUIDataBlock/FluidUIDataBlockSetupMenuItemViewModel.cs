// <copyright file="FluidUIDataBlockSetupMenuItemViewModel.cs" company=".">
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
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.ViewModels;
using System;

public partial class FluidUIDataBlockSetupMenuItemViewModel : ObservableObject, IFluidUIChangedAction
{
    private FluidUIBaseViewModel viewModel;

    private FluidUIDataBlockSetupViewModel setupViewModel;

    private FluidUIStandSettings fluidUIStandSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIDataBlockSetupMenuItemViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    public FluidUIDataBlockSetupMenuItemViewModel(FluidUIBaseViewModel eboardFluidUIBaseViewModel, FluidUIStandSettings fluidUIStandSettings)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.fluidUIStandSettings = fluidUIStandSettings;

        this.setupViewModel = new FluidUIDataBlockSetupViewModel(this.viewModel, this.fluidUIStandSettings, templateCreation: false);

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    public FluidUIDataBlockSetupViewModel SetupViewModel => this.setupViewModel;

    /// <inheritdoc/>
    public void Dispose()
    {
        this.setupViewModel?.Dispose();
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.setupViewModel = new FluidUIDataBlockSetupViewModel(this.viewModel, this.fluidUIStandSettings, templateCreation: false);
        this.OnPropertyChanged(nameof(this.SetupViewModel));
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.SetupViewModel?.UpdateValues();

        this.OnPropertyChanged(nameof(this.SetupViewModel));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }
}

// EOF