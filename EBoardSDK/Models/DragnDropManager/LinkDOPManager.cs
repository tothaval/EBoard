// <copyright file="LinkDOPManager.cs" company=".">
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

using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

internal class LinkDOPManager : DropObjectProcessingBase
{
    internal override EboardFeedbackMessage ProcessDrop(FileInfo fileInfo, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null)
    {
        var result = this.ProcessDrop(fileInfo.FullName, dropTargetContextAreaViewModel, mainViewModel, coords);
        return result;
    }

    internal EboardFeedbackMessage ProcessDrop(string path, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "processing initiated",
        };

        var activeScreen = mainViewModel.GetActiveScreen();

        if (activeScreen == null)
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = $"MainViewModel.GetActiveScreen() returned null";

            return result;
        }

        switch (dropTargetContextAreaViewModel.ContextArea)
        {
            case Enums.FluidUIContextAreas.Eboard:
                result = this.ProcessEboardDropRequest(path, activeScreen);

                return result;
            case Enums.FluidUIContextAreas.Screen:
                result = this.ProcessScreenDropRequest(path, activeScreen, coords);

                return result;
            case Enums.FluidUIContextAreas.Element:
                result = this.ProcessElementDropRequest(path, dropTargetContextAreaViewModel, activeScreen);

                return result;
            case Enums.FluidUIContextAreas.Navigation:
            case Enums.FluidUIContextAreas.Plugin:
            case Enums.FluidUIContextAreas.Unknown:
                result.TaskResult = EBoardTaskResult.Failure;
                result.ResultMessage = $"unprocessable context area: {dropTargetContextAreaViewModel.ContextArea}";

                return result;
            default:
                result.TaskResult = EBoardTaskResult.Failure;
                result.ResultMessage = $"unprocessable request, no Enums.FluidUIContextAreas match";
                break;
        }

        return result;
    }

    private async Task<EboardFeedbackMessage> InvokeLinkPlugin(string path, ScreenViewModel activeScreen, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "EboardDropRequest processing initiated",
        };

        if (string.IsNullOrWhiteSpace(path))
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = "received empty or null string as path, processing aborted.";

            return result;
        }

        var manager = new SDKPluginManager();
        var plugin = await manager.InvokePluginByName("Link") as LinkViewModel;

        if (plugin != null)
        {
            result = this.DetermineLinkType(path, plugin, activeScreen);

            manager.InvokePluginOnEboard(plugin, activeScreen, coords);

            return result;
        }

        result.TaskResult = EBoardTaskResult.Failure;
        result.ResultMessage = $"path could not be resolved";

        return result;
    }

    private EboardFeedbackMessage DetermineLinkType(string path, LinkViewModel plugin, ScreenViewModel activeScreen)
    {
        var manager = new SDKPluginManager();
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "DetermineLinkType processing initiated",
        };

        var file = new FileInfo(path);
        if (file != null && file.Exists)
        {
            plugin.SetFileLink(file);

            result.TaskResult = EBoardTaskResult.Success;
            result.ResultMessage = $"link plugin created";

            return result;
        }

        var dir = new DirectoryInfo(path);
        if (dir != null && dir.Exists)
        {
            plugin.SetDirectoryLink(dir);

            result.TaskResult = EBoardTaskResult.Success;
            result.ResultMessage = $"link plugin created";

            return result;
        }

        result.TaskResult = EBoardTaskResult.Failure;
        result.ResultMessage = $"path could not be resolved";

        return result;
    }

    private EboardFeedbackMessage ProcessEboardDropRequest(string path, ScreenViewModel activeScreen)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "EboardDropRequest processing initiated",
        };

        try
        {
            result = this.InvokeLinkPlugin(path, activeScreen).Result;

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to create link plugin. Exception: {ex.Message}";

            return result;
        }
    }

    private EboardFeedbackMessage ProcessElementDropRequest(string path, FluidUIBaseViewModel dropTargetContextAreaViewModel, ScreenViewModel activeScreen)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "ElementDropRequest processing initiated",
        };

        try
        {
            var elementViewModel = dropTargetContextAreaViewModel as ElementViewModel;

            if (elementViewModel != null && elementViewModel.Plugin != null)
            {
                if (elementViewModel.Plugin.PluginViewModelType.Equals(typeof(LinkViewModel)))
                {
                    var plugin = elementViewModel.Plugin as LinkViewModel;

                    if (plugin != null)
                    {
                        result = this.DetermineLinkType(path, plugin, activeScreen);
                        return result;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to create link plugin. Exception: {ex.Message}";

            return result;
        }

        result.TaskResult = EBoardTaskResult.Failure;
        result.ResultMessage = $"media file drop unsuccesul";

        return result;
    }

    private EboardFeedbackMessage ProcessScreenDropRequest(string path, ScreenViewModel activeScreen, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "ScreenDropRequest processing initiated",
        };

        try
        {
            result = this.InvokeLinkPlugin(path, activeScreen, coords).Result;

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to create link plugin. Exception: {ex.Message}";

            return result;
        }
    }
}

// EOF