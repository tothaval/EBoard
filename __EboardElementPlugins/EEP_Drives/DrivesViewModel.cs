// <copyright file="DrivesViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
namespace EEP_Drives;

<<<<<<< Updated upstream
using CommunityToolkit.Mvvm.Input;
=======
using CommunityToolkit.Mvvm.ComponentModel;
>>>>>>> Stashed changes
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
<<<<<<< Updated upstream
using Serilog;
using System.Diagnostics;
=======
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class DrivesViewModel : EBoardElementPluginBaseViewModel
{
    private DriveInfo[] allDrives = DriveInfo.GetDrives();

    private string pluginHeader = "Drives Element";
    private string pluginName = "Drives";

    /// <summary>
    /// Initializes a new instance of the <see cref="DrivesViewModel"/> class.
    /// </summary>
    public DrivesViewModel()
    {
        this.ElementScreenIntegrationConstraints = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.OnePerScreen);

        this.RefreshDrives();
    }

    public ICollection<DriveInfoViewModel> Drives { get; set; }

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new ();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary? ResourceDictionary => new () { Source = new Uri("/EEP_Drives;component/DefaultResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(DrivesView);

    public override Type ElementPluginViewModel => typeof(DrivesViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    private void RefreshDrives()
    {
        this.Drives = new List<DriveInfoViewModel>();

        foreach (var di in this.allDrives)
        {
            var divm = new DriveInfoViewModel(di, this);

            if (this.Drives.Count < this.allDrives.Length && !this.Drives.Contains(divm))
            {
                this.Drives.Add(divm);
            }
        }
    }

    [RelayCommand]
    private void OpenDrive(DriveInfo? driveInfo)
    {
        if (driveInfo != null)
        {
            try
            {
                Process.Start("explorer.exe", driveInfo.Name);
            }
            catch (Exception exception)
            {
                Log.Error($"could not process drive {driveInfo.Name}\n" +
                    $"{driveInfo.VolumeLabel}\n" +
                    $"{driveInfo.DriveFormat}\n" +
                    $"{exception.Message}\n" +
                    $"{exception.StackTrace}\n" +
                    $"{exception.Source}");
            }
        }
    }
}

// EOF