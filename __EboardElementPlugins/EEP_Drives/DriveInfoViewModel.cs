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
using EBoardSDK.Interfaces;
using System;
using System.IO;

public partial class DriveInfoViewModel : ObservableObject
{
    private readonly DriveInfo driveInfo;

    [ObservableProperty]
    private string freeSpace;

    private double gigabyte = 1024 * 1024 * 1024;

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

    private IPlugin plugin;

    public IPlugin Plugin => this.plugin;

    public DriveInfoViewModel(DriveInfo drive, IPlugin plugin)
    {
        this.plugin = plugin;

        this.driveInfo = drive;

        if (this.driveInfo.IsReady)
        {
            var sizeGigaByte = this.DriveInfo.TotalFreeSpace / this.gigabyte;
            this.FreeSpace = $"{sizeGigaByte:N2} gb";

            sizeGigaByte = this.DriveInfo.AvailableFreeSpace / this.gigabyte;
            this.AvailableFreeSpace = $"{sizeGigaByte:N2} ";

            var totalsizeGigaByte = this.DriveInfo.TotalSize / this.gigabyte;
            this.Size = $"{totalsizeGigaByte:N2} gb";

            this.DriveFormat = $"{this.DriveInfo.DriveFormat}";

            this.VolumeLabel = $"{this.DriveInfo.VolumeLabel}";

            this.SizeIndicationPercent = Math.Round(100 - (sizeGigaByte / totalsizeGigaByte * 100), 2);
        }
        else
        {
            this.AvailableFreeSpace = $"-";

            this.Size = $"-";

            this.DriveFormat = $"-";

            this.VolumeLabel = $"not ready / empty";

            this.SizeIndicationPercent = 0;
        }

        this.Name = $"{this.DriveInfo.Name}";

        this.DriveType = $"{this.DriveInfo.DriveType} type";
        this.RootDirectory = $"{this.DriveInfo.RootDirectory}";
    }

    public DriveInfo DriveInfo => this.driveInfo;
}

// EOF