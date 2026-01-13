// <copyright file="LinearBrushSetupViewModel.cs" company=".">
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
namespace EBoardSDK.Controls.BrushSetup.LinearBrushSetup;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Models;
using System.Windows.Media;

public partial class LinearBrushSetupViewModel : ObservableObject, IDisposable
{
    public Brush Brush { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinearBrushSetupViewModel"/> class.
    /// </summary>
    public LinearBrushSetupViewModel()
    {
        var gradientStops = new GradientStopCollection();

        gradientStops.Add(new GradientStop(Colors.Red, 0.0));
        gradientStops.Add(new GradientStop(Colors.Blue, 1.0));

        var brush = new LinearGradientBrush();
        brush.GradientStops = gradientStops;
        brush.StartPoint = new System.Windows.Point(0, 0.5);
        brush.EndPoint = new System.Windows.Point(1, 0.5);

        this.Brush = new ColorDataModel(brush).GetBrush().Result;

        this.OnPropertyChanged(nameof(this.Brush));
    }

    public void Dispose()
    {
    }
}

// EOF