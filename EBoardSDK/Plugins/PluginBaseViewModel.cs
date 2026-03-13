// <copyright file="PluginBaseViewModel.cs" company=".">
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

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// This class inherits <see cref="ObservableObject"/> and implements the
/// <see cref="IPlugin"/> interface. It is used to integrate plugins into
/// the eboard prototype plugin architecture.
/// <para/>
/// Plugins can save and load content if implementations are added to the
/// overrides of the save and load functions in the deriving Plugin main
/// view model.
/// <para/>
/// A Plugin can have a MenuItem that is integrated into the Element CAs
/// right-click context menu and use the Element CAs tooltip by not adding
/// a Menu or a ToolTip to the Plugin main view.
/// <para/>
/// They can either use the Element CAs FluidUI or the <see cref="FluidUI"/> property.
/// </summary>
public abstract partial class PluginBaseViewModel : FluidUIBaseViewModel, IPlugin
{
    private readonly FluidUIContextAreas contextArea = FluidUIContextAreas.Plugin;

    private bool containerIsArea = false;

    private bool menuItemSet = false;

    private ObservableObject? menuItemViewModel;

    private UserControl? menuItem;

    private ElementViewModel? elementViewModel;

    private object? pluginModel;

    /// <inheritdoc/>
    public abstract Assembly? PluginAssembly { get; }

    /// <inheritdoc/>
    public abstract Type? PluginModelType { get; }

    /// <inheritdoc/>
    public abstract Type PluginViewModelType { get; }

    /// <inheritdoc/>
    public object? PluginModel => this.pluginModel;

    /// <inheritdoc/>
    public InstantiationAndCopyConstraints? ScreenInstantiationConstraints { get; set; } = new InstantiationAndCopyConstraints(InstantiationPolicy.ValueNotSet, CopyConstraints.ValueNotSet);

    /// <inheritdoc/>
    public abstract PluginCategories Category { get; }

    /// <inheritdoc/>
    public abstract ImageBrush? Logo { get; set; }

    /// <inheritdoc/>
    public abstract string Name { get; }

    /// <inheritdoc/>
    public abstract string Header { get; }

    /// <inheritdoc/>
    public abstract ResourceDictionary ResourceDictionary { get; }

    /// <inheritdoc/>
    public virtual ObservableObject? MenuItemViewModel => this.menuItemViewModel;

    /// <inheritdoc/>
    public virtual UserControl? MenuItem => this.menuItem;

    /// <inheritdoc/>
    public bool ContainerIsArea => this.containerIsArea;

    /// <inheritdoc/>
    public bool MenuItemSet => this.menuItemSet;

    /// <inheritdoc/>
    public ElementViewModel? ElementViewModel => this.elementViewModel;

    /// <inheritdoc/>
    public override FluidUIContextAreas ContextArea => this.contextArea;

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();

        this.elementViewModel = null;
        this.menuItem = null;
        this.menuItemViewModel = null;
        this.Logo = null;
        this.pluginModel = null;
        this.ScreenInstantiationConstraints = null;
    }

    /// <inheritdoc/>
    public virtual void Duplicate()
    {
        if (this.ElementViewModel == null)
        {
            return;
        }

        new SDKPluginManager().DuplicatePlugin(this.ElementViewModel);
    }

    /// <inheritdoc/>
    public async Task<bool> Initialize()
    {
        try
        {
            var path = await new SDKDataManager().GetInstalledPluginsDirectory();

            if (string.IsNullOrWhiteSpace(this.Name) || string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var resourcestring = Path.Combine(path, $"{this.Name}_Logo.png");

            var imagefile = new FileInfo(resourcestring);

            if (!imagefile.Exists)
            {
                return false;
            }

            var imagesource = new BitmapImage(new Uri(imagefile.FullName));

            this.Logo = new ImageBrush(imagesource);

            return true;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        return false;
    }

    /// <inheritdoc/>
    public virtual void InsertModel<T>(T model)
    {
    }

    /// <inheritdoc/>
    public abstract Task<EboardFeedbackMessage> Load(string path);

    /// <inheritdoc/>
    public abstract Task<EboardFeedbackMessage> Save(string path);

    /// <inheritdoc/>
    public virtual void PrepareCopy()
    {
    }

    /// <inheritdoc/>
    public virtual void RefreshInitialization()
    {
        this.ViewWasSet();
    }

    /// <inheritdoc/>
    public void SetContainerIsArea(bool containerIsArea = true)
    {
        this.containerIsArea = containerIsArea;
        this.OnPropertyChanged(nameof(this.ContainerIsArea));
    }

    /// <inheritdoc/>
    public void SetElementViewModel(ElementViewModel elementViewModel)
    {
        this.elementViewModel = elementViewModel;

        this.OnPropertyChanged(nameof(this.ElementViewModel));
    }

    /// <inheritdoc/>
    public void SetMenuItem(UserControl menuItem)
    {
        this.menuItem = menuItem;

        this.menuItemSet = this.MenuItemViewModel != null && this.MenuItem != null;

        this.OnPropertyChanged(nameof(this.MenuItem));
        this.OnPropertyChanged(nameof(this.MenuItemSet));
        this.OnPropertyChanged(nameof(this.MenuItemViewModel));
    }

    /// <inheritdoc/>
    public void SetMenuItemViewModel(ObservableObject menuItemViewModel)
    {
        this.menuItemViewModel = menuItemViewModel;

        this.menuItemSet = this.MenuItemViewModel != null && this.MenuItem != null;

        this.OnPropertyChanged(nameof(this.MenuItem));
        this.OnPropertyChanged(nameof(this.MenuItemSet));
        this.OnPropertyChanged(nameof(this.MenuItemViewModel));
    }

    /// <inheritdoc/>
    public void SetModel(object model)
    {
        this.pluginModel = model;

        this.OnPropertyChanged(nameof(this.PluginModel));
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.OnPropertyChanged(nameof(this.ElementViewModel));
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.MenuItem));
        this.OnPropertyChanged(nameof(this.MenuItemSet));
        this.OnPropertyChanged(nameof(this.MenuItemViewModel));
    }

    /// <inheritdoc/>
    public virtual void ViewWasSet()
    {
    }
}

// EOF