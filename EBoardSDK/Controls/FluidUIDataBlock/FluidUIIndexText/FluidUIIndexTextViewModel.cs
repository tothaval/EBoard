// <copyright file="FluidUIIndexTextViewModel.cs" company=".">
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
using EBoardSDK.Models.FluidUIDataBlock;
using EBoardSDK.ViewModels;
using System;

public partial class FluidUIIndexTextViewModel : ObservableObject
{
    private readonly FluidUIBaseViewModel viewModel;
    private readonly FluidUIIndexText indexText;

    [ObservableProperty]
    private int index;

    [ObservableProperty]
    private DateTime time;

    [ObservableProperty]
    private string text;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIIndexTextViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="fluidUIIndexText"></param>
    public FluidUIIndexTextViewModel(FluidUIBaseViewModel viewModel, FluidUIIndexText fluidUIIndexText)
    {
        this.viewModel = viewModel;
        this.indexText = fluidUIIndexText;

        this.Index = this.indexText.Index;
        this.Time = this.indexText.Time;
        this.Text = this.indexText.Text;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIIndexTextViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="fluidUIIndexText"></param>
    /// <param name="index"></param>
    public FluidUIIndexTextViewModel(FluidUIBaseViewModel viewModel, FluidUIIndexText fluidUIIndexText, int index)
    {
        this.viewModel = viewModel;
        this.indexText = fluidUIIndexText;

        this.Index = index;
        this.Time = this.indexText.Time;
        this.Text = this.indexText.Text;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public FluidUIBaseViewModel ViewModel => this.viewModel;

    partial void OnIndexChanged(int value)
    {
        this.indexText.Index = value;

        //var manager = new FluidUIDataBlockManager(this.ViewModel);
        //manager.set
    }

    partial void OnTextChanged(string value)
    {
        this.indexText.Text = value;
    }

    partial void OnTimeChanged(DateTime value)
    {
        this.indexText.Time = value;
    }
}

// EOF