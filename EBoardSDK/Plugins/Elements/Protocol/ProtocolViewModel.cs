// <copyright file="ProtocolViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.Protocol;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public partial class ProtocolViewModel : PluginBaseViewModel
{
    [ObservableProperty]
    private Note note = new();

    [ObservableProperty]
    private int iD = -1;

    [ObservableProperty]
    private string title = "?";

    [ObservableProperty]
    private DateTime dateTimeCreated;

    [ObservableProperty]
    private DateTime dateTimeEdited;

    [ObservableProperty]
    private string content = "!";

    private string pluginHeader = "Protocol Element";

    private string pluginName = "Protocol";

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolViewModel"/> class.
    /// </summary>
    public ProtocolViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.OnlyModel);

        if (this.Note == null)
        {
            this.Note = new Note();
        }

        this.DateTimeCreated = DateTime.Now;
        this.DateTimeEdited = DateTime.Now;

        System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        timer.Tick += this.Timer_Tick;

        timer.Interval = new TimeSpan(0, 0, 0, 0, 125);

        timer.Start();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolViewModel"/> class.
    /// </summary>
    /// <param name="note"></param>
    public ProtocolViewModel(Note note)
        : this()
    {
        this.Note = note;
    }

    public static DateTime CurrentDateTime => DateTime.UtcNow;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(Note);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(ProtocolViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<Note>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(Note).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = this.Note;

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is Note note)
        {
            this.ApplyModel(note);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<Note>(json!);

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
        var model = new Note(this);

        this.SetModel(model);
    }

    public void SetNote(Note note)
    {
        this.Note = note;
    }

    private void ApplyModel(Note note)
    {
        this.Note = note;

        this.DateTimeCreated = note.DateTimeCreated;
        this.DateTimeEdited = note.DateTimeEdited;
        this.Content = note.Content;
        this.Title = note.Title;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        this.OnPropertyChanged(nameof(this.CurrentDateTime));
    }

    partial void OnIDChanged(int value)
    {
        this.Note.ID = value;
    }

    partial void OnTitleChanged(string value)
    {
        this.Note.Title = value;

        this.DateTimeEdited = CurrentDateTime;
    }

    partial void OnDateTimeCreatedChanged(DateTime value)
    {
        this.Note.DateTimeCreated = value;
    }

    partial void OnDateTimeEditedChanged(DateTime value)
    {
        this.Note.DateTimeEdited = value;
    }

    partial void OnContentChanged(string value)
    {
        this.Note.Content = value;

        this.DateTimeEdited = CurrentDateTime;
    }

    [RelayCommand]
    private void NewEntry()
    {
        this.DateTimeEdited = CurrentDateTime;
        this.Content = this.Content.Insert(0, $"{this.DateTimeEdited}\n\n\n");
    }
}

// EOF