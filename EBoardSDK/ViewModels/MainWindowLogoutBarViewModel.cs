// <copyright file="MainWindowLogoutBarViewModel.cs" company=".">
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
/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  MainWindowLogoutBarViewModel
 *
 *  view model for MainWindowLogoutBarView, which has two buttons atm to close the application
 */
namespace EBoardSDK.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.SharedMethods;

public partial class MainWindowLogoutBarViewModel : ObservableObject
{
    private readonly IFluidUIDesignModel brushManagement;
    private readonly MainViewModel mainViewModel;

    [ObservableProperty]
    private string txtExitEboard = "Off";

    [ObservableProperty]
    private string txtShutDownMachine = "Shutdown";

    public IFluidUIDesignModel BrushManagement => this.brushManagement;

    public MainViewModel MainViewModel => this.mainViewModel;

    public MainWindowLogoutBarViewModel(IFluidUIDesignModel brushManagement, MainViewModel mainViewModel)
    {
        this.brushManagement = brushManagement;
        this.mainViewModel = mainViewModel;

        //brushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
    }

    //private void BrushManagement_PropertyChangedEvent()
    //{
    //    this.OnPropertyChanged(nameof(this.BrushManagement));
    //}

    [RelayCommand]
    private void Close()
    {
        new SharedMethod_UI().CloseApplication();
    }

    [RelayCommand]
    private void ShutDown()
    {
        var runner = this.mainViewModel.GetRunnerInstance();

        var saveResult = runner.SaveEboardDataAsync().Result;

        new SharedMethod_UI().ShutDownMachine();
    }
}

// EOF