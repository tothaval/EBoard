// <copyright file="SharedMethod_UI.cs" company=".">
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
namespace EBoardSDK.SharedMethods;

using EBoardConfigManager.Models;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIDataBlock;
using EBoardSDK.Interfaces.FluidUIDesign;
using EBoardSDK.Interfaces.FluidUISize;
using EBoardSDK.Models;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class SharedMethod_UI
{
    public SolidColorBrush ImagePathErrorDefaultBrush => new SolidColorBrush(Colors.White);

    private string txtShutDownQuestion = "Do you want to power down your physical hardware?";

    private string txtShutDownTitle = "Shutdown machine?";

    public Brush ChangeBackgroundToImage(Brush brush, string imagePath)
    {
        if (imagePath == null || imagePath == string.Empty)
        {
            return brush;
        }

        try
        {
            brush = new ImageBrush(new BitmapImage(
                new Uri(imagePath, UriKind.Absolute)));
        }
        catch (Exception)
        {
            return this.ImagePathErrorDefaultBrush;
        }

        return brush;
    }

    public double ConvertNegativeSizeValuesToNaN(double value)
    {
        if (value <= 0)
        {
            return double.NaN;
        }

        return value;
    }

    public int ResetSizeDisplayValue(double value)
    {
        if (double.IsNaN(value))
        {
            return -1;
        }

        return (int)value;
    }

    public void CloseApplication()
    {
        Application.Current.Shutdown();
    }

    public QuadValueSetupViewModel BuildQuadValueSetup(IFluidUISizeModel borderManagement, IFluidUIDesignModel brushManagement, Action action, BorderTargets borderTargets)
    {
        var quadValue = new QuadValue<int>();

        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                quadValue.Value1 = (int)borderManagement.CornerRadius.TopLeft;
                quadValue.Value2 = (int)borderManagement.CornerRadius.TopRight;
                quadValue.Value3 = (int)borderManagement.CornerRadius.BottomRight;
                quadValue.Value4 = (int)borderManagement.CornerRadius.BottomLeft;
                break;
            case BorderTargets.Margin:
                quadValue.Value1 = (int)borderManagement.Margin.Left;
                quadValue.Value2 = (int)borderManagement.Margin.Top;
                quadValue.Value3 = (int)borderManagement.Margin.Right;
                quadValue.Value4 = (int)borderManagement.Margin.Bottom;
                break;
            case BorderTargets.Padding:
                quadValue.Value1 = (int)borderManagement.Padding.Left;
                quadValue.Value2 = (int)borderManagement.Padding.Top;
                quadValue.Value3 = (int)borderManagement.Padding.Right;
                quadValue.Value4 = (int)borderManagement.Padding.Bottom;
                break;
            case BorderTargets.Thickness:
                quadValue.Value1 = (int)borderManagement.BorderThickness.Left;
                quadValue.Value2 = (int)borderManagement.BorderThickness.Top;
                quadValue.Value3 = (int)borderManagement.BorderThickness.Right;
                quadValue.Value4 = (int)borderManagement.BorderThickness.Bottom;
                break;
            default:
                break;
        }

        var quadValueSetupVM = new QuadValueSetupViewModel(quadValue, action, brushManagement);
        return quadValueSetupVM;
    }

    public QuadValueSetupViewModel GetQuadValueSetupViewModel(
    IFluidUISizeModel borderManagement,
    IFluidUIDesignModel brushManagement,
    Action quadValueAction,
    BorderTargets borderTargets)
    {
        var quadVM = this.BuildQuadValueSetup(borderManagement, brushManagement, quadValueAction, borderTargets);

        return quadVM;
    }

    public void MaximizeApplication(Window mainWindow)
    {
        // MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        if (mainWindow.WindowState == WindowState.Normal)
        {
            mainWindow.WindowState = WindowState.Maximized;

            // mainWindow.Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            Application.Current.Resources["EboardMainWindowMaximizeContextMenuHeader"] = "Normalize";
        }
        else
        {
            mainWindow.WindowState = WindowState.Normal;

            // mainWindow.Background = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["EboardMainWindowMaximizeContextMenuHeader"] = "Maximize";
        }
    }

    public void SetupTitleAndText(IFluidUIDataBlockModel dataBlockModel, string title, string text)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            text = string.Empty;
        }

        if (string.IsNullOrWhiteSpace(dataBlockModel.Title) || dataBlockModel.Title.Equals("title"))
        {
            dataBlockModel.Title = title;
        }

        if (string.IsNullOrWhiteSpace(dataBlockModel.Text) || dataBlockModel.Text.Equals("text"))
        {
            dataBlockModel.Text = text;
        }
    }

    public string UserSelectImage(string imagePathProperty)
    {
        Microsoft.Win32.OpenFileDialog setPath = new Microsoft.Win32.OpenFileDialog();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");
        setPath.Filter = "files (*.*)|*.*";
        setPath.FilterIndex = 2;
        setPath.RestoreDirectory = true;

        if (setPath.ShowDialog() == true)
        {
            imagePathProperty = setPath.FileName;

            // viewModel.ImagePath = setPath.FileName;
        }

        return imagePathProperty;
    }

    public string SetSaveDirectory()
    {
        Microsoft.Win32.OpenFolderDialog setPath = new Microsoft.Win32.OpenFolderDialog();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");

        if (setPath.ShowDialog() == true)
        {
            var savePath = Path.Combine(setPath.FolderName, DataLocations.EBoardDataRootPath);

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);

                Directory.CreateDirectory(Path.Combine(savePath, DataLocations.EBoardInstalledPluginsPath));

                Directory.CreateDirectory(Path.Combine(savePath, DataLocations.EBoardScreenDataPath));
            }

            return setPath.FolderName;
        }

        return string.Empty;
    }

    public void ShutDownMachine()
    {
        MessageBoxResult result = MessageBox.Show(this.txtShutDownQuestion, this.txtShutDownTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result == MessageBoxResult.Yes)
        {
            string command = "/C shutdown /p";
            Process.Start("cmd.exe", command);
        }
    }
}

// EOF