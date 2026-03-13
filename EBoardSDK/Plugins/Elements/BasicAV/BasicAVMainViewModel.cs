// <copyright file="BasicAVMainViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.BasicAV;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
<<<<<<< Updated upstream
=======
using EBoardSDK.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

public partial class BasicAVMainViewModel : EBoardElementPluginBaseViewModel
{
    private bool mediaPlayerIsPlaying = false;

    private bool userIsDraggingSlider = false;

    private DispatcherTimer? timer = null;

    [ObservableProperty]
    private string playTimeString = "00:00:00";

    [ObservableProperty]
    private string maximumPlayTimeString = "00:00:00";

    [ObservableProperty]
    private double playTimeSpan;

    [ObservableProperty]
    private string selectButtonContent = "_select";

    [ObservableProperty]
    private string playButtonContent = ">";

    [ObservableProperty]
    private string pauseButtonContent = "||";

    [ObservableProperty]
    private string stopButtonContent = "|<<";

    [ObservableProperty]
    private double maximumPlayTime;

    [ObservableProperty]
    private string fileName = "select a media file";

    [ObservableProperty]
    private string filepath = string.Empty;

    [ObservableProperty]
    private double volume;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMediaSelected))]
    private MediaElement? media;

<<<<<<< Updated upstream
    private string pluginHeader = "BasicAV player";

    private string pluginName = "BasicAV";
=======
    [ObservableProperty]
    private bool activateReplay = false;

    [ObservableProperty]
    private bool destructOnEnd = false;

    [ObservableProperty]
    private bool isGrouped = false;

    [ObservableProperty]
    private bool showDestructToggleButton = false;

    [ObservableProperty]
    private bool showGroupToggleButton = false;

    [ObservableProperty]
    private bool unlinkOnEnd = false;

    [ObservableProperty]
    private int processingOrder;
>>>>>>> Stashed changes

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAVMainViewModel"/> class.
    /// </summary>
    public BasicAVMainViewModel()
    {
        this.Volume = 0.5;
    }

<<<<<<< Updated upstream
    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (this.Media != null)
        {
            if (this.Media.Source != null
                && this.Media.NaturalDuration.HasTimeSpan
                && !this.userIsDraggingSlider)
            {
                this.PlayTimeSpan = this.Media.Position.TotalSeconds;
                this.MaximumPlayTime = this.Media.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }
    }

    public override PluginCategories PluginCategory => PluginCategories.Element;
=======
    public bool IsMediaSelected => this.Media != null;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();
>>>>>>> Stashed changes

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

<<<<<<< Updated upstream
    public override bool NoDefaultBorders { get; } = false;
=======
    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();
>>>>>>> Stashed changes

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

<<<<<<< Updated upstream
    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(BasicAVMainView);

    public override Type ElementPluginViewModel => typeof(BasicAVMainViewModel);

    public bool InsertBasicAVModel(BasicAVModel basicAVModel)
=======
    public override void Dispose()
    {
        base.Dispose();

        this.PlayTimeSpan = 0.0;
        this.MaximumPlayTime = 0.0;
        this.FileName = "select a media file";
        this.Filepath = string.Empty;

        if (this.Media != null)
        {
            this.Media.Stop();
            this.Media.Close();
            this.Media = null;
        }

        if (this.timer != null)
        {
            this.timer.Stop();
            this.timer.Tick -= this.Timer_Tick;
            this.timer = null;
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
>>>>>>> Stashed changes
    {
        try
        {
            if (basicAVModel != null)
            {
                this.FileName = basicAVModel.Filename;
                this.Filepath = basicAVModel.Filepath;
                this.Volume = basicAVModel.Volume;

                if (!string.IsNullOrWhiteSpace(this.Filepath))
                {
                    var fileInfo = new FileInfo(this.Filepath);

                    if (fileInfo.Exists)
                    {
                        if (this.Media != null)
                        {
                            this.Media.Source = new Uri(this.Filepath);
                            this.Media.Position = TimeSpan.FromSeconds(basicAVModel.PlayTimeSpan);
                            this.MaximumPlayTime = this.Media.NaturalDuration.TimeSpan.TotalSeconds;
                            this.PlayTimeSpan = basicAVModel.PlayTimeSpan;
                        }
                    }
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            var msg = ex;
        }

        return false;
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<BasicAVModel>(path)!;

            if (data != null)
            {
                if (this.InsertBasicAVModel(data))
                {
                    return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
                }
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = string.Empty };
    }

    public override async Task<EBoardFeedbackMessage> Save(string path)
    {
        var model = new BasicAVModel(this);

        EBoardFeedbackMessage? serializationResult = null;

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    public void StatusBarDragCompleted()
    {
        if (this.Media != null && this.Media.Source != null)
        {
            this.Media.Position = TimeSpan.FromSeconds(this.PlayTimeSpan);
            this.userIsDraggingSlider = false;
            this.Media.Play();
        }
    }

    public void StatusBarDragStart()
    {
        this.userIsDraggingSlider = true;
        this.Media?.Pause();
    }

<<<<<<< Updated upstream
=======
    internal void SetMediaFile(FileInfo fileInfo, bool startPlaying = false)
    {
        if (!fileInfo.Exists)
        {
            return;
        }

        if (this.timer != null)
        {
            this.timer.Stop();
            this.timer.Tick -= this.Timer_Tick;
            this.timer = null;
        }

        if (this.Media == null)
        {
            this.Media = new MediaElement()
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Manual,
            };
        }

        if (this.Media != null)
        {
            this.Media.Stop();
            this.Media.Close();

            this.Media.Source = new Uri(fileInfo.FullName);
            this.Filepath = fileInfo.FullName;
        }

        if (startPlaying)
        {
            this.Play();
        }
    }

    private void ApplyModel(BasicAVModel basicAVModel)
    {
        this.Volume = basicAVModel.Volume;
        this.FileName = basicAVModel.Filename;
        this.Filepath = basicAVModel.Filepath;

        if (!string.IsNullOrWhiteSpace(basicAVModel.Filepath))
        {
            var fileInfo = new FileInfo(this.Filepath);

            this.SetMediaFile(fileInfo, false);

            this.PlayTimeSpan = basicAVModel.PlayTimeSpan;

            if (this.Media != null)
            {
                this.Media.Position = TimeSpan.FromSeconds(this.PlayTimeSpan);
            }
        }

        this.ActivateReplay = basicAVModel.ActivateReplay;
        this.DestructOnEnd = basicAVModel.DestructOnEnd;
        this.IsGrouped = basicAVModel.IsGrouped;
        this.ShowDestructToggleButton = basicAVModel.ShowDestructToggleButton;
        this.ShowGroupToggleButton = basicAVModel.ShowGroupToggleButton;
        this.UnlinkOnEnd = basicAVModel.UnlinkOnEnd;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (this.Media != null)
        {
            if (this.Media.Source != null
                && this.Media.NaturalDuration.HasTimeSpan
                && !this.userIsDraggingSlider)
            {
                this.PlayTimeSpan = this.Media.Position.TotalSeconds;
                this.MaximumPlayTime = this.Media.NaturalDuration.TimeSpan.TotalSeconds;

                var isOnEndPosition = this.Media.Position.TotalSeconds == this.Media.NaturalDuration.TimeSpan.TotalSeconds;

                if (isOnEndPosition && this.ActivateReplay)
                {
                    this.PlayTimeSpan = 0.0;
                }

                if (isOnEndPosition && this.DestructOnEnd)
                {
                    this.ElementViewModel?.Delete(confirmRemove: false);
                }

                if (isOnEndPosition && this.UnlinkOnEnd)
                {
                    this.Dispose();
                }
            }
        }
    }

>>>>>>> Stashed changes
    partial void OnPlayTimeSpanChanged(double value)
    {
        if (Media != null && Media.Source != null)
        {
            this.PlayTimeString = TimeSpan.FromSeconds(this.PlayTimeSpan).ToString(@"hh\:\:mm\:ss");
            this.Media.Position = TimeSpan.FromSeconds(this.PlayTimeSpan);
        }
    }

    partial void OnMaximumPlayTimeChanged(double value)
    {
        if (Media != null && Media.Source != null)
        {
            this.MaximumPlayTimeString = TimeSpan.FromSeconds(this.MaximumPlayTime).ToString(@"hh\:\:mm\:ss");
        }
    }

    partial void OnVolumeChanged(double value)
    {
        if (Media != null && Media.Source != null)
        {
            this.Media.Volume = this.Volume;
        }
    }

    partial void OnActivateReplayChanged(bool value)
    {
        if (value && this.DestructOnEnd)
        {
            this.DestructOnEnd = false;
        }

        if (value && this.UnlinkOnEnd)
        {
            this.UnlinkOnEnd = false;
        }
    }

    partial void OnDestructOnEndChanged(bool value)
    {
        if (value && this.ActivateReplay)
        {
            this.ActivateReplay = false;
        }

        if (value && this.UnlinkOnEnd)
        {
            this.UnlinkOnEnd = false;
        }
    }

    partial void OnUnlinkOnEndChanged(bool value)
    {
        if (value && this.ActivateReplay)
        {
            this.ActivateReplay = false;
        }

        if (value && this.DestructOnEnd)
        {
            this.DestructOnEnd = false;
        }
    }

    [RelayCommand]
    private void Pause()
    {
        if (this.mediaPlayerIsPlaying && this.timer != null)
        {
            this.Media?.Pause();

            this.timer.Stop();
        }
    }

    [RelayCommand]
    private void Play()
    {
        if (this.timer == null)
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(0.5);
            this.timer.Tick += this.Timer_Tick;
        }

        if (this.Media != null && this.Media.Source != null)
        {
            this.Media.Position = TimeSpan.FromSeconds(this.PlayTimeSpan);

            this.timer.Start();
            this.Media.Play();
            this.mediaPlayerIsPlaying = true;
        }
    }

    [RelayCommand]
    private void Select()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter =

            // "Media files " +
            // "(*.mp3;*.mpg,*.mpeg)|*.mp3,*.mpg,*.mpeg|" +
            "All files (*.*)|*.*";
        if (openFileDialog.ShowDialog() == true)
        {
            this.FileName = openFileDialog.SafeFileName;
            this.Filepath = openFileDialog.FileName;
            this.SetMediaFile(new FileInfo(openFileDialog.FileName), false);
        }
    }

    [RelayCommand]
    private void Stop()
    {
        if (this.mediaPlayerIsPlaying && this.timer != null)
        {
            this.Media?.Stop();
            this.mediaPlayerIsPlaying = false;

            this.timer.Stop();
        }
    }
}

// EOF