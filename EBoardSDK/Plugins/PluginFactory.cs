// <copyright file="PluginFactory.cs" company=".">
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
namespace EBoardSDK.Plugins;

using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.StandardText;

using System.Windows.Controls;

internal static class PluginFactory
{
    internal static IPlugin? GetPluginByCommand(string pluginCategory, string command)
    {
        string pluginView = string.Concat($"EBoardSDK.Plugins.{pluginCategory}.{command}.{command}View");
        string pluginViewModel = string.Concat($"EBoardSDK.Plugins.{pluginCategory}.{command}.{command}ViewModel");

        Type? type_PluginView = Type.GetType(pluginView);
        Type? type_PluginViewModel = Type.GetType(pluginViewModel);

        if (type_PluginView is null || type_PluginViewModel is null)
        {
            return null;
        }

        UserControl? pluginViewInstance = (UserControl)Activator.CreateInstance(type_PluginView);
        IPlugin? plugin = Activator.CreateInstance(type_PluginViewModel) as IPlugin;

        if (pluginViewInstance is not null && plugin is not null)
        {
            // TODO fluidUIDesign is not the viewmodel
            pluginViewInstance.DataContext = plugin;

            return plugin;
        }

        return new StandardTextViewModel()
        {
            Text = "Plugin Instantiation Error",
        };
    }
}

// EOF