// <copyright file="FluidUIFontSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.FluidUIFont;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Interfaces.FluidUIText;
using EBoardSDK.ViewModels;
using System;
using System.Windows;

using FontFamily = System.Windows.Media.FontFamily;

public partial class FluidUIFontSetupViewModel : ObservableObject, IFluidUIFontSetup
{
    private EboardFluidUIBaseViewModel viewModel;

    [ObservableProperty]
    private FontFamily fontFamily;

    public FluidUIFontSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.FontFamily = new FontFamily(this.ViewModel.FluidUI.Font.FontFamilyName);

        this.ViewModel.FluidUI.Font.PropertyChangedEvent += this.Font_PropertyChangedEvent;
    }

    public event Action? PropertyChangedEvent;

    public List<FontWeight> FontWeightList { get; } =
[
        FontWeights.Black,
        FontWeights.Bold,
        FontWeights.DemiBold,
        FontWeights.ExtraBlack,
        FontWeights.ExtraBold,
        FontWeights.ExtraLight,
        FontWeights.Heavy,
        FontWeights.Light,
        FontWeights.Medium,
        FontWeights.Normal,
        FontWeights.Regular,
        FontWeights.SemiBold,
        FontWeights.Thin,
        FontWeights.UltraBlack,
        FontWeights.UltraBold,
        FontWeights.UltraLight,
    ];

    public EboardFluidUIBaseViewModel ViewModel => this.viewModel;

    public void ApplyFontSize(int fontSize)
    {
        this.ViewModel.FluidUI.Font.FontSize = fontSize;

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
    }

    public void Dispose()
    {
    }

    public void SetInitialValues()
    {
    }

    partial void OnFontFamilyChanged(FontFamily value)
    {
        this.ViewModel.FluidUI.Font.FontFamily = value;

        this.PropertyChangedEvent?.Invoke();

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamilyName));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamily));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.ViewModel));
        this.OnPropertyChanged(nameof(this.FontFamily));
    }

    private void Font_PropertyChangedEvent()
    {
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontWeight));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontSize));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontSizeDisplay));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamilyName));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamily));

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.FontFamily));
    }
}

// EOF