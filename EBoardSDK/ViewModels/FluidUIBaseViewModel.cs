// <copyright file="FluidUIBaseViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Utilities;
using EBoardSDK.Windows;
using System.IO;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// This class implements IFluidUI interface and inherits ObservableObject.
/// It serves as base view model for all FluidUI Context Areas (CA).
///
/// It contains fields, properties and functions that are all about storing
/// FluidUI data or changing it. It has a <see cref="Type(FluidUIMenuViewModel)"/>
/// and two <see cref="IFluidUIContext"/>, one for display and one as storage
/// for a previous FluidUI configuration.
/// </summary>
public partial class FluidUIBaseViewModel : ObservableObject, IFluidUI
{
    private IFluidUIContext fluidUIBackUp = new FluidUIContext();

    private IFluidUIContext fluidUI = new FluidUIContext();

    protected FluidUIMenuViewModel? fluidUIMenuViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIBaseViewModel"/> class.
    /// </summary>
    public FluidUIBaseViewModel()
    {
        this.FluidUI.SetInitialValues();

        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    /// <inheritdoc/>
    public event Action? PropertyChangedEvent;

    /// <summary>
    /// Gets the model class that contains all FluidUI related properties
    /// of the FluidUI CA that inherits this base view model.
    /// </summary>
    public IFluidUIContext FluidUI => this.fluidUI;

    /// <summary>
    /// Gets the model class that contains all FluidUI related properties
    /// of the FluidUI CA that inherits this base view model.
    /// </summary>
    public virtual FluidUIContextAreas ContextArea { get; }

    public FluidUIMenuViewModel? FluidUIMenuViewModel => this.fluidUIMenuViewModel;

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        this.FluidUI.Dispose();
        this.fluidUIBackUp.Dispose();

        this.FluidUIMenuViewModel?.Dispose();
        this.fluidUIMenuViewModel = null;

        this.PropertyChangedEvent = null;
    }

    /// <inheritdoc/>
    public void SetInitialValues()
    {
        this.FluidUI.SetInitialValues();
    }

    /// <inheritdoc/>
    public void UpdateValues()
    {
        this.UpdateFluidUI();
    }

    internal virtual void BecomesActive()
    {
    }

    internal virtual void BecomesInactive()
    {
        this.FluidUIMenuViewModel?.Dispose();
        this.fluidUIMenuViewModel = null;
    }

    internal virtual void CreateFluidUIMenuViewModel()
    {
    }

    internal void DeleteFluidUIMenuViewModel()
    {
        this.FluidUIMenuViewModel?.Dispose();
    }

    internal void Drop(DragEventArgs e, FluidUIBaseViewModel fluidUIBaseViewModel)
    {
        string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];

        if (files == null)
        {
            return;
        }

        MainViewModel? viewModel = null;

        switch (this.ContextArea)
        {
            case FluidUIContextAreas.Eboard:
                var mainViewModel = fluidUIBaseViewModel as MainViewModel;
                if (mainViewModel != null)
                {
                    viewModel = mainViewModel;
                }

                break;
            case FluidUIContextAreas.Navigation:
                var nav = fluidUIBaseViewModel as NavigationContextViewModel;
                if (nav != null)
                {
                    viewModel = nav.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Screen:
                var screen = fluidUIBaseViewModel as ScreenViewModel;
                if (screen != null)
                {
                    viewModel = screen.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Element:
                var element = fluidUIBaseViewModel as ElementViewModel;

                if (element != null)
                {
                    viewModel = element.ScreenViewModel.MainViewModel;
                }

                break;
            case FluidUIContextAreas.Plugin:
            case FluidUIContextAreas.Unknown:
                break;
            default:
                break;
        }

        foreach (var item in files)
        {
            var fileInfo = new FileInfo(item);

            if (fileInfo.Exists)
            {
                var filetype = new SDKDataManager().filenameCheck(fileInfo);

                if (filetype == SupportedFileTypeCategories.Unknown || string.IsNullOrWhiteSpace(fileInfo.Extension))
                {
                    continue;
                }

                switch (filetype)
                {
                    case SupportedFileTypeCategories.Eboard:
                        // TODO search for active screen and correct model type and insert into free slot,
                        // if none can be found, create new one and insert

                        // TODO extension enum 
                        break;
                    case SupportedFileTypeCategories.FluidUI:

                        try
                        {
                            if (Loader.LoadJsonFile<FluidUIContext>(fileInfo.FullName).Result is FluidUIContext fluidui)
                            {
                                this.SetFluidUIByUser(fluidui);

                                viewModel?.WriteToMessageStrip($"fluidUI configuration file {fileInfo.Name} applied to target: {this.ContextArea}, {this.FluidUI.DataBlock?.Title}");

                                e.Handled = true;
                                break;
                            }

                            // var n = Loader.LoadJsonFile<EboardConfig?>(fileInfo.FullName).Result;

                            // if (n is EboardConfig eboard)
                            // {
                            //     this.viewModel.SetFluidUIByUser(eboard.EBoardContext);
                            //     continue;
                            // }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        // if (Loader.LoadJsonFile<EboardScreen>(fileInfo.FullName).Result is EboardScreen screen)
                        // {
                        //     this.viewModel.SetFluidUIByUser(screen.EBoardScreenContext);
                        //     continue;
                        // }

                        // if (Loader.LoadJsonFile<ElementConfig>(fileInfo.FullName).Result is ElementConfig element)
                        // {
                        //     this.viewModel.SetFluidUIByUser(element.ElementContext);
                        //     continue;
                        // }
                        break;
                    case SupportedFileTypeCategories.Image:
                        // TODO search for active screen and image or imageArea plugins and insert into free slot,
                        // if none can be found, create new one and insert
                        break;
                    case SupportedFileTypeCategories.Plugin:
                        // TODO if possible and allowed, install plugin and instantiate one instance
                        break;
                    case SupportedFileTypeCategories.Sound:
                        // TODO search for active screen and basicAV or SoundMix plugins and insert into free slot,
                        // if none can be found, create new one and insert
                        break;
                    case SupportedFileTypeCategories.Unknown:
                        break;
                    default:
                        break;
                }
            }
        }

        e.Handled = true;
    }

    internal virtual void ResetFluidUIToPrevious()
    {
        this.fluidUI = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.fluidUIBackUp);
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    internal void SetFluidUI(IFluidUIContext? fluidUIContext)
    {
        if (fluidUIContext == null)
        {
            return;
        }

        if (this.fluidUI == null)
        {
            this.fluidUI = new FluidUIContext();
            this.FluidUI.SetInitialValues();
        }

        if (fluidUIContext.DataBlock != null)
        {
            this.fluidUI.DataBlock = fluidUIContext.DataBlock;
            this.UpdateDataBlock(updateFluidUI: false);
        }

        if (fluidUIContext.Design != null)
        {
            this.fluidUI.Design = fluidUIContext.Design;
            this.UpdateDesign(updateFluidUI: false);
        }

        if (fluidUIContext.Font != null)
        {
            this.fluidUI.Font = fluidUIContext.Font;
            this.UpdateFont(updateFluidUI: false);
        }

        if (fluidUIContext.Size != null)
        {
            this.fluidUI.Size = fluidUIContext.Size;
            this.UpdateSize(updateFluidUI: false);
        }

        if (fluidUIContext.Stand != null)
        {
            this.fluidUI.Stand = fluidUIContext.Stand;
            this.UpdateStand(updateFluidUI: false);
        }

        this.UpdateFluidUI();
    }

    internal void SetFluidUIByUser(IFluidUIContext? fluidUIContext)
    {
        if (fluidUIContext != null)
        {
            this.fluidUIBackUp = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.FluidUI);

            this.SetFluidUI(fluidUIContext);
        }
    }

    internal void SetFluidUICopyByUser(IFluidUIContext fluidUIContext)
    {
        if (fluidUIContext != null)
        {
            var copy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(fluidUIContext);

            this.SetFluidUIByUser(copy);
        }
    }

    internal void SetFluidUIMenuViewModel(FluidUIMenuViewModel? fluidUIMenuViewModel)
    {
        if (fluidUIMenuViewModel != null)
        {
            this.fluidUIMenuViewModel = fluidUIMenuViewModel;
        }

        this.UpdateFluidUI();
    }

    internal virtual void Setup()
    {
    }

    internal virtual void TriggerRedraw()
    {
    }

    internal virtual void UpdateDataBlock(bool updateFluidUI = true)
    {
        this.OnPropertyChanged(nameof(this.FluidUI.DataBlock));
        this.FluidUIMenuViewModel?.FluidUIDataBlockSetupViewModel?.UpdateValues();

        if (updateFluidUI)
        {
            this.UpdateFluidUI();
        }
    }

    internal virtual void UpdateDesign(bool updateFluidUI = true)
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Design));
        this.FluidUIMenuViewModel?.FluidUIDesignSetupViewModel?.UpdateValues();

        if (updateFluidUI)
        {
            this.UpdateFluidUI();
        }
    }

    internal virtual void UpdateFont(bool updateFluidUI = true)
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Font));
        this.FluidUIMenuViewModel?.FluidUIFontSetupViewModel?.UpdateValues();

        if (updateFluidUI)
        {
            this.UpdateFluidUI();
        }
    }

    internal virtual void UpdateSize(bool updateFluidUI = true)
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Size));

        this.FluidUIMenuViewModel?.FluidUISizeSetupViewModel?.UpdateValues();

        if (updateFluidUI)
        {
            this.UpdateFluidUI();
        }
    }

    internal virtual void UpdateStand(bool updateFluidUI = true)
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Stand));

        this.FluidUIMenuViewModel?.FluidUIStandSetupViewModel?.UpdateValues();

        if (updateFluidUI)
        {
            this.UpdateFluidUI();
        }

        this.TriggerRedraw();
    }

    internal virtual void UpdateFluidUI()
    {
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }
}

// EOF