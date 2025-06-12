namespace EEP_Drives;

using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using Serilog;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class DrivesViewModel : EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "Drives Element";
    private string pluginName = "Drives";

    private DriveInfo[] allDrives = DriveInfo.GetDrives();

    public DrivesViewModel()
    {
        this.BorderManagement = new BorderManagement();
        this.BrushManagement = new BrushManagement();

        this.ElementScreenIntegrationConstraints = new ElementScreenIntegrationConstraints(ElementInstantiationPolicy.OnePerScreen);

        this.BrushManagement.PropertyChangedEvent += BrushManagement_PropertyChangedEvent;


        this.RefreshDrives();
    }

    private void BrushManagement_PropertyChangedEvent()
    {
        RefreshDrives();
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

    public ICollection<DriveInfoViewModel> Drives { get; set; }

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }


    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Drives";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary? ResourceDictionary => new() { Source = new Uri("/EEP_Drives;component/DefaultResourceDictionary.xaml", uriKind: UriKind.Relative) };

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