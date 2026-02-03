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

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.ViewModels;
using EBoardSDK.Views;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// This interface inherits <see cref="IFluidUI"/> and contains properties
/// and methods that are required for Plugin architecture of the prototype.
/// </summary>
public interface IPlugin : IFluidUI
{
    /// <summary>
    /// Gets the assembly of the Plugin.
    /// </summary>
    public Assembly? PluginAssembly { get; }

    /// <summary>
    /// Gets the Plugin logo that is used e.g. on menu items.
    /// Plugins are not required to have a logo.
    /// </summary>
    public abstract ImageBrush? Logo { get; }

    /// <summary>
    /// Gets the type of the Plugin main view model
    /// that can be used for instantiation using
    /// <see cref="Activator.CreateInstance(Type)"/>.
    /// </summary>
    public Type PluginViewModelType { get; }

    /// <summary>
    /// Gets the type of the Plugin data model class
    /// that can be used for instantiation using
    /// <see cref="Activator.CreateInstance(Type)"/>.
    /// </summary>
    public Type? PluginModelType { get; }

    /// <summary>
    /// Gets the object of the Plugin data model class.
    /// </summary>
    public object? PluginModel { get; }

    /// <summary>
    /// Gets the InstantiationAndCopyConstraints of the Plugin.
    /// </summary>
    public abstract InstantiationAndCopyConstraints? ScreenInstantiationConstraints { get; }

    /// <summary>
    /// Gets the PluginCategories value of the Plugin.
    ///
    /// Be advised:
    /// <see cref="PluginCategories.Eboard"/> will be ignored if Plugin assembly is not EboardSDK.
    /// </summary>
    public abstract PluginCategories Category { get; }

    /// <summary>
    /// Gets the plugin header that can be used for displaying
    /// the Plugins name in a less limited format.
    /// </summary>
    public abstract string Header { get; }

    /// <summary>
    /// Gets the name of the plugin that is used internally
    /// for processing.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the Plugin has a
    /// menu item. The value is used for Visibility in
    /// <see cref="ElementViewModel"/> context menu.
    /// </summary>
    public abstract bool MenuItemSet { get; }

    /// <summary>
    /// Gets the ResourceDictionary of the Plugin.
    /// </summary>
    public ResourceDictionary ResourceDictionary { get; }

    /// <summary>
    /// Gets the Element CA containing the Plugin instance.
    /// </summary>
    public ElementViewModel? ElementViewModel { get; }

    /// <summary>
    /// Gets the view model for the Plugin context menu item.
    /// </summary>
    public ObservableObject? MenuItemViewModel { get; }

    /// <summary>
    /// Gets the view for the Plugin context menu item.
    /// </summary>
    public UserControl? MenuItem { get; }

    /// <summary>
    /// This task is used to load image file for PluginLogo and to add
    /// <see cref="ResourceDictionary"/> to the application during runtime.
    ///
    /// </summary>
    /// <returns>Returns true on success and false on failure.</returns>
    public Task<bool> Initialize();

    /// <summary>
    /// This function can be used to insert a model into the Plugin.
    /// </summary>
    /// <typeparam name="T">The type of the model class.</typeparam>
    /// <param name="model">The instance of the model class.</param>
    public void InsertModel<T>(T model);

    /// <summary>
    /// The task is called and awaited on program start and can trigger
    /// loading of data for the Plugin. If not needed, return an empty
    /// call in the implementation.
    /// </summary>
    /// <param name="path">Desired is the path to the location of the file that contains the Plugin model.</param>
    /// <returns>When awaited, it returns a <see cref="EboardFeedbackMessage"/> object,
    /// else a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public Task<EboardFeedbackMessage> Load(string path);

    /// <summary>
    /// Use this in deriving viewmodel to reload certain stuff that couldn't load upon
    /// constructor initiation. In current plugin architecture this is needed in some cases.
    ///
    /// Empty constructors are used for most plugins.
    /// Due to instantiaton time or missing object availability at the time the constructor
    /// is initiated, certain data or viewmodels can not be found or accessed.
    ///
    /// This function can be used to circumvent such object availability issues.
    /// </summary>
    public abstract void RefreshInitialization();

    /// <summary>
    /// The task is called and awaited on runtime end and in some cases
    /// before ctd and can trigger saving of data for the Plugin. If not
    /// needed, return an empty call in the implementation.
    /// </summary>
    /// <param name="path">Desired is the path where the file storing the Plugin model should be written to.</param>
    /// <returns>When awaited, it returns a <see cref="EboardFeedbackMessage"/> object,
    /// else a <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public Task<EboardFeedbackMessage> Save(string path);

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
    public void ViewWasSet();

    /// <summary>
    /// This function is used to trigger instantiation of new Plugins
    /// of the Plugin type.
    /// </summary>
    internal abstract void Duplicate();

    /// <summary>
    /// This function is used to trigger model building and assigning
    /// on users demand.
    /// </summary>
    internal abstract void PrepareCopy();

    /// <summary>
    /// This function sets values to <see cref="MenuItem"/> and <see cref="MenuItemViewModel"/>.
    /// It will switch <see cref="MenuItemSet"/> to true.
    /// </summary>
    /// <param name="menuItem">The usercontrol for the Plugin menu item that should be displayed in the Element CA context menu.</param>
    protected void SetMenuItem(UserControl menuItem);

    /// <summary>
    /// This function sets values to <see cref="MenuItem"/> and <see cref="MenuItemViewModel"/>.
    /// It will switch <see cref="MenuItemSet"/> to true.
    /// </summary>
    /// <param name="menuItemViewModel">The view model for the Plugin menu item that should be displayed in the Element CA context menu.</param>
    protected void SetMenuItemViewModel(ObservableObject menuItemViewModel);

    /// <summary>
    /// This function sets a value to <see cref="PluginModel"/>.
    /// </summary>
    /// <param name="model">The data model class instance object.</param>
    protected void SetModel(object model);
}

// EOF