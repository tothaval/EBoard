// <copyright file="EboardConfig.cs" company=".">
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
namespace EBoardSDK.Models;
using EBoardSDK.Interfaces;
using EBoardSDK.Utilities;
using EBoardSDK.ViewModels;

/// <summary>
/// This class is the model for MainWindow context area. It stores the
/// FluidUIContext information for the Navigation context area.
/// </summary>
public class EboardConfig
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EboardConfig"/> class.
    /// </summary>
    public EboardConfig()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EboardConfig"/> class.
    /// </summary>
    /// <param name="mainViewModel"></param>
    public EboardConfig(MainViewModel mainViewModel)
    {
        this.EBoardIndex = mainViewModel.NavigationContextViewModel.EboardBrowserViewModel.CurrentSelectionID;
        this.EBoardCount = mainViewModel.NavigationContextViewModel.EboardCount;
        this.EBoardBrowserSwitch = mainViewModel.MenuBar.EBoardBrowserSwitch;
        this.ScreenControlSwitch = mainViewModel.MenuBar.ScreenControlSwitch;

        this.EBoardContext = (FluidUIContext)mainViewModel.FluidUI;

        this.FluidUIContextCopy = (FluidUIContext)mainViewModel.FluidUIContextCopy;

        this.EBoardBrowserViewContext = (FluidUIContext)mainViewModel.NavigationContextViewModel.FluidUI;

        this.ElementCopyList = mainViewModel.ElementCopyList.ToList();
    }

    public int EBoardCount { get; set; } = 1;

    public int EBoardIndex { get; set; } = 0;

    public bool EBoardBrowserSwitch { get; set; } = true;

    public bool ScreenControlSwitch { get; set; } = true;

    public FluidUIContext EBoardContext { get; set; } = new();

    public FluidUIContext FluidUIContextCopy { get; set; } = (FluidUIContext)SDKDataManager.DefaultFluidUIContext;

    public FluidUIContext EBoardBrowserViewContext { get; set; } = new();

    public List<PluginCopy> ElementCopyList { get; set; } = new();
}

// EOF