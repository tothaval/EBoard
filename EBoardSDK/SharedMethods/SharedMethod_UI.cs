// <copyright file="SharedMethod_UI.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.SharedMethods;

using EBoardConfigManager.Models;
using EBoardSDK.Controls;
using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Enums;
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

    public SolidColorBrushSetupViewModel BuildSolidColorBrushSetup(BrushManagement brushManagement, BrushTargets brushTargets, Action okResult)
    {
        return new SolidColorBrushSetupViewModel(brushManagement, brushTargets, okResult);
    }

    public QuadValueSetupViewModel BuildQuadValueSetup(QuadValue<int> quadValue, Action action, BrushManagement brushManagement)
    {
        return new QuadValueSetupViewModel(quadValue, action, brushManagement);
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

    public string SetBackgroundImage(string imagePathProperty)
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
