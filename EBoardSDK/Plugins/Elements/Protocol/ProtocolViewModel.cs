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
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class ProtocolViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private Note note;

    [ObservableProperty]
    private int iD = -1;

    [ObservableProperty]
    private string title = "?";

    public DateTime CurrentDateTime => DateTime.Now;

    [ObservableProperty]
    private DateTime dateTime_Created;

    [ObservableProperty]
    private DateTime dateTime_Edited;

    [ObservableProperty]
    private string content = "!";

    /// <summary>
    /// Initializes a new instance of the <see cref="ProtocolViewModel"/> class.
    /// </summary>
    public ProtocolViewModel()
    {
        if (this.Note == null)
        {
            this.Note = new Note();
        }

        this.DateTime_Created = DateTime.Now;
        this.DateTime_Edited = DateTime.Now;

        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        Timer.Tick += this.Timer_Tick;

        Timer.Interval = new TimeSpan(0, 0, 0, 0, 125);

        Timer.Start();
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

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Protocol Element";

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    private string pluginName = "Protocol";

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(ProtocolView);

    public override Type ElementPluginViewModel => typeof(ProtocolViewModel);

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<Note>(path)!;

            if (data != null)
            {
                this.note = data;

                this.DateTime_Created = data.DateTime_Created;
                this.DateTime_Edited = data.DateTime_Edited;
                this.Content = data.Content;
                this.Title = data.Title;

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
        EBoardFeedbackMessage? serializationResult = null;

        var model = this.Note;

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    public void SetNote(Note note)
    {
        this.Note = note;
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

        this.DateTime_Edited = this.CurrentDateTime;
    }

    partial void OnDateTime_CreatedChanged(DateTime value)
    {
        this.Note.DateTime_Created = value;
    }

    partial void OnDateTime_EditedChanged(DateTime value)
    {
        this.Note.DateTime_Edited = value;
    }

    partial void OnContentChanged(string value)
    {
        this.Note.Content = value;

        this.DateTime_Edited = this.CurrentDateTime;
    }

    [RelayCommand]
    private void NewEntry()
    {
        this.DateTime_Edited = this.CurrentDateTime;
        this.Content = this.Content.Insert(0, $"{this.DateTime_Edited}\n\n\n");
    }
}

// EOF