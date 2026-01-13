// <copyright file="EboardFluidUIBaseViewModel.cs" company=".">
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
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUISize;

public partial class EboardFluidUIBaseViewModel : ObservableObject, IFluidUI
{
    public event Action? PropertyChangedEvent;

    private IFluidUIContext fluidUI;

    protected FluidUIMenuViewModel? fluidUIMenuViewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.FluidUI))]
    protected int height = -1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.FluidUI))]
    protected int width = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="EboardFluidUIBaseViewModel"/> class.
    /// </summary>
    public EboardFluidUIBaseViewModel()
    {
        if (this.fluidUI == null)
        {
            this.fluidUI = new FluidUIContext();
            this.OnPropertyChanged(nameof(this.FluidUI));
        }
    }

    public IFluidUIContext FluidUI => this.fluidUI;

    public FluidUIMenuViewModel FluidUIMenuViewModel => this.fluidUIMenuViewModel;

    public virtual void Dispose()
    {
        this.FluidUI.Dispose();
        this.FluidUIMenuViewModel?.Dispose();
    }

    public void SetInitialValues()
    {
        this.FluidUI.SetInitialValues();
    }

    internal virtual void BecomesActive()
    {
    }

    internal virtual void BecomesInactive()
    {
        this.FluidUI.Dispose();
        this.FluidUIMenuViewModel?.Dispose();
    }

    internal virtual void CreateFluidUIMenuViewModel()
    {
    }

    internal void DeleteFluidUIMenuViewModel()
    {
        this.FluidUIMenuViewModel?.Dispose();
    }

    internal void SetFluidUI(IFluidUIContext? fluidUIContext)
    {
        if (fluidUIContext == null)
        {
            this.fluidUI = new FluidUIContext();
            this.FluidUI.SetInitialValues();
        }
        else
        {
            if (fluidUIContext.DataBlock != null)
            {
                this.fluidUI.DataBlock = fluidUIContext.DataBlock;
                this.UpdateDataBlock();
            }

            if (fluidUIContext.Design != null)
            {
                this.fluidUI.Design = fluidUIContext.Design;
                this.UpdateDesign();
            }

            if (fluidUIContext.Font != null)
            {
                this.fluidUI.Font = fluidUIContext.Font;
                this.UpdateFont();
            }

            if (fluidUIContext.Size != null)
            {
                this.fluidUI.Size = fluidUIContext.Size;
                this.UpdateSize();
            }

            if (fluidUIContext.Stand != null)
            {
                this.fluidUI.Stand = fluidUIContext.Stand;
                this.UpdateStand();
            }
        }

        this.UpdateFluidUI();
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

    internal virtual void UpdateDataBlock()
    {
        this.OnPropertyChanged(nameof(this.FluidUI.DataBlock));

        this.UpdateFluidUI();
    }

    internal virtual void UpdateDesign()
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Design));

        this.UpdateFluidUI();
    }

    internal virtual void UpdateFont()
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Font));

        this.UpdateFluidUI();
    }

    internal virtual void UpdateSize()
    {
        this.SetSize();

        this.OnPropertyChanged(nameof(this.FluidUI.Size));

        this.UpdateFluidUI();
    }

    internal virtual void UpdateStand()
    {
        this.OnPropertyChanged(nameof(this.FluidUI.Stand));

        this.FluidUIMenuViewModel?.FluidUIStandSetupViewModel?.ApplyFluidUIStandValues();

        this.UpdateFluidUI();

        this.TriggerRedraw();
    }

    internal virtual void UpdateFluidUI()
    {
        this.OnPropertyChanged(nameof(this.FluidUI));
        this.OnPropertyChanged(nameof(this.FluidUIMenuViewModel));
    }

    protected void SetSize()
    {
        var manager = new FluidUISizeManager(this);

        var height = manager.GetHeight();
        var width = manager.GetWidth();

        if (this.Height != height)
        {
            this.Height = height;
        }

        if (this.Width != width)
        {
            this.Width = width;
        }

        this.OnPropertyChanged(nameof(this.FluidUI.Size));

        this.UpdateFluidUI();
    }

    partial void OnHeightChanged(int value)
    {
        var manager = new FluidUISizeManager(this);

        manager.SetHeight(value);
    }

    partial void OnWidthChanged(int value)
    {
        var manager = new FluidUISizeManager(this);

        manager.SetWidth(value);
    }
}

// EOF