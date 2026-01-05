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

/// <summary>
/// TODO: benötigte ViewModel für ValueChanges aus den FluidUI Models rausholen und hier mit rein?
/// </summary>
public partial class FluidUIMenuViewModel : ObservableObject
{
    private readonly FluidUIDataBlockSetupViewModel fluidUIDataBlockSetupViewModel;
    private readonly FluidUIDesignSetupViewModel fluidUIDesignSetupViewModel;
    private readonly FluidUIFontSetupViewModel fluidUIFontSetupViewModel;
    private readonly FluidUISizeSetupViewModel fluidUISizeSetupViewModel;
    private readonly FluidUIStandSetupViewModel fluidUIStandSetupViewModel;

    private EboardFluidUIBaseViewModel viewModel;

    public FluidUIMenuViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, EBoardViewModel? eBoardViewModel = null, bool fluidUIContextHasStand = false, bool fluidUIContextHasArea = true)
    {
        this.viewModel = eboardFluidUIBaseViewModel;

        this.fluidUIDataBlockSetupViewModel = new FluidUIDataBlockSetupViewModel(eboardFluidUIBaseViewModel);
        this.fluidUIDesignSetupViewModel = new FluidUIDesignSetupViewModel(eboardFluidUIBaseViewModel);
        this.fluidUIFontSetupViewModel = new FluidUIFontSetupViewModel(eboardFluidUIBaseViewModel);
        this.fluidUISizeSetupViewModel = new FluidUISizeSetupViewModel(eboardFluidUIBaseViewModel, fluidUIContextHasArea);
        this.fluidUIStandSetupViewModel = new FluidUIStandSetupViewModel(eboardFluidUIBaseViewModel, eBoardViewModel, fluidUIContextHasStand);
    }

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public FluidUIDataBlockSetupViewModel FluidUIDataBlockSetupViewModel => this.fluidUIDataBlockSetupViewModel;

    public FluidUIDesignSetupViewModel FluidUIDesignSetupViewModel => this.fluidUIDesignSetupViewModel;

    public FluidUIFontSetupViewModel FluidUIFontSetupViewModel => this.fluidUIFontSetupViewModel;

    public FluidUISizeSetupViewModel FluidUISizeSetupViewModel => this.fluidUISizeSetupViewModel;

    public FluidUIStandSetupViewModel FluidUIStandSetupViewModel => this.fluidUIStandSetupViewModel;
}

// EOF