// <copyright file="SoundMixMainViewModel.cs" company=".">
// Stephan Kammel
// </copyright>


namespace EBoardSDK.Plugins.Elements.BasicAV;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using Microsoft.Win32;
using System;
using System.IO;
using System.Printing;
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
    private MediaElement media;

    private string pluginHeader = "BasicAV player";

    private string pluginName = "BasicAV";

    public BasicAVMainViewModel()
    {
        this.BrushManagement ??= new BrushManagement();

        this.BorderManagement ??= new BorderManagement();

        this.PluginLogo ??= new ImageBrush();

        this.Volume = 0.5;

        this.Media = new MediaElement()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            LoadedBehavior = MediaState.Manual,
            Stretch = Stretch.UniformToFill,
            AllowDrop = true,
            Focusable = false,
            StretchDirection = StretchDirection.Both,
            IsManipulationEnabled = false,
            IsHitTestVisible = false,
        };

        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(0.5);
        timer.Tick += this.Timer_Tick;
        timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (this.Media.Source != null
            && this.Media.NaturalDuration.HasTimeSpan
            && !this.userIsDraggingSlider)
        {
            this.PlayTimeSpan = this.Media.Position.TotalSeconds;
            this.MaximumPlayTime = this.Media.NaturalDuration.TimeSpan.TotalSeconds;
        }
    }

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override bool NoDefaultBorders { get; } = false;

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override string ElementPluginName => "BasicAV";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(BasicAVMainView);

    public override Type ElementPluginViewModel => typeof(BasicAVMainViewModel);

    public bool InsertBasicAVModel(BasicAVModel basicAVModel)
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
                        this.Media.Source = new Uri(this.Filepath);
                        this.Media.Position = TimeSpan.FromSeconds(basicAVModel.PlayTimeSpan);
                        this.MaximumPlayTime = this.Media.NaturalDuration.TimeSpan.TotalSeconds;
                        this.PlayTimeSpan = basicAVModel.PlayTimeSpan;
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
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<BasicAVModel>(path)!;

            if (this.InsertBasicAVModel(data))
            {
                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
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

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
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
        this.Media.Pause();
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
            this.Media.Pause();
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
            this.Media.Source = new Uri(openFileDialog.FileName);
            this.Filepath = openFileDialog.FileName;
            this.FileName = openFileDialog.SafeFileName;
        }
    }

    [RelayCommand]
    private void Stop()
    {
        if (this.mediaPlayerIsPlaying)
        {
            this.Media.Stop();
            this.mediaPlayerIsPlaying = false;
        }
    }
}

// EOF