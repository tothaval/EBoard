// <copyright file="FluidUIDOPManager.cs" company=".">
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
namespace EBoardSDK.Models.DragnDropManager;
using EBoardConfigManager.Helper;
using EBoardSDK.ViewModels;
using System;
using System.IO;
using System.Windows;

internal class FluidUIDOPManager : DropObjectProcessingBase
{
    // TODO: erweitern, so dass auch fluidUI daten aus edf dateien geladen werden koennen
    internal override EboardFeedbackMessage ProcessDrop(FileInfo fileInfo, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "processing initiated",
        };

        try
        {
            if (Loader.LoadJsonFile<FluidUIContext>(fileInfo.FullName).Result is FluidUIContext fluidui)
            {
                var manager = new FluidUIContextManager(dropTargetContextAreaViewModel);

                if (dropTargetContextAreaViewModel.FluidUIMenuViewModel != null)
                {
                    manager.ApplyConfigurationTarget(fluidui, dropTargetContextAreaViewModel.FluidUIMenuViewModel.FluidUIDataBlockSetupViewModel.SetupViewModel.LoadSelectionViewModel);
                }

                dropTargetContextAreaViewModel.SetFluidUIByUser(fluidui);

                var successmessage = $"fluidUI configuration file {fileInfo.Name} applied to target: {dropTargetContextAreaViewModel.ContextArea}, {dropTargetContextAreaViewModel.FluidUI.DataBlock?.Title}";

                mainViewModel?.WriteToMessageStrip(successmessage);

                result.TaskResult = EBoardTaskResult.Success;
                result.ResultMessage = successmessage;

                return result;
            }

            // var n = Loader.LoadJsonFile<EboardConfig?>(fileInfo.FullName).Result;

            // if (n is EboardConfig eboard)
            // {
            //     this.viewModel.SetFluidUIByUser(eboard.EBoardContext);
            //     continue;
            // }
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Success;
            result.ResultMessage = $"exception occured while applying FluidUI data: {ex.Message}";

            return result;
        }

        // if (Loader.LoadJsonFile<EboardScreen>(fileInfo.FullName).Result is EboardScreen screen)
        // {
        //     this.viewModel.SetFluidUIByUser(screen.EBoardScreenContext);
        //     continue;
        // }

        // if (Loader.LoadJsonFile<ElementConfig>(fileInfo.FullName).Result is ElementConfig element)
        // {
        //     this.viewModel.SetFluidUIByUser(element.ElementContext);
        //     continue;
        // }

        result.TaskResult = EBoardTaskResult.Failure;
        result.ResultMessage = "FluidUI data wasn't loaded";

        return result;
    }
}

// EOF