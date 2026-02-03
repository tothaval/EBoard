// <copyright file="QuadFluidUIKeyTextViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIDataBlock.FluidUIKeyText;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.ViewModels;

public partial class QuadFluidUIKeyTextViewModel : ObservableObject
{
    [ObservableProperty]
    private FluidUIKeyTextViewModel value1;

    [ObservableProperty]
    private FluidUIKeyTextViewModel value2;

    [ObservableProperty]
    private FluidUIKeyTextViewModel value3;

    [ObservableProperty]
    private FluidUIKeyTextViewModel value4;

    private FluidUIBaseViewModel viewModel;
    private QuadValueTargets quadValueTargets;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadFluidUIKeyTextViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="quadValueTargets"></param>
    public QuadFluidUIKeyTextViewModel(FluidUIBaseViewModel viewModel, QuadValueTargets quadValueTargets)
    {
        this.viewModel = viewModel;
        this.quadValueTargets = quadValueTargets;

        if (this.viewModel.FluidUI.DataBlock != null)
        {
            switch (this.quadValueTargets)
            {
                case QuadValueTargets.A:
                    this.Value1 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueA.Value1);
                    this.Value2 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueA.Value2);
                    this.Value3 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueA.Value3);
                    this.Value4 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueA.Value4);
                    break;
                case QuadValueTargets.B:
                    this.Value1 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueB.Value1);
                    this.Value2 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueB.Value2);
                    this.Value3 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueB.Value3);
                    this.Value4 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueB.Value4);
                    break;
                case QuadValueTargets.C:
                    this.Value1 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueC.Value1);
                    this.Value2 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueC.Value2);
                    this.Value3 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueC.Value3);
                    this.Value4 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueC.Value4);
                    break;
                case QuadValueTargets.D:
                    this.Value1 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueD.Value1);
                    this.Value2 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueD.Value2);
                    this.Value3 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueD.Value3);
                    this.Value4 = new FluidUIKeyTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.KeyTextQuadValueD.Value4);
                    break;
                default:
                    break;
            }
        }

        this.OnPropertyChanged(nameof(this.QuadValueTargets));
    }

    public QuadValueTargets QuadValueTargets => this.quadValueTargets;

    internal void Reset()
    {
        this.Value1.Key = string.Empty;
        this.Value2.Key = string.Empty;
        this.Value3.Key = string.Empty;
        this.Value4.Key = string.Empty;

        this.Value1.Text = string.Empty;
        this.Value2.Text = string.Empty;
        this.Value3.Text = string.Empty;
        this.Value4.Text = string.Empty;
    }
}

// EOF