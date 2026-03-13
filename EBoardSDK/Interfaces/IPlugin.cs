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

public interface IPlugin : IFluidUI
{
    public Assembly? ElementPluginAssembly { get; }

    public abstract ImageBrush PluginLogo { get; set; }

    public Type? ElementPluginModel { get; }

    public Type ElementPluginView { get; }

    public Type ElementPluginViewModel { get; }

    public abstract ElementScreenIntegrationConstraints? ElementScreenIntegrationConstraints { get; set; }

    public abstract bool NoDefaultBorders { get; }

    public abstract UserControl Plugin { get; }

    public abstract PluginCategories PluginCategory { get; }

<<<<<<< Updated upstream
    public abstract string PluginHeader { get; set; }

    public abstract string PluginName { get; set; }
=======
    /// <summary>
    /// Gets a value indicating whether the Plugin is
    /// instantiated within an area or not.
    /// <see cref="ElementViewModel"/> context menu.
    /// </summary>
    public bool ContainerIsArea { get; }

    /// <summary>
    /// Gets a value indicating whether the Plugin has a
    /// menu item. The value is used for Visibility in
    /// <see cref="ElementViewModel"/> context menu.
    /// </summary>
    public abstract bool MenuItemSet { get; }
>>>>>>> Stashed changes

    public ResourceDictionary ResourceDictionary { get; }

    public EBoardViewModel EBoardViewModel { get; }

    public ElementViewModel ElementViewModel { get; }

    public Task<bool> Initialize();

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public Task<EBoardFeedbackMessage> Load(string path);

    /// <summary>
    ///
    /// </summary>
    /// <param name="path"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public Task<EBoardFeedbackMessage> Save(string path);

    public abstract void RefreshInitialization();

    public void SetEBoardAndElementViewModel(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel);

<<<<<<< Updated upstream
=======
    /// <summary>
    /// This function serves as a setter for the <see cref="IPlugin.ContainerIsArea"/> property.
    /// </summary>
    /// <param name="containerIsArea">Indicates whether the plugin is instanciated within an area
    /// control or not.</param>
    public void SetContainerIsArea(bool containerIsArea = true);

    /// <summary>
    /// This function serves as a setter for the <see cref="ElementViewModel"/> instance property
    /// that contains the Plugin.
    /// </summary>
    /// <param name="elementViewModel">Desired is the Element CA instance that contains the Plugin.</param>
    public void SetElementViewModel(ElementViewModel elementViewModel);

    /// <summary>
    /// This function is used to apply a <see cref="IFluidUIContext"/> to a field
    /// for the inherited <see cref="IFluidUI.FluidUI"/> property.
    /// </summary>
    /// <param name="fluidUIContext">The <see cref="IFluidUIContext"/> that replaces the old value.</param>
    public void SetFluidUI(IFluidUIContext fluidUIContext);

    /// <summary>
    /// This function is called when the <see cref="ElementView"/> was set in the
    /// Element CA that contains the Plugin.
    /// </summary>
>>>>>>> Stashed changes
    public void ViewWasSet();
}

// EOF