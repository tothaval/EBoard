// <copyright file="FluidUIContextManager.cs" company=".">
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
namespace EBoardSDK.Models;

using EBoardConfigManager.Helper;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Interfaces.FluidUIFont;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Interfaces.FluidUIStand;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Models.FluidUIStand;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System.Windows;
using System.Windows.Media;

internal class FluidUIContextManager : IFluidUIManager
{
    private FluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIContextManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIContextManager(FluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    /// <summary>
    /// Resets all FluidUI properties to initial values
    /// and updates EboardFluidUIBaseViewModel.
    /// </summary>
    public void Reset(bool calledByIFluidUIContextManager = false)
    {
        new FluidUIDataBlockManager(this.viewModel).Reset(calledByIFluidUIContextManager: true);
        new FluidUIDesignManager(this.viewModel).Reset(calledByIFluidUIContextManager: true);
        new FluidUIFontManager(this.viewModel).Reset(calledByIFluidUIContextManager: true);
        new FluidUISizeManager(this.viewModel).Reset(calledByIFluidUIContextManager: true);
        new FluidUIStandManager(this.viewModel).Reset(calledByIFluidUIContextManager: true);

        this.viewModel.SetFluidUIByUser(this.viewModel.FluidUI);
    }

    internal void Apply_FluidUI(
         IFluidUIDataBlockModel? dataBlock = null,
         IFluidUIDesignModel? design = null,
         IFluidUIFontModel? font = null,
         IFluidUISizeModel? size = null,
         IFluidUIStandModel? stand = null)
    {
        this.Apply_FluidUIDataBlock(dataBlock);
        this.Apply_FluidUIDesign(design);
        this.Apply_FluidUIFont(font);
        this.Apply_FluidUISize(size);
        this.Apply_FluidUIStand(stand);
    }

    internal void Apply_FluidUIDataBlock(IFluidUIDataBlockModel? dataBlock = null)
    {
        if (dataBlock != null)
        {
            this.viewModel.FluidUI.DataBlock = (FluidUIDataBlockModel)dataBlock;

            return;
        }

        this.viewModel.FluidUI.DataBlock = new FluidUIDataBlockModel();
        this.viewModel.FluidUI.DataBlock.SetInitialValues();
    }

    internal void Apply_FluidUIDesign(IFluidUIDesignModel? design = null)
    {
        if (design != null)
        {
            this.viewModel.FluidUI.Design = (FluidUIDesignModel)design;

            return;
        }

        this.viewModel.FluidUI.Design = new FluidUIDesignModel();
        this.viewModel.FluidUI.Design.SetInitialValues();
    }

    internal void Apply_FluidUIFont(IFluidUIFontModel? font = null)
    {
        if (font != null)
        {
            this.viewModel.FluidUI.Font = (FluidUIFontModel)font;

            return;
        }

        this.viewModel.FluidUI.Font = new FluidUIFontModel();
        this.viewModel.FluidUI.Font.SetInitialValues();
    }

    internal void Apply_FluidUISize(IFluidUISizeModel? size = null)
    {
        if (size != null)
        {
            this.viewModel.FluidUI.Size = (FluidUISizeModel)size;

            return;
        }

        this.viewModel.FluidUI.Size = new FluidUISizeModel();
        this.viewModel.FluidUI.Size.SetInitialValues();
    }

    internal void Apply_FluidUIStand(IFluidUIStandModel? stand = null)
    {
        if (stand != null)
        {
            this.viewModel.FluidUI.Stand = (FluidUIStandModel)stand;

            return;
        }

        this.viewModel.FluidUI.Stand = new FluidUIStandModel();
        this.viewModel.FluidUI.Stand.SetInitialValues();
    }

    internal async Task<IFluidUIContext?> GetFluidUIConfigurationFromFile(FluidUISelectionViewModel configuration)
    {
        var folder = "eboard/fluidui/";
        var helper = new SharedMethod_UI();

        var filename = await helper.GetFileToLoad(folder, $"files (*.{helper.FluidUIConfigurationFileExtension})|*.{helper.FluidUIConfigurationFileExtension}");

        if (filename != null && Loader.FileExists(filename))
        {
            var fluiduiconfig = await Loader.LoadJsonFile<FluidUIContext>(filename);

            if (fluiduiconfig != null)
            {
                this.ProcessConfigurationTarget(fluiduiconfig, configuration);

                return fluiduiconfig;
            }
        }

        return null;
    }

    internal async Task LoadFluidUIConfiguration(FluidUISelectionViewModel configuration)
    {
        var fluiduiconfig = await this.GetFluidUIConfigurationFromFile(configuration);

        if (fluiduiconfig != null)
        {
            this.viewModel.SetFluidUIByUser(fluiduiconfig);
        }
    }

    internal IFluidUIContext GetCustomFluidUIConfiguration(FluidUISelectionViewModel configuration)
    {
        var manager = new FluidUIDeepCopyManager();
        var copy = manager.DeepCopyIFluidUIContext(this.viewModel.FluidUI);

        this.ProcessConfigurationTarget(copy, configuration);

        return copy;
    }

    internal IFluidUIContext GetCustomFluidUIContextCopy(FluidUISelectionViewModel configuration, IFluidUIContext fluidUIContext)
    {
        var manager = new FluidUIDeepCopyManager();
        var copy = manager.DeepCopyIFluidUIContext(fluidUIContext);

        this.ProcessConfigurationTarget(copy, configuration);

        return copy;
    }

    internal async Task SaveFluidUIConfiguration(FluidUISelectionViewModel configuration)
    {
        var copy = this.GetCustomFluidUIConfiguration(configuration);

        var helper = new SharedMethod_UI();
        var path = await new SharedMethod_UI().SetSaveFileName(helper.FluidUIConfigurationFileExtension);

        _ = Saver.SaveJsonFile(path, copy);
    }

    private void ProcessConfigurationTarget(IFluidUIContext fluiduiconfig, FluidUISelectionViewModel configuration)
    {
        if (configuration.All.Selected)
        {
            return;
        }

        if (!configuration.DataBlock.Selected)
        {
            fluiduiconfig.DataBlock = null;
        }

        if (!configuration.Design.Selected)
        {
            fluiduiconfig.Design = null;
        }

        if (!configuration.Font.Selected)
        {
            fluiduiconfig.Font = null;
        }

        if (!configuration.Size.Selected)
        {
            fluiduiconfig.Size = null;
        }

        if (!configuration.Stand.Selected)
        {
            fluiduiconfig.Stand = null;
        }
    }

}

// EOF