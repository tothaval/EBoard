// <copyright file="TextFileDOPManager.cs" company=".">
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
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.ViewModels;
using Serilog;
using System;
using System.IO;
using System.Windows;

internal class TextFileDOPManager : DropObjectProcessingBase
{
    internal override EboardFeedbackMessage ProcessDrop(FileInfo fileInfo, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, Point? coords = null)
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

        var textmodel = this.GetTextModel(fileInfo).Result;

        if (textmodel == null)
        {
            result.TaskResult = EBoardTaskResult.Failure;
            result.ResultMessage = $"TextFileDOPManager.GetTextModel(FileInfo fileInfo) returned null";

            return result;
        }

        switch (dropTargetContextAreaViewModel.ContextArea)
        {
            case Enums.FluidUIContextAreas.Eboard:
                result = this.ProcessEboardDropRequest(textmodel, dropTargetContextAreaViewModel, mainViewModel, activeScreen);

                return result;
            case Enums.FluidUIContextAreas.Screen:
                result = this.ProcessScreenDropRequest(textmodel, dropTargetContextAreaViewModel, mainViewModel, activeScreen, coords);

                return result;
            case Enums.FluidUIContextAreas.Element:
                result = this.ProcessElementDropRequest(textmodel, dropTargetContextAreaViewModel, mainViewModel, activeScreen);

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

    private async Task<StandardTextModel?> GetTextModel(FileInfo fileInfo)
    {
        var textmodel = new StandardTextModel();

        try
        {
            textmodel = await Loader.LoadJsonFile<StandardTextModel>(fileInfo.FullName);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            textmodel = null;
        }

        return textmodel;
    }

    private async Task<EboardFeedbackMessage> InvokeStandardTextPlugin(StandardTextModel standardTextModel, ScreenViewModel activeScreen, bool directlyPlayFile, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "EboardDropRequest processing initiated",
        };

        var manager = new SDKPluginManager();
        var plugin = await manager.InvokePluginByName("StandardText") as StandardTextViewModel;

        if (plugin != null)
        {
            plugin.InsertModel(standardTextModel);
        }

        manager.InvokePluginOnEboard(plugin, activeScreen, coords);

        result.TaskResult = EBoardTaskResult.Success;
        result.ResultMessage = $"text file drop succesul";

        return result;
    }

    private EboardFeedbackMessage ProcessEboardDropRequest(StandardTextModel standardTextModel, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, ScreenViewModel activeScreen)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "EboardDropRequest processing initiated",
        };

        try
        {
            result = this.InvokeStandardTextPlugin(standardTextModel, activeScreen, true).Result;

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to apply text file, exception: {ex.Message}";

            return result;
        }
    }

    private EboardFeedbackMessage ProcessElementDropRequest(StandardTextModel standardTextModel, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, ScreenViewModel activeScreen)
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
                if (elementViewModel.Plugin.PluginViewModelType.Equals(typeof(StandardTextViewModel)))
                {
                    var standardText = elementViewModel.Plugin as StandardTextViewModel;

                    if (standardText != null)
                    {
                        standardText.InsertModel(standardTextModel);

                        result.TaskResult = EBoardTaskResult.Success;
                        result.ResultMessage = $"text file drop succesul";

                        return result;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to apply text file, exception: {ex.Message}";

            return result;
        }

        result.TaskResult = EBoardTaskResult.Failure;
        result.ResultMessage = $"text file drop unsuccesul";

        return result;
    }

    private EboardFeedbackMessage ProcessScreenDropRequest(StandardTextModel standardTextModel, FluidUIBaseViewModel dropTargetContextAreaViewModel, MainViewModel mainViewModel, ScreenViewModel activeScreen, Point? coords = null)
    {
        var result = new EboardFeedbackMessage()
        {
            TaskResult = EBoardTaskResult.Unknown,
            ResultMessage = "ScreenDropRequest processing initiated",
        };

        try
        {
            result = this.InvokeStandardTextPlugin(standardTextModel, activeScreen, true, coords).Result;

            return result;
        }
        catch (Exception ex)
        {
            result.TaskResult = EBoardTaskResult.Exception;
            result.ResultMessage = $"exception while trying to apply text file, exception: {ex.Message}";

            return result;
        }
    }
}

// EOF