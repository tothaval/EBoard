// <copyright file="LinkViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Elements.Link;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public partial class LinkViewModel : EBoardElementPluginBaseViewModel, ICollectiveClickableObject
{
    private readonly string linkDataFileName = "linkdata.xml";

    [ObservableProperty]
    private string epicText = "this is the most epic text in the entire existance.";

    [ObservableProperty]
    private string linkStatusText;

    [ObservableProperty]
    private string linkTargetPath;

    [ObservableProperty]
    private string linkTargetName;

    [ObservableProperty]
    private ImageSource? imageSource;

    [ObservableProperty]
    private bool isLinked = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InverseEditBoolForTextBoxCaretSetting))]
    private bool isEditLinkTargetNameTextBoxReadOnly = true;

    [ObservableProperty]
    private string editText = "Edit";

    public bool InverseEditBoolForTextBoxCaretSetting => !this.IsEditLinkTargetNameTextBoxReadOnly;

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Link Element";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "Link";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "Link";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(LinkView);

    public override Type ElementPluginViewModel => typeof(LinkViewModel);

    public Action CollectiveClickEvent => this.ExecuteOnClick;

    public LinkViewModel()
    {
        this.OnPropertyChanged(nameof(this.InverseEditBoolForTextBoxCaretSetting));
    }

    public void InsertLinkModel(LinkModel linkModel)
    {
        try
        {
            if (linkModel != null)
            {
                this.LinkFile(linkModel.LinkTargetPath, linkModel.LinkTargetName);
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    private void ExecuteOnClick()
    {
        if (this.IsLinked)
        {
            this.ExecuteLinkTarget();
            return;
        }
    }

    [RelayCommand]
    private void ExecuteLinkTarget()
    {
        try
        {
            ProcessStartInfo start = new ProcessStartInfo(this.LinkTargetPath)
            {
                UseShellExecute = true
            };
            Process.Start(start);
        }
        catch
        {
        }
    }


    [RelayCommand]
    private void Link()
    {
        Microsoft.Win32.OpenFileDialog setPath = new Microsoft.Win32.OpenFileDialog();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");
        setPath.Filter = "files (*.*)|*.*";
        setPath.FilterIndex = 2;
        setPath.RestoreDirectory = true;

        if (setPath.ShowDialog() == true)
        {
            this.LinkFile(setPath.FileName);
        }
    }

    private void LinkFile(string fileName, string? linkName = null)
    {
        try
        {
            var fileInfo = new FileInfo(fileName);

            if (fileInfo.Exists)
            {
                this.LinkTargetName = linkName ?? fileInfo.Name;
                this.LinkTargetPath = fileInfo.FullName;

                this.IsLinked = fileInfo.Exists;

                if (Uri.IsWellFormedUriString(this.LinkTargetPath, UriKind.RelativeOrAbsolute))
                {
                    //thx to https://www.brad-smith.info/blog/archives/164 for IconTools.cs
                    Icon icon = IconTools.GetIconForExtension(".html", ShellIconSize.LargeIcon);

                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        var stream = new MemoryStream();
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        this.ImageSource = BitmapFrame.Create(stream);
                    }
                }
                else
                {
                    Icon icon = IconTools.GetIconForFile(this.LinkTargetPath, ShellIconSize.LargeIcon);

                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        var stream = new MemoryStream();
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        this.ImageSource = BitmapFrame.Create(stream);

                    }

                    //// alternative ImageSource solution
                    //// thx to: https://stackoverflow.com/questions/1127647/convert-system-drawing-icon-to-system-media-imagesource

                    //ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    //    icon.Handle,
                    //    Int32Rect.Empty,
                    //    BitmapSizeOptions.FromEmptyOptions());
                }

                this.OnPropertyChanged(nameof(this.ImageSource));
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    [RelayCommand]
    private void Reset()
    {
        this.IsLinked = false;
        this.LinkTargetName = string.Empty;
        this.LinkTargetPath = string.Empty;

        this.LinkStatusText = $"unlinked";

        this.ImageSource = null;

        this.OnPropertyChanged(nameof(this.ImageSource));
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<LinkModel>(path);

            if (data != null)
            {
                this.LinkFile(data.LinkTargetPath, data.LinkTargetName);

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = string.Empty };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new LinkModel() { LinkTargetName = this.LinkTargetName, LinkTargetPath = this.LinkTargetPath };

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }

    [RelayCommand]
    private void EditLinkTargetName()
    {
        if (this.IsEditLinkTargetNameTextBoxReadOnly)
        {
            this.IsEditLinkTargetNameTextBoxReadOnly = false;
            this.EditText = "Edit";
        }
        else
        {
            this.IsEditLinkTargetNameTextBoxReadOnly = true;
            this.EditText = "Save";
        }

        this.OnPropertyChanged(nameof(this.InverseEditBoolForTextBoxCaretSetting));
    }

    [RelayCommand]
    private void SaveLinkTargetNameCommand()
    {
        this.IsEditLinkTargetNameTextBoxReadOnly = true;
        this.OnPropertyChanged(nameof(this.InverseEditBoolForTextBoxCaretSetting));
    }
}

// EOF