// <copyright file="ImageFileDOPManager.cs" company=".">
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

using EBoardSDK.Enums;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System;
using System.IO;
using System.Windows;

internal class ImageFileDOPManager : DropObjectProcessingBase
{
    internal EboardFeedbackMessage ProcessBrushDrop(FileInfo fileInfo, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, BrushTargets? brushTarget = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "processing initiated",
        };

        try
        {
            var imageBrush = FluidUIDesignDefaultPropertyFactory.CreateImageBrush(fileInfo.FullName);

            var manager = new FluidUIDesignManager(dropTargetContextAreaViewModel);

            manager.SetBrush(imageBrush, brushTarget ?? BrushTargets.Background);

            result.TaskResult = EBoardTaskResult.Success;
            result.ResultMessage = $"image file applied as brush to {dropTargetContextAreaViewModel.ContextArea}";

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to apply image file as background to {dropTargetContextAreaViewModel.ContextArea}. Exception: {ex.Message}";

            return result;
        }
    }

    internal override EboardFeedbackMessage ProcessDrop(FileInfo fileInfo, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "processing initiated",
        };

        try
        {
            var imageBrush = FluidUIDesignDefaultPropertyFactory.CreateImageBrush(fileInfo.FullName);

            var manager = new FluidUIDesignManager(dropTargetContextAreaViewModel);

            manager.SetBrush(imageBrush, BrushTargets.Background);

            result.TaskResult = EBoardTaskResult.Success;
            result.ResultMessage = $"image file applied as brush to {dropTargetContextAreaViewModel.ContextArea}";

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to apply image file as background to {dropTargetContextAreaViewModel.ContextArea}. Exception: {ex.Message}";

            return result;
        }
    }
}

// EOF