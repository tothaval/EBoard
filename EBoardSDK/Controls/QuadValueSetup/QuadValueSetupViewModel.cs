// <copyright file="QuadValueSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.QuadValueSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Models;

public partial class QuadValueSetupViewModel : ObservableObject
{
    private readonly IFluidUIDesignModel brushManagement;

    [ObservableProperty]
    private QuadValue<int> quadValue;

    private int all = 0;
    private int topLeft = 0;
    private int topRight = 0;
    private int bottomLeft = 0;
    private int bottomRight = 0;

    public IFluidUIDesignModel BrushManagement => this.brushManagement;

    private Action okAction;

    public QuadValueSetupViewModel(QuadValue<int> thebeforethickness, Action okResult, IFluidUIDesignModel brushManagement)
    {
        this.brushManagement = brushManagement;

        this.okAction = okResult;

        this.quadValue = thebeforethickness;

        var c = thebeforethickness;

        if (c.Value1 == c.Value2 && c.Value1 == c.Value3 && c.Value1 == c.Value4)
        {
            this.all = c.Value1;
        }
        else
        {
            var cmid = ((int)c.Value1 + (int)c.Value2 + (int)c.Value3 + (int)c.Value4) / 4;

            this.all = cmid;
        }

        // calling the fields on purpose, to trigger property changed only once
        this.topLeft = (int)c.Value1;
        this.topRight = (int)c.Value2;
        this.bottomRight = (int)c.Value3;
        this.BottomLeft = (int)c.Value4;

        this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    public string QuadValueString => $"{this.QuadValue.Value1},{this.QuadValue.Value2},{this.QuadValue.Value3},{this.QuadValue.Value4}";

    public int All
    {
        get
        {
            return this.all;
        }

        set
        {
            if (this.all != value)
            {
                this.all = value;

                this.topLeft = value;
                this.topRight = value;
                this.bottomRight = value;
                this.bottomLeft = value;

                this.QuadValue = this.BuildThickness(this.All);

                this.OnPropertyChanged(nameof(this.QuadValueString));

                this.OnPropertyChanged(nameof(this.QuadValue));
                this.OnPropertyChanged(nameof(this.All));
                this.OnPropertyChanged(nameof(this.TopLeft));
                this.OnPropertyChanged(nameof(this.TopRight));
                this.OnPropertyChanged(nameof(this.BottomRight));
                this.OnPropertyChanged(nameof(this.BottomLeft));
            }
        }
    }


    public int TopLeft
    {
        get
        {
            return this.topLeft;
        }

        set
        {
            if (this.topLeft != value)
            {
                this.topLeft = value;

                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.TopLeft));
            }
        }
    }


    public int TopRight
    {
        get
        {
            return this.topRight;
        }

        set
        {
            if (this.topRight != value)
            {
                this.topRight = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.TopRight));
            }
        }
    }


    public int BottomLeft
    {
        get
        {
            return this.bottomLeft;
        }

        set
        {
            if (this.bottomLeft != value)
            {
                this.bottomLeft = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.BottomLeft));
            }
        }
    }

    public int BottomRight
    {
        get
        {
            return this.bottomRight;
        }

        set
        {
            if (this.bottomRight != value)
            {
                this.bottomRight = value;
                this.QuadValue = this.BuildThickness();

                this.OnPropertyChanged(nameof(this.QuadValueString));
                this.OnPropertyChanged(nameof(this.BottomRight));
            }
        }
    }

    public void Dispose()
    {
        this.BrushManagement.PropertyChangedEvent -= this.BrushManagement_PropertyChangedEvent;

        GC.SuppressFinalize(this);
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.BrushManagement));
    }

    [RelayCommand]
    private void Ok()
    {
        this.okAction?.Invoke();
    }

    private QuadValue<int> BuildThickness(int v) => new QuadValue<int>(v);

    private QuadValue<int> BuildThickness()
    {
        return new QuadValue<int>(this.TopLeft, this.TopRight, this.bottomRight, this.bottomLeft);
    }
}

// EOF