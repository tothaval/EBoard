// <copyright file="SharedMethod_UI.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.SharedMethods;

using EBoardSDK.Controls.QuadValueSetup;
using EBoardSDK.Models;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class SharedMethod_UI
{
    public SolidColorBrush ImagePathErrorDefaultBrush => new SolidColorBrush(Colors.White);

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

    public QuadValueSetupViewModel BuildQuadValueSetup(QuadValue<int> quadValue, Action action)
    {
        return new QuadValueSetupViewModel(quadValue, action);
    }

    public void MaximizeApplication(Window mainWindow)
    {
        //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        if (mainWindow.WindowState == WindowState.Normal)
        {
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.Background = (SolidColorBrush)Application.Current.Resources["BackgroundBrush"];
            Application.Current.Resources["EboardMainWindowMaximizeContextMenuHeader"] = "Normalize";
        }
        else
        {
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Background = new SolidColorBrush(Colors.Transparent);
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
            //viewModel.ImagePath = setPath.FileName;
        }

        return imagePathProperty;
    }
}
