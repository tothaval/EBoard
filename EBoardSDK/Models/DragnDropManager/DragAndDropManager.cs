// <copyright file="DragAndDropManager.cs" company=".">
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
using EBoardSDK.Interfaces;
using EBoardSDK.ViewModels;
using System.Windows;

public class DragAndDropManager
{
    private FluidUIBaseViewModel dropTargetContextAreaViewModel;

    public DragAndDropManager(FluidUIBaseViewModel dropTargetContextAreaViewModel)
    {
        this.dropTargetContextAreaViewModel = dropTargetContextAreaViewModel;
    }

    public EboardFeedbackMessage ProcessDropObjects(System.Windows.DragEventArgs e, Point? coords = null, BrushTargets? brushTarget = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "processing initiated",
        };

        if (e == null)
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = "System.Windows.DragEventArgs object is null";

            return result;
        }

        string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];

        if (files == null)
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = "drop data parsing returned null";

            e.Handled = true;

            return result;
        }

        var mainViewModel = this.GetMainViewModelFromFluidUIBaseViewModel();

        if (mainViewModel == null)
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = "MainViewModel instance could not be found";

            e.Handled = true;

            // kritisch oder nicht? scheint nur die messagestripausgabe betroffen zu sein
            return result;
        }

        // TODO: improve messaging or use external libraries.
        var results = new DropObjectSplitter().ProcessDropFiles(files, this.dropTargetContextAreaViewModel, mainViewModel, coords, brushTarget);

        e.Handled = true;

        result.TaskResult = EBoardTaskResult.Unknown;
        result.ResultMessage = "drop file processing finished";

        return result;
    }

    private MainViewModel? GetMainViewModelFromFluidUIBaseViewModel()
    {
        MainViewModel? viewModel = null;

        switch (this.dropTargetContextAreaViewModel.ContextArea)
        {
            case FluidUIContextAreas.Eboard:
                var mainViewModel = this.dropTargetContextAreaViewModel as MainViewModel;
                if (mainViewModel != null)
                {
                    viewModel = mainViewModel;
                }

                break;
            case FluidUIContextAreas.Navigation:
                var nav = this.dropTargetContextAreaViewModel as NavigationContextViewModel;
                if (nav != null)
                {
                    viewModel = nav.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Screen:
                var screen = this.dropTargetContextAreaViewModel as ScreenViewModel;
                if (screen != null)
                {
                    viewModel = screen.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Element:
                var element = this.dropTargetContextAreaViewModel as ElementViewModel;

                if (element != null)
                {
                    viewModel = element.ScreenViewModel.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Plugin:
                var plugin = this.dropTargetContextAreaViewModel as IPlugin;

                if (plugin != null)
                {
                    viewModel = plugin.ElementViewModel?.ScreenViewModel.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Unknown:
                break;
            default:
                break;
        }

        return viewModel;
    }
}

// EOF