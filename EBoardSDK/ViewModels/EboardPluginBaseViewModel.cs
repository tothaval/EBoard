// <copyright file="EboardPluginBaseViewModel.cs" company=".">
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

using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;

/// <summary>
/// This class is for eboard category plugins to extend
/// them beyond PluginBaseViewModel where necessary.
/// </summary>
public abstract class EboardPluginBaseViewModel : PluginBaseViewModel
{
    protected bool instantiatedAsElement = false;

    protected MainViewModel mainViewModel;

    protected EboardPluginBaseViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.OnlyFluidUI);

        this.instantiatedAsElement = true;
        this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
    }

    /// <summary>
    /// Gets whether the deriving class is used as Element CA or not.
    /// </summary>
    public abstract bool InstantiatedAsElement { get; }

    public MainViewModel MainViewModel => this.mainViewModel;

    /// <summary>
    /// This method sets the <see cref="MainViewModel"/> property with the parameter.
    /// It is used on Eboard category plugins, because they need to access and
    /// interact with the MainWindow CA.
    /// </summary>
    /// <param name="mainViewModel">Desired is the single instance.</param>
    internal void SetMainViewModel(MainViewModel mainViewModel)
    {
        this.mainViewModel = mainViewModel;

        this.Setup();

        this.OnPropertyChanged(nameof(this.InstantiatedAsElement));
        this.OnPropertyChanged(nameof(this.MainViewModel));
    }

    /// <summary>
    /// This method is called by <see cref="EboardPluginBaseViewModel.SetMainViewModel(MainViewModel)"/>
    /// or by <see cref="PluginBaseViewModel.RefreshInitialization()"/> and can be used
    /// to finalize the plugins initialization.
    /// </summary>
    protected abstract void Setup();
}

// EOF