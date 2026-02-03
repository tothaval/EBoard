// <copyright file="QuadFluidUIIndexTextViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIDataBlock.FluidUIIndexText;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Enums;
using EBoardSDK.ViewModels;

public partial class QuadFluidUIIndexTextViewModel : ObservableObject
{
    [ObservableProperty]
    private FluidUIIndexTextViewModel value1;

    [ObservableProperty]
    private FluidUIIndexTextViewModel value2;

    [ObservableProperty]
    private FluidUIIndexTextViewModel value3;

    [ObservableProperty]
    private FluidUIIndexTextViewModel value4;

    private FluidUIBaseViewModel viewModel;
    private QuadValueTargets quadValueTargets;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadFluidUIIndexTextViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="quadValueTargets"></param>
    public QuadFluidUIIndexTextViewModel(FluidUIBaseViewModel viewModel, QuadValueTargets quadValueTargets)
    {
        this.viewModel = viewModel;
        this.quadValueTargets = quadValueTargets;

        if (this.viewModel.FluidUI.DataBlock != null)
        {
            switch (this.quadValueTargets)
            {
                case QuadValueTargets.A:
                    this.Value1 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue1.Value1, 1);
                    this.Value2 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue1.Value2, 2);
                    this.Value3 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue1.Value3, 3);
                    this.Value4 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue1.Value4, 4);
                    break;
                case QuadValueTargets.B:
                    this.Value1 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue2.Value1, 1);
                    this.Value2 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue2.Value2, 2);
                    this.Value3 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue2.Value3, 3);
                    this.Value4 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue2.Value4, 4);
                    break;
                case QuadValueTargets.C:
                    this.Value1 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue3.Value1, 1);
                    this.Value2 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue3.Value2, 2);
                    this.Value3 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue3.Value3, 3);
                    this.Value4 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue3.Value4, 4);
                    break;
                case QuadValueTargets.D:
                    this.Value1 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue4.Value1, 1);
                    this.Value2 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue4.Value2, 2);
                    this.Value3 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue4.Value3, 3);
                    this.Value4 = new FluidUIIndexTextViewModel(this.viewModel, this.viewModel.FluidUI.DataBlock.IndexTextQuadValue4.Value4, 4);
                    break;
                default:
                    break;
            }
        }

        this.OnPropertyChanged(nameof(this.QuadValueTargets));
    }

    public QuadValueTargets QuadValueTargets => this.quadValueTargets;

    public void Reset()
    {
        this.Value1.Text = string.Empty;
        this.Value2.Text = string.Empty;
        this.Value3.Text = string.Empty;
        this.Value4.Text = string.Empty;
    }
}

// EOF