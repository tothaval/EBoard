// <copyright file="LinkViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.Link;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Helper;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Eboard.Summoner;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public partial class LinkViewModel : PluginBaseViewModel, ICollectiveClickable
{
    private readonly string pluginHeader = "Link Element";
    private readonly string pluginName = "Link";

    private string simpleToolTip = string.Empty;

    private string detailedToolTip = string.Empty;

    [ObservableProperty]
    private bool hideToolTip = false;

    [ObservableProperty]
    private LinkTargets linkTargetType = LinkTargets.File;

    [ObservableProperty]
    private string editText = "Edit";

    [ObservableProperty]
    private string epicText = "this is the most epic text in the entire existance.";

    [ObservableProperty]
    private bool extendedToolTip = false;

    [ObservableProperty]
    private string linkStatusText = "unlinked";

    [ObservableProperty]
    private string linkTargetPath = string.Empty;

    [ObservableProperty]
    private string linkTargetName = string.Empty;

    [ObservableProperty]
    private string toolTipContent = string.Empty;

    [ObservableProperty]
    private ImageSource? imageSource;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.IsLinkEmpty))]
    private bool isLinked = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InverseEditBoolForTextBoxCaretSetting))]
    private bool isEditLinkTargetNameTextBoxReadOnly = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinkViewModel"/> class.
    /// </summary>
    public LinkViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.LinkStatusText = "unlinked";

        this.SetMenuItemViewModel(new LinkMenuItemViewModel(this));
        this.SetMenuItem(new LinkMenuItem(this.MenuItemViewModel!));

        this.OnPropertyChanged(nameof(this.MenuItemSet));
        this.OnPropertyChanged(nameof(this.MenuItem));
        this.OnPropertyChanged(nameof(this.MenuItemViewModel));

        this.OnPropertyChanged(nameof(this.InverseEditBoolForTextBoxCaretSetting));
    }

    public bool IsLinkEmpty => !this.IsLinked;

    public bool InverseEditBoolForTextBoxCaretSetting => !this.IsEditLinkTargetNameTextBoxReadOnly;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new ();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new ();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(LinkModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(LinkViewModel);

    public void ExecuteClick()
    {
        this.ExecuteOnClick();
    }

    public void TriggerToolTipVisibility(bool visible)
    {
        this.HideToolTip = !visible;
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        this.SetMenuItemViewModel(new LinkMenuItemViewModel(this));
        this.SetMenuItem(new LinkMenuItem(this.MenuItemViewModel!));

        this.OnPropertyChanged(nameof(this.ElementViewModel));
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<LinkModel>(path);

            if (data != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(LinkModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new LinkModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is LinkModel linkModel)
        {
            this.ApplyModel(linkModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<LinkModel>(json!);

            if (jsonParsed != null)
            {
                this.ApplyModel(jsonParsed);
            }
        }
        catch (JsonException jsonEx)
        {
            Log.Error(jsonEx.Message);

            this.Reset();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);

            this.Reset();
        }
    }

    public override void PrepareCopy()
    {
        var model = new LinkModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(LinkModel linkModel)
    {
        this.ExtendedToolTip = linkModel.ExtendedToolTip;
        this.LinkTarget(linkModel.LinkTarget, linkModel.LinkTargetPath ?? string.Empty, linkModel.LinkTargetName);
    }

    private void ExecuteOnClick()
    {
        if (this.IsLinked)
        {
            this.ExecuteLinkTarget();
            return;
        }
    }

    private void LinkTarget(LinkTargets linkTarget, string link, string? linkName = null)
    {
        this.LinkTargetType = linkTarget;

        switch (linkTarget)
        {
            case LinkTargets.File:
                this.LinkFileTarget(link, linkName);
                break;
            case LinkTargets.Web:
                this.LinkWebTarget(link, linkName);
                break;
            case LinkTargets.Folder:
                this.LinkFolderTarget(link, linkName);
                break;
            default:
                break;
        }
    }

    private void SetupToolTip(string detailedMessage)
    {
        this.detailedToolTip = detailedMessage;
        this.simpleToolTip = $"{this.LinkTargetPath}";

        if (this.ExtendedToolTip)
        {
            this.ToolTipContent = detailedMessage;
        }
        else
        {
            this.ToolTipContent = this.simpleToolTip;
        }
    }

    private void LinkFileTarget(string link, string? linkName = null)
    {
        try
        {
            var fileInfo = new FileInfo(link);

            if (fileInfo.Exists)
            {
                this.LinkTargetName = linkName ?? fileInfo.Name;
                this.LinkTargetPath = fileInfo.FullName;

                this.IsLinked = fileInfo.Exists;

                this.SetupToolTip(
                    $"{this.LinkTargetPath}\n" +
                    $"dir: {fileInfo.DirectoryName}\n" +
                    $"{fileInfo.Length} bytes\n" +
                    $"last access:{fileInfo.LastAccessTimeUtc}\n" +
                    $"last write: {fileInfo.LastWriteTimeUtc}\n" +
                    $"attributes: {fileInfo.Attributes}");

                if (Uri.IsWellFormedUriString(this.LinkTargetPath, UriKind.RelativeOrAbsolute))
                {
                    // thx to https://www.brad-smith.info/blog/archives/164 for IconTools.cs
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

                    // ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    //    icon.Handle,
                    //    Int32Rect.Empty,
                    //    BitmapSizeOptions.FromEmptyOptions());
                }

                this.OnPropertyChanged(nameof(this.ImageSource));
                this.OnPropertyChanged(nameof(this.IsLinkEmpty));
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    private void LinkFolderTarget(string folderName, string? linkName = null)
    {
        try
        {
            var dirInfo = new DirectoryInfo(folderName);

            if (dirInfo.Exists)
            {
                this.LinkTargetName = string.IsNullOrWhiteSpace(linkName) ? dirInfo.Name : linkName;
                this.LinkTargetPath = dirInfo.FullName;

                this.IsLinked = dirInfo.Exists;

                this.SetupToolTip($"{this.LinkTargetPath}\ndirs: {dirInfo.EnumerateDirectories().Count()}\nfiles: {dirInfo.EnumerateFiles().Count()}\ncreated:{dirInfo.CreationTimeUtc}\nlast access: {dirInfo.LastAccessTimeUtc}\nlast write: {dirInfo.LastWriteTimeUtc}");

                if (Uri.IsWellFormedUriString(this.LinkTargetPath, UriKind.RelativeOrAbsolute))
                {
                    // thx to https://www.brad-smith.info/blog/archives/164 for IconTools.cs
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

                    // ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    //    icon.Handle,
                    //    Int32Rect.Empty,
                    //    BitmapSizeOptions.FromEmptyOptions());
                }

                this.OnPropertyChanged(nameof(this.ImageSource));
                this.OnPropertyChanged(nameof(this.IsLinkEmpty));
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    private void LinkWebTarget(string linkTargetPath, string? linkName = null)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(linkTargetPath))
            {
                if (string.IsNullOrWhiteSpace(linkName))
                {
                    this.LinkTargetName = linkTargetPath;
                }
                else
                {
                    this.LinkTargetName = linkName;
                }

                if (Uri.IsWellFormedUriString(linkTargetPath, UriKind.RelativeOrAbsolute))
                {
                    this.LinkTargetPath = linkTargetPath;

                    this.IsLinked = true;

                    this.SetupToolTip($"{this.LinkTargetPath}\nwebsite");

                    // thx to https://www.brad-smith.info/blog/archives/164 for IconTools.cs
                    Icon icon = IconTools.GetIconForExtension(".html", ShellIconSize.LargeIcon);

                    using (Bitmap bmp = icon.ToBitmap())
                    {
                        var stream = new MemoryStream();
                        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        this.ImageSource = BitmapFrame.Create(stream);
                    }
                }

                this.OnPropertyChanged(nameof(this.ImageSource));
                this.OnPropertyChanged(nameof(this.IsLinkEmpty));
            }
        }
        catch (Exception)
        {
            this.Reset();
        }
    }

    partial void OnExtendedToolTipChanged(bool value)
    {
        this.SetupToolTip(this.detailedToolTip);
    }

    [RelayCommand]
    private void ExecuteLinkTarget()
    {
        try
        {
            ProcessStartInfo start = new ProcessStartInfo(this.LinkTargetPath)
            {
                UseShellExecute = true,
            };
            Process.Start(start);
        }
        catch
        {
        }
    }

    [RelayCommand]
    private void LinkFile()
    {
        Microsoft.Win32.OpenFileDialog setPath = new ();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");
        setPath.Filter = "files (*.*)|*.*";
        setPath.FilterIndex = 2;
        setPath.RestoreDirectory = true;

        if (setPath.ShowDialog() == true)
        {
            this.LinkTarget(LinkTargets.File, setPath.FileName);
        }
    }

    [RelayCommand]
    private void LinkFolder()
    {
        Microsoft.Win32.OpenFolderDialog setPath = new ();
        setPath.InitialDirectory = Environment.GetEnvironmentVariable("userdir");

        if (setPath.ShowDialog() == true)
        {
            this.LinkTarget(LinkTargets.Folder, setPath.FolderName, setPath.SafeFolderName);
        }
    }

    [RelayCommand]
    private void LinkWeb()
    {
        this.LinkTarget(LinkTargets.Web, this.LinkTargetPath);
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
        this.OnPropertyChanged(nameof(this.IsLinkEmpty));
    }

    [RelayCommand]
    private void SaveLinkTargetNameCommand()
    {
        this.IsEditLinkTargetNameTextBoxReadOnly = true;
        this.OnPropertyChanged(nameof(this.InverseEditBoolForTextBoxCaretSetting));
    }
}

// EOF