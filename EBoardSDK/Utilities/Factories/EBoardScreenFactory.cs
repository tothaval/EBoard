// <copyright file="EBoardScreenFactory.cs" company=".">
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
namespace EBoardSDK.Utilities.Factories;

using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.ViewModels;
using Serilog;
using System.IO;
using System.Windows;

public static class EBoardScreenFactory
{
    public static EBoardViewModel GetEBoardViewModelByEboardScreen(EboardScreen eboardScreen, MainViewModel mainViewModel)
    {
        if (eboardScreen.EBoardScreenContext == null)
        {
            eboardScreen.EBoardScreenContext = new FluidUIContext();
            eboardScreen.EBoardScreenContext.SetInitialValues();
        }

        var eBoardViewModel = new EBoardViewModel(mainViewModel, eboardScreen);

        ObservableCollection<ElementViewModel> elementViewModels = [];

        eboardScreen.Elements.Select(x => x).ToList().ForEach(
        async element =>
        {
            ElementViewModel elementViewModel = new ElementViewModel(eBoardViewModel, element);

            Type? type_PluginViewModel = Type.GetType(element.AssemblyName);

            if (type_PluginViewModel != null)
            {
                EBoardElementPluginBaseViewModel? externalPlugin = Activator.CreateInstance(type_PluginViewModel) as EBoardElementPluginBaseViewModel;

                if (externalPlugin != null)
                {
                    externalPlugin.SetEBoardAndElementViewModel(eBoardViewModel, elementViewModel);

                    externalPlugin.RefreshInitialization();

                    externalPlugin.PluginHeader = element.PluginHeader;

                    var contentPath = element.ContentFilePath;

                    if (!string.IsNullOrWhiteSpace(contentPath))
                    {
                        await externalPlugin.Load(contentPath);
                    }

                    try
                    {
                        var app = Application.Current;

                        app?.Resources?.MergedDictionaries?.Add(externalPlugin.ResourceDictionary);

                        elementViewModel.Plugin = externalPlugin;
                    }
                    catch (IOException ioex)
                    {
                        var ioexAdditionalMessage = string.Join(
                            $"\n__{type_PluginViewModel}\t",
                            $"plugin load error: {externalPlugin.PluginName}",
                            "ResourceDictionary path or file is corrupt");

                        Log.Error(ioex, ioexAdditionalMessage);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "unhandled exception");
                        throw;
                    }
                }
            }

            elementViewModel.Redraw();

            elementViewModels.Add(elementViewModel);
        });

        eBoardViewModel.Elements = elementViewModels;

        return eBoardViewModel;
    }

    public static EboardScreen GetNewEboardScreen(string name, int depth, double width, double height)
    {
        var fluidui = new FluidUIContext();
        fluidui.SetInitialValues();

        if (fluidui.DataBlock != null)
        {
            fluidui.DataBlock.Title = name;
        }

        if (fluidui.Size != null)
        {
            fluidui.Size.Width = width;
            fluidui.Size.Height = height;
        }

        if (fluidui.Stand != null)
        {
            fluidui.Stand.Zmaximum = depth;
        }

        return new EboardScreen
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardScreenContext = fluidui,
        };
    }
}

// EOF