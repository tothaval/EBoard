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
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DragnDropManager;
using System.Windows;

/// <summary>
/// This class implements IFluidUI interface and inherits ObservableObject.
/// It serves as base view model for all FluidUI Context Areas (CA).
///
/// It contains fields, properties and functions that are all about storing
/// FluidUI data or changing it. It has a <see cref="Type(FluidUIMenuViewModel)"/>
/// and two <see cref="IFluidUIContext"/>, one for display and one as storage
/// for a previous FluidUI configuration.
/// </summary>
public partial class FluidUIBaseViewModel : ObservableObject, IFluidUI, IHideControl
{
    private IFluidUIContext fluidUIBackUp = new FluidUIContext();

    private IFluidUIContext fluidUI = new FluidUIContext();

    // the HideControl# flags are for optional configuration
    // by the user to deactivate ui functions per instance
    [ObservableProperty]
    private bool hideControl0 = false;

    [ObservableProperty]
    private bool hideControl1 = false;

    [ObservableProperty]
    private bool hideControl2 = false;

    [ObservableProperty]
    private bool hideControl3 = false;

    [ObservableProperty]
    private bool hideControl4 = false;

    [ObservableProperty]
    private bool hideControl5 = false;

    [ObservableProperty]
    private bool hideControl6 = false;

    [ObservableProperty]
    private bool hideControl7 = false;

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

    public void SetFluidUI(IFluidUIContext? fluidUIContext)
    {
        if (fluidUIContext == null)
        {
            return;
        }

        this.HideControl0 = fluidUIContext.HideControl0;
        this.HideControl1 = fluidUIContext.HideControl1;
        this.HideControl2 = fluidUIContext.HideControl2;
        this.HideControl3 = fluidUIContext.HideControl3;
        this.HideControl4 = fluidUIContext.HideControl4;
        this.HideControl5 = fluidUIContext.HideControl5;
        this.HideControl6 = fluidUIContext.HideControl6;
        this.HideControl7 = fluidUIContext.HideControl7;

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

    public void SetFluidUIByUser(IFluidUIContext? fluidUIContext)
    {
        if (fluidUIContext != null)
        {
            this.fluidUIBackUp = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.FluidUI);

            this.SetFluidUI(fluidUIContext);
        }
    }

    public void SetFluidUICopyByUser(IFluidUIContext fluidUIContext)
    {
        if (fluidUIContext != null)
        {
            var copy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(fluidUIContext);

            this.SetFluidUIByUser(copy);
        }
    }

    public void SetFluidUIMenuViewModel(FluidUIMenuViewModel? fluidUIMenuViewModel)
    {
        if (fluidUIMenuViewModel != null)
        {
            this.fluidUIMenuViewModel = fluidUIMenuViewModel;
        }

        this.UpdateFluidUI();
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

    internal void Drop(DragEventArgs e, Point? coords = null, BrushTargets? brushTarget = null)
    {
        var dropResult = new DragAndDropManager(this).ProcessDropObjects(e, coords, brushTarget);

        e.Handled = true;
    }

    internal virtual void ResetFluidUIToPrevious()
    {
        this.fluidUI = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.fluidUIBackUp);
        this.OnPropertyChanged(nameof(this.FluidUI));
    }

    internal virtual void Setup()
    {
    }

    internal virtual void TriggerRedraw()
    {
    }

    internal void ClearDataBlock()
    {
        this.OnPropertyChanged(nameof(this.FluidUI.DataBlock));
        this.FluidUIMenuViewModel?.CreateFluidUIDataBlockSetupViewModel();
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

    protected virtual void HideControl0Changed(bool value)
    {
    }

    protected virtual void HideControl1Changed(bool value)
    {
    }

    protected virtual void HideControl2Changed(bool value)
    {
    }

    partial void OnHideControl0Changed(bool value)
    {
        this.HideControl0Changed(value);
    }

    partial void OnHideControl1Changed(bool value)
    {
        this.HideControl1Changed(value);
    }

    partial void OnHideControl2Changed(bool value)
    {
        this.HideControl2Changed(value);
    }
}

// EOF