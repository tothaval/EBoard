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
namespace EBoardSDK.Windows;

using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel? viewModel;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="mainViewModel">Desired is a single MainViewModel instance for eboard runtime,
    /// it will be set as DataContext for this MainWindow class.</param>
    public MainWindow(MainViewModel mainViewModel)
    {
        this.viewModel = mainViewModel;
        this.DataContext = mainViewModel;

        Application.Current.Resources["MenuItemHeaderMaximize"] = "Maximize";

        this.InitializeComponent();
    }

    internal void MaximizeMainWindow()
    {
        if (this.WindowState == WindowState.Normal)
        {
            if (this.viewModel != null)
            {
                var manager = FluidUIManagerFactory.GetSizeManager(this.viewModel);

                manager.Reset_FluidUISizeWidthAndHeight();
            }

            this.WindowState = WindowState.Maximized;
            Application.Current.Resources["MenuItemHeaderMaximize"] = "Normalize";

            return;
        }

        this.WindowState = WindowState.Normal;
        Application.Current.Resources["MenuItemHeaderMaximize"] = "Maximize";
    }

    internal void MinimizeMainWindow()
    {
        this.WindowState = System.Windows.WindowState.Minimized;
    }

    private void SizeAndPositionUpdate()
    {
        if (this.viewModel != null)
        {
            var sizeManager = FluidUIManagerFactory.GetSizeManager(this.viewModel);
            var standManager = FluidUIManagerFactory.GetStandManager(this.viewModel);

            if (this.viewModel.FluidUI.Size != null)
            {
                if (sizeManager.GetWidth() != double.NaN)
                {
                    sizeManager.SetWidth(this.ActualWidth);
                }

                if (sizeManager.GetHeight() != double.NaN)
                {
                    sizeManager.SetHeight(this.ActualHeight);
                }
            }

            if (this.viewModel.FluidUI.Stand != null)
            {
                standManager.SetPosition(new Point(this.Left, this.Top));
            }
        }
    }

    private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.viewModel?.DeselectElements();

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

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        this.MaximizeMainWindow();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e)
    {
        this.MinimizeMainWindow();
    }

    private void EboardMainWindow_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop) && this.viewModel != null)
        {
            this.viewModel.Drop(e, this.viewModel);
        }
    }
}

// EOF