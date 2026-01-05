// <copyright file="ScreenControlViewModel.cs" company=".">
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
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Utilities;

public partial class ScreenControlViewModel : ObservableObject // so machen das die Settings für das Eboard auch die Oberfläche der SettingsView definieren.
{
    [ObservableProperty]
    private int arrangementOffsetValue;

    [ObservableProperty]
    private int arrangementRotationValue;

    private EBoardViewModel eBoardViewModel;

    private ArrangeSelectedElements arrangeSelectedElements;

    public ScreenControlViewModel(EBoardViewModel eBoardViewModel)
    {
        this.eBoardViewModel = eBoardViewModel;
        this.arrangeSelectedElements = new ArrangeSelectedElements();
    }

    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    // in anderes Modul auslagern, hier einstellen, anderswo das arrangement von selektionen setzen
    // da könnte ich hier alle Settings reinpacken, also auch aus dem eboardbrowser ggf raus kopieren
    [RelayCommand]
    private void ArrangeGroupAsLine()
    {
        this.arrangeSelectedElements.ArrangeGroupAsLine(this.EBoardViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsSquare()
    {
        this.arrangeSelectedElements.ArrangeGroupAsSquare(this.EBoardViewModel);
    }

    [RelayCommand]
    private void ArrangeGroupAsRandomMatrix10x10()
    {
        this.arrangeSelectedElements.ArrangeGroupAsRandomMatrix10x10(this.EBoardViewModel);
    }
}

// EOF