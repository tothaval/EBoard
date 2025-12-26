// <copyright file="MainWindow.xaml.cs" company=".">
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
namespace EBoardSDK;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
    }

    public MainWindow(MainViewModel mainViewModel)
    {
        this.InitializeComponent();

        this.DataContext = mainViewModel;
    }

    private void SizeAndPositionUpdate()
    {
        ((MainViewModel)this.DataContext).FluidUI.Stand.Position = new Point(this.Left, this.Top);

        ((MainViewModel)this.DataContext).FluidUI.Size.Width = this.ActualWidth;
        ((MainViewModel)this.DataContext).FluidUI.Size.Height = this.ActualHeight;
    }

    private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ((MainViewModel)this.DataContext).DeselectElements();

        if (e.LeftButton == MouseButtonState.Pressed)
        {
            this.DragMove();
        }
    }

    private void EboardMainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        this.SizeAndPositionUpdate();

        e.Handled = true;
    }

    private void EboardMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        this.SizeAndPositionUpdate();

        e.Handled = true;
    }

    private void MenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Normal)
        {
            this.WindowState = WindowState.Maximized;
            this.Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Normalize";
        }
        else
        {
            this.WindowState = WindowState.Normal;
            this.Background = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["MaximizeContextMenuItemHeader"] = "Maximize";
        }
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        this.WindowState = System.Windows.WindowState.Minimized;
    }
}

// EOF