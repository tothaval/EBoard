// <copyright file="StandardTextMenuItemViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Shapes.TextShape;
using System.Windows.Media;

public partial class StandardTextMenuItemViewModel : ObservableObject
{
    private StandardTextViewModel viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardTextMenuItemViewModel"/> class.
    /// </summary>
    /// <param name="viewModel"></param>
    public StandardTextMenuItemViewModel(StandardTextViewModel viewModel)
    {
        this.viewModel = viewModel;

        this.OnPropertyChanged(nameof(this.ViewModel));
    }

    public StandardTextViewModel ViewModel => this.viewModel;

    [RelayCommand]
    private void LoadStandardTextModelFromFile()
    {
        this.ViewModel.LoadStandardTextModelFromFile();
    }

    [RelayCommand]
    private void MakeTextShape()
    {
        if (this.ViewModel.ElementViewModel == null)
        {
            return;
        }

        var screen = this.ViewModel.ElementViewModel.ScreenViewModel;
        var manager = new FluidUIDeepCopyManager();
        var copy = manager.DeepCopyIFluidUIContext(this.ViewModel.ElementViewModel.FluidUI);

        // var elementViewModel = new ElementViewModel(screen);
        var textshape = new TextShapeViewModel($"{this.ViewModel.Title}\n{this.ViewModel.Text}");

        // textshape.SetElementViewModel(elementViewModel);
        textshape.SetFluidUIContext(copy);
        textshape.RefreshInitialization();

        // elementViewModel.Plugin = textshape;
        new SDKPluginManager().InvokePluginOnEboard(textshape, screen, new System.Windows.Point(100, 100));

        // new SDKPluginManager().InvokeElementViewModelOnEboard(elementViewModel);
    }

    [RelayCommand]
    private void SaveStandardTextModelToFile()
    {
        this.ViewModel.SaveStandardTextModelToFile();
    }
}

// EOF