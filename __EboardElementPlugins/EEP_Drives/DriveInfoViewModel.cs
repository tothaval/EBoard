// <copyright file="DriveInfoViewModel.cs" company=".">
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
namespace EEP_Drives;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.Link;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public partial class DriveInfoViewModel : ObservableObject
{
    private LinkViewModel linkViewModel;

    private DrivesViewModel drivesViewModel;

    private readonly DriveInfo driveInfo;

    [ObservableProperty]
    private string freeSpace;

    private double gigabyte = 1024 * 1024 * 1024;

    [ObservableProperty]
    private string toolTipText;

    [ObservableProperty]
    private string size;

    [ObservableProperty]
    private string driveType;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private string rootDirectory;

    [ObservableProperty]
    private double sizeIndicationPercent;

    [ObservableProperty]
    private string availableFreeSpace;

    [ObservableProperty]
    private string driveFormat;

    [ObservableProperty]
    private string volumeLabel;

    /// <summary>
    /// Initializes a new instance of the <see cref="DriveInfoViewModel"/> class.
    /// </summary>
    /// <param name="drive"></param>
    /// <param name="plugin"></param>
    /// <param name="drivesViewModel"></param>
    public DriveInfoViewModel(DriveInfo drive, DrivesViewModel drivesViewModel)
    {
        this.drivesViewModel = drivesViewModel;

        this.linkViewModel = new LinkViewModel();

        if (this.DrivesViewModel.ElementViewModel != null)
        {
            this.linkViewModel.SetElementViewModel(this.DrivesViewModel.ElementViewModel);
            this.linkViewModel.RefreshInitialization();
        }

        this.driveInfo = drive;

        StringBuilder tooltip = new StringBuilder();

        if (this.driveInfo.IsReady)
        {
            var sizeGigaByte = this.DriveInfo.TotalFreeSpace / this.gigabyte;
            tooltip.AppendLine($"{sizeGigaByte:N2} gb");

            sizeGigaByte = this.DriveInfo.AvailableFreeSpace / this.gigabyte;
            tooltip.AppendLine($"{sizeGigaByte:N2} ");

            var totalsizeGigaByte = this.DriveInfo.TotalSize / this.gigabyte;
            tooltip.AppendLine($"{totalsizeGigaByte:N2} gb");

            tooltip.AppendLine($"{this.DriveInfo.DriveFormat}");

            tooltip.AppendLine($"{this.DriveInfo.VolumeLabel}");

            this.SizeIndicationPercent = Math.Round(100 - (sizeGigaByte / totalsizeGigaByte * 100), 2);
        }
        else
        {
            tooltip.AppendLine(this.AvailableFreeSpace = $"-");
            tooltip.AppendLine(this.Size = $"-");
            tooltip.AppendLine(this.DriveFormat = $"-");
            tooltip.AppendLine(this.VolumeLabel = $"not ready / empty");

            this.SizeIndicationPercent = 0;
        }

        this.LinkViewModel.InsertModel(
            new LinkModel()
            {
                LinkTargetPath = this.DriveInfo.RootDirectory.FullName,
                LinkTargetName = this.DriveInfo.Name,
                LinkTarget = EBoardSDK.Enums.LinkTargets.Folder,
                ExtendedToolTip = true,
            });

        this.LinkViewModel.TriggerToolTipVisibility(false);

        this.ToolTipText = tooltip.ToString();

        this.Name = $"{this.DriveInfo.Name}";

        this.DriveType = $"{this.DriveInfo.DriveType} type";
        this.RootDirectory = $"{this.DriveInfo.RootDirectory}";

        this.OnPropertyChanged(nameof(this.LinkViewModel));
        this.OnPropertyChanged(nameof(this.DrivesViewModel));
    }

    public DrivesViewModel DrivesViewModel => this.drivesViewModel;

    public LinkViewModel LinkViewModel => this.linkViewModel;

    public DriveInfo DriveInfo => this.driveInfo;

    [RelayCommand]
    private void OpenDrive()
    {
        if (this.DriveInfo != null)
        {
            try
            {
                Process.Start("explorer.exe", this.DriveInfo.Name);
            }
            catch (Exception exception)
            {
                Log.Error($"could not process drive {this.DriveInfo.Name}\n" +
                    $"{this.DriveInfo.VolumeLabel}\n" +
                    $"{this.DriveInfo.DriveFormat}\n" +
                    $"{exception.Message}\n" +
                    $"{exception.StackTrace}\n" +
                    $"{exception.Source}");
            }
        }
    }
}

// EOF