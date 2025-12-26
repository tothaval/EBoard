// <copyright file="BasicAVMainView.xaml.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.BasicAV;

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/// <summary>
/// Interaktionslogik für SoundMixMainView.xaml
/// </summary>
public partial class BasicAVMainView : UserControl
{
    public BasicAVMainView()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        e.Handled = true;
    }

    private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.StatusBarDragStart();
        }
    }

    private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.StatusBarDragCompleted();
        }
    }
}

// EOF