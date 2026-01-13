// <copyright file="FluidUIFontManager.cs" company=".">
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
namespace EBoardSDK.Models.FluidUIFont;

using EBoardSDK.Interfaces;
using EBoardSDK.ViewModels;
using System.Windows;
using System.Windows.Media;

internal class FluidUIFontManager : IFluidUIManager
{
    private EboardFluidUIBaseViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluidUIFontManager"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    internal FluidUIFontManager(EboardFluidUIBaseViewModel viewModel)
    {
        this.viewModel = viewModel;
    }

    public void Reset()
    {
        if (this.viewModel.FluidUI.Font != null)
        {
            this.viewModel.FluidUI.Font.SetInitialValues();

            this.viewModel.UpdateFont();
        }
    }

    internal FontFamily GetFontFamily()
    {
        return this.viewModel.FluidUI.Font?.FontFamily ?? new FontFamily("Verdana");
    }

    internal int GetFontSize()
    {
        return this.viewModel.FluidUI.Font?.FontSizeDisplay ?? 15;
    }

    internal FontWeight GetFontWeight()
    {
        return this.viewModel.FluidUI.Font?.FontWeight ?? FontWeights.Normal;
    }

    internal void ResetFont()
    {
        if (this.viewModel.FluidUI.Font != null)
        {
            this.viewModel.FluidUI.Font.FontSize = 15.0;
            this.viewModel.FluidUI.Font.FontFamily = new System.Windows.Media.FontFamily("Verdana");
            this.viewModel.FluidUI.Font.FontWeight = FontWeights.Normal;

            this.viewModel.UpdateFont();
        }
    }

    internal void SetFontFamily(FontFamily fontfamily)
    {
        if (this.viewModel.FluidUI.Font != null)
        {
            this.viewModel.FluidUI.Font.FontFamily = fontfamily;

            this.viewModel.UpdateFont();
        }
    }

    internal void SetFontSize(int fontsize)
    {
        if (this.viewModel.FluidUI.Font != null)
        {
            this.viewModel.FluidUI.Font.FontSize = fontsize;

            this.viewModel.UpdateFont();
        }
    }

    internal void SetFontWeight(FontWeight fontweight)
    {
        if (this.viewModel.FluidUI.Font != null)
        {
            this.viewModel.FluidUI.Font.FontWeight = fontweight;

            this.viewModel.UpdateFont();
        }
    }
}

// EOF