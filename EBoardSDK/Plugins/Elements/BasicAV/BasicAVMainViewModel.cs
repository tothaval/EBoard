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
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Microsoft.Win32;
using Serilog;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

/// <summary>
/// Plugin for media playing. Uses a WPF framework <see cref="MediaElement"/>
/// and offers limited amount of controls to use it.
/// </summary>
public partial class BasicAVMainViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "BasicAV player";
    private readonly string pluginName = "BasicAV";

    private bool mediaPlayerIsPlaying = false;

    private bool userIsDraggingSlider = false;

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
    private MediaElement? media;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicAVMainViewModel"/> class.
    /// </summary>
    public BasicAVMainViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.Volume = 0.5;

        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(0.5);
        timer.Tick += this.Timer_Tick;
        timer.Start();
    }

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
    public override Type? PluginModelType => typeof(BasicAVModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(BasicAVMainViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<BasicAVModel>(path);

            if (data != null)
            {
                this.InsertModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(BasicAVModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new BasicAVModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is BasicAVModel basicAVModel)
        {
            this.ApplyModel(basicAVModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<BasicAVModel>(json!);

            if (jsonParsed != null)
            {
                this.ApplyModel(jsonParsed);
            }
        }
        catch (JsonException jsonEx)
        {
            Log.Error(jsonEx.Message);

            throw;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);

            throw;
        }
    }

    public override void PrepareCopy()
    {
        var model = new BasicAVModel(this);

        this.SetModel(model);
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

    private void ApplyModel(BasicAVModel basicAVModel)
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
            }
        }
    }

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

    [RelayCommand]
    private void Pause()
    {
        if (this.mediaPlayerIsPlaying)
        {
            this.Media?.Pause();
        }
    }

    [RelayCommand]
    private void Play()
    {
        if (this.Media != null && this.Media.Source != null)
        {
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
                this.Media.Source = new Uri(openFileDialog.FileName);
                this.Filepath = openFileDialog.FileName;
                this.FileName = openFileDialog.SafeFileName;
            }
        }
    }

    [RelayCommand]
    private void Stop()
    {
        if (this.mediaPlayerIsPlaying)
        {
            this.Media?.Stop();
            this.mediaPlayerIsPlaying = false;
        }
    }
}

// EOF