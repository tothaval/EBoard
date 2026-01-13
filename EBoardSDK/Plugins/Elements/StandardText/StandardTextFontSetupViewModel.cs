// <copyright file="StandardTextFontSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

public partial class StandardTextFontSetupViewModel : ObservableObject
{
    private EboardFluidUIBaseViewModel viewModel;
    private StandardTextViewModel standardTextViewModel;

    private bool isTitle = true;

    [ObservableProperty]
    private FontFamily fontFamily;

    [ObservableProperty]
    private int fontSizeValue;

    [ObservableProperty]
    private FontWeight fontWeight;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardTextFontSetupViewModel"/> class.
    /// </summary>
    /// <param name="eboardFluidUIBaseViewModel"></param>
    /// <param name="standardTextViewModel"></param>
    /// <param name="isTitle"></param>
    public StandardTextFontSetupViewModel(EboardFluidUIBaseViewModel eboardFluidUIBaseViewModel, StandardTextViewModel standardTextViewModel, bool isTitle)
    {
        this.viewModel = eboardFluidUIBaseViewModel;
        this.standardTextViewModel = standardTextViewModel;
        this.isTitle = isTitle;

        if (isTitle)
        {
            this.FontFamily = this.standardTextViewModel.TitleFont.FontFamily;
            this.FontSizeValue = (int)this.standardTextViewModel.TitleFont.FontSize;
            this.FontWeight = this.standardTextViewModel.TitleFont.FontWeight;
        }
        else
        {
            this.FontFamily = this.standardTextViewModel.TextFont.FontFamily;
            this.FontSizeValue = (int)this.standardTextViewModel.TextFont.FontSize;
            this.FontWeight = this.standardTextViewModel.TextFont.FontWeight;
        }

        this.OnPropertyChanged(nameof(this.ViewModel));
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
        if (this.isTitle)
        {
            this.standardTextViewModel.TitleFont.FontSize = fontSize;
        }
        else
        {
            this.standardTextViewModel.TextFont.FontSize = fontSize;
        }

        this.standardTextViewModel.UpdateFont();
    }

    public void Dispose()
    {
    }

    public void ResetFont()
    {
        this.FontFamily = new FontFamily("Verdana");
        this.FontSizeValue = 15;
        this.FontWeight = FontWeights.Normal;

        if (this.isTitle)
        {
            this.standardTextViewModel.TitleFont.FontFamily = this.FontFamily;
            this.standardTextViewModel.TitleFont.FontSize = this.FontSizeValue;
            this.standardTextViewModel.TitleFont.FontWeight = this.FontWeight;
        }
        else
        {
            this.standardTextViewModel.TextFont.FontFamily = this.FontFamily;
            this.standardTextViewModel.TextFont.FontSize = this.FontSizeValue;
            this.standardTextViewModel.TextFont.FontWeight = this.FontWeight;
        }

        this.standardTextViewModel.UpdateFont();

        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamilyName));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontFamily));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontSize));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font.FontSizeDisplay));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI.Font));
        this.OnPropertyChanged(nameof(this.ViewModel.FluidUI));
        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public void SetInitialValues()
    {
    }

    partial void OnFontFamilyChanged(FontFamily value)
    {
        if (this.isTitle)
        {
            this.standardTextViewModel.TitleFont.FontFamily = this.FontFamily;
        }
        else
        {
            this.standardTextViewModel.TextFont.FontFamily = this.FontFamily;
        }

        this.standardTextViewModel.UpdateFont();
    }

    partial void OnFontWeightChanged(FontWeight value)
    {
        if (this.isTitle)
        {
            this.standardTextViewModel.TitleFont.FontWeight = this.FontWeight;
        }
        else
        {
            this.standardTextViewModel.TextFont.FontWeight = this.FontWeight;
        }

        this.standardTextViewModel.UpdateFont();
    }

    [RelayCommand]
    private void ApplyFontSize()
    {
        this.ApplyFontSize(this.FontSizeValue);
    }

    [RelayCommand]
    protected void Reset()
    {
        this.ResetFont();
    }
}

// EOF