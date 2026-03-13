// <copyright file="RectangleMenuItemViewModel.cs" company=".">
// Stephan Kammel
// </copyright>
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
namespace EBoardSDK.Plugins.Shapes.Rectangle;
using CommunityToolkit.Mvvm.ComponentModel;

public partial class RectangleMenuItemViewModel : ObservableObject
{
    private RectangleViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleMenuItemViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    public RectangleMenuItemViewModel(RectangleViewModel viewModel)
    {
        this.viewModel = viewModel;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public RectangleViewModel ViewModel => this.viewModel;
}

// EOF