// <copyright file="IPlugin.cs" company=".">
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
namespace EBoardSDK.Interfaces;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.ViewModels;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public interface IPlugin
{
    public Assembly? ElementPluginAssembly { get; }

    public abstract ImageBrush PluginLogo { get; set; }

    public Type? ElementPluginModel { get; }

    public string ElementPluginName { get; }

    public Type ElementPluginView { get; }

    public Type ElementPluginViewModel { get; }

    public abstract ElementScreenIntegrationConstraints? ElementScreenIntegrationConstraints { get; set; }

    public abstract bool NoDefaultBorders { get; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

    public abstract string PluginHeader { get; set; }

    public abstract string PluginName { get; set; }

    public ResourceDictionary ResourceDictionary { get; }

    public EBoardViewModel EBoardViewModel { get; }

    public ElementViewModel ElementViewModel { get; }

    public Task<EBoardFeedbackMessage> Load(string path);

    public Task<EBoardFeedbackMessage> Save(string path);

    public abstract void RefreshInitialization();

    public void SetEBoardAndElementViewModel(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel);
}

// EOF