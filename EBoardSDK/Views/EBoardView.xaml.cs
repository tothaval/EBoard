// <copyright file="EBoardView.xaml.cs" company=".">
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
namespace EBoardSDK.Views;

using EBoardSDK.ViewModels;
<<<<<<< Updated upstream
=======
using System.Windows;
>>>>>>> Stashed changes
using System.Windows.Controls;

/// <summary>
/// Interaktionslogik für EBoardView.xaml.
/// </summary>
public partial class EBoardView : UserControl
{
    private EBoardViewModel eboardViewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="EBoardView"/> class.
    /// </summary>
    public EBoardView()
    {
        this.InitializeComponent();

        this.DataContextChanged += this.EBoardView_DataContextChanged;
    }

    private void EBoardView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        this.SetPlacement();
    }

    public void SetPlacement()
    {
        if (this.DataContext != null)
        {
<<<<<<< Updated upstream
            this.eboardViewModel = (EBoardViewModel)this.DataContext;
=======
            var dc = this.DataContext as ScreenViewModel;

            if (dc != null)
            {
                this.eboardViewModel = (ScreenViewModel)this.DataContext;
            }
>>>>>>> Stashed changes
        }

        if (this.eboardViewModel != null)
        {
            this.eboardViewModel.SetView(this);
        }
    }
<<<<<<< Updated upstream
=======

    private void EBoardView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        this.SetViewToDataContext();
    }

    private void EBoard_Board_Drop(object sender, System.Windows.DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            if (this.eboardViewModel != null)
            {
                var coords = e.GetPosition(this);

                this.eboardViewModel.Drop(e, coords: coords);
            }
        }

        e.Handled = true;
    }
>>>>>>> Stashed changes
}

// EOF