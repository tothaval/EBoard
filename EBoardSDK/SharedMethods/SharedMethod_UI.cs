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
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUISize;
using EBoardSDK.Plugins.Elements.BasicAV;
using EBoardSDK.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

public class SharedMethod_UI
{
    public SolidColorBrush ImagePathErrorDefaultBrush => new SolidColorBrush(Colors.White);

    private string txtShutDownQuestion = "Do you want to power down your physical hardware?";

    private string txtShutDownTitle = "Shutdown machine?";

<<<<<<< Updated upstream
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
=======
    public string FluidUIConfigurationFileExtension => "fcf"; // fcf > fluid ui configuration file
>>>>>>> Stashed changes

    public string StandardTextFileExtension => "stf"; // stf > standard text file

    public QuadValueSetupViewModel BuildQuadValueSetup(EboardFluidUIBaseViewModel viewModel, Action action, BorderTargets borderTargets)
    {
        var manager = new FluidUISizeManager(viewModel);
        var quadValue = new QuadValue<int>();

        switch (borderTargets)
        {
            case BorderTargets.CornerRadius:
                var corner = manager.GetCornerRadius();

                quadValue.Value1 = (int)corner.TopLeft;
                quadValue.Value2 = (int)corner.TopRight;
                quadValue.Value3 = (int)corner.BottomRight;
                quadValue.Value4 = (int)corner.BottomLeft;
                break;
            case BorderTargets.Margin:
                var margin = manager.GetMargin();

                quadValue.Value1 = (int)margin.Left;
                quadValue.Value2 = (int)margin.Top;
                quadValue.Value3 = (int)margin.Right;
                quadValue.Value4 = (int)margin.Bottom;
                break;
            case BorderTargets.Padding:
                var padding = manager.GetPadding();

                quadValue.Value1 = (int)padding.Left;
                quadValue.Value2 = (int)padding.Top;
                quadValue.Value3 = (int)padding.Right;
                quadValue.Value4 = (int)padding.Bottom;
                break;
            case BorderTargets.Thickness:
                var borderThickness = manager.GetBorderThickness();

                quadValue.Value1 = (int)borderThickness.Left;
                quadValue.Value2 = (int)borderThickness.Top;
                quadValue.Value3 = (int)borderThickness.Right;
                quadValue.Value4 = (int)borderThickness.Bottom;
                break;
            default:
                break;
        }

        var quadValueSetupVM = new QuadValueSetupViewModel(viewModel, quadValue, action);
        return quadValueSetupVM;
    }

<<<<<<< Updated upstream
    public QuadValueSetupViewModel GetQuadValueSetupViewModel(
    EboardFluidUIBaseViewModel viewModel,
    Action quadValueAction,
    BorderTargets borderTargets)
=======
    public void CloseApplication()
>>>>>>> Stashed changes
    {
        Application.Current.Shutdown();
    }

    public double ConvertNegativeSizeValuesToNaN(double value)
    {
        if (value <= 0)
        {
            return double.NaN;
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="folder"></param>
    /// <param name="filter"></param>
    /// <param name="initialDirectoryPath"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<string> GetFileToLoad(string folder = "Eboard/fluidui", string filter = "files (*.*)|*.*", string? initialDirectoryPath = null)
    {
        if (string.IsNullOrWhiteSpace(initialDirectoryPath))
        {
            initialDirectoryPath = null;
        }

        var path = initialDirectoryPath ?? Environment.CurrentDirectory;

        Microsoft.Win32.OpenFileDialog setPath = new Microsoft.Win32.OpenFileDialog();
        FluidUIContext fluidUIContext;

        setPath.InitialDirectory = path;

        setPath.Filter = filter;
        setPath.FilterIndex = 2;
        setPath.RestoreDirectory = true;

        if (setPath.ShowDialog() == true)
        {
            return setPath.FileName;
        }

        return string.Empty;
    }

    public QuadValueSetupViewModel GetQuadValueSetupViewModel(FluidUIBaseViewModel viewModel, Action quadValueAction, BorderTargets borderTargets)
    {
        var quadVM = this.BuildQuadValueSetup(viewModel, quadValueAction, borderTargets);

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

     /// <summary>
    ///
    /// </summary>
    /// <param name="fileExtension"></param>
    /// <param name="initialDirectoryPath"></param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<string> SetSaveFileName(string fileExtension, string? initialDirectoryPath = null)
    {
        if (string.IsNullOrWhiteSpace(initialDirectoryPath))
        {
            initialDirectoryPath = null;
        }

        var path = initialDirectoryPath ?? Environment.CurrentDirectory;

        Microsoft.Win32.SaveFileDialog setPath = new Microsoft.Win32.SaveFileDialog();
        setPath.InitialDirectory = path;
        setPath.AddExtension = true;
        setPath.DefaultExt = fileExtension;

        if (setPath.ShowDialog() == true)
        {
            return setPath.FileName;
        }

        return string.Empty;
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

    public int TransformDoubleNaNToInt(double value)
    {
        if (double.IsNaN(value))
        {
            return -1;
        }

        return (int)value;
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
}

// EOF