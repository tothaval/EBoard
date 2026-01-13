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
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.ViewModels;
using System.Windows;

public partial class QuadValueSetupViewModel : ObservableObject
{
    private readonly EboardFluidUIBaseViewModel viewModel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QuadValueString))]
    private int all = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QuadValueString))]
    private int topLeft = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QuadValueString))]
    private int topRight = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QuadValueString))]
    private int bottomLeft = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(QuadValueString))]
    private int bottomRight = 0;

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    private Action okAction;

    /// <summary>
    /// Initializes a new instance of the <see cref="QuadValueSetupViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="quadValue"></param>
    /// <param name="okResult"></param>
    public QuadValueSetupViewModel(EboardFluidUIBaseViewModel viewModel, QuadValue<int> quadValue, Action okResult)
    {
        this.viewModel = viewModel;

        this.okAction = okResult;

        this.ApplyQuadValue(quadValue);
    }

    public string QuadValueString => $"{this.TopLeft},{this.TopRight},{this.BottomRight},{this.BottomLeft}";

    public void ApplyQuadValue(QuadValue<int> quadValue)
    {
        this.TopLeft = quadValue.Value1;
        this.TopRight = quadValue.Value2;
        this.BottomRight = quadValue.Value3;
        this.BottomLeft = quadValue.Value4;

        this.SetAllValue();
    }

    public object GetQuadValueObject(BorderTargets borderTargets)
    {
        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                return new CornerRadius(
                    this.TopLeft,
                    this.TopRight,
                    this.BottomRight,
                    this.BottomLeft);
            case BorderTargets.Margin:
            case BorderTargets.Padding:
            case BorderTargets.Thickness:
                return new Thickness(
                    this.TopLeft,
                    this.TopRight,
                    this.BottomRight,
                    this.BottomLeft);
            default:
                return new QuadValue<int>(this.All);
        }
    }

    public void Dispose()
    {
    }

    public void Reset()
    {
        this.All = 0;
    }

    private void SetAllValue()
    {
        if (this.TopLeft == this.TopRight && this.TopLeft == this.BottomRight && this.TopLeft == this.BottomLeft)
        {
            this.All = this.TopLeft;
        }
        else
        {
            var cmid = (this.TopLeft + this.TopRight + this.BottomRight + this.BottomLeft) / 4;

            this.All = cmid;
        }

        this.OnPropertyChanged(nameof(this.All));
    }

    partial void OnAllChanged(int value)
    {
        this.topLeft = value;
        this.topRight = value;
        this.bottomRight = value;
        this.bottomLeft = value;

        this.OnPropertyChanged(nameof(this.TopLeft));
        this.OnPropertyChanged(nameof(this.TopRight));
        this.OnPropertyChanged(nameof(this.BottomRight));
        this.OnPropertyChanged(nameof(this.BottomLeft));
    }

    [RelayCommand]
    private void Ok()
    {
        this.okAction?.Invoke();
    }
}

// EOF