namespace EEP_Drives;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using System;
using System.IO;

public partial class DriveInfoViewModel : ObservableObject
{
    private readonly BrushManagement brushManagement;

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

    public IPlugin Plugin => plugin;

    public DriveInfoViewModel(DriveInfo drive, IPlugin plugin)
    {
        this.plugin = plugin;

        this.brushManagement = plugin.BrushManagement;
        this.BorderManagement = plugin.BorderManagement;

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

    public BrushManagement BrushManagement => brushManagement;

    public BorderManagement BorderManagement { get; }
}

// EOF