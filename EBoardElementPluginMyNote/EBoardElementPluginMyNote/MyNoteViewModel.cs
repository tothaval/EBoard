// <copyright file="MyNoteViewModel.cs" company=".">
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
namespace EBoardElementPluginMyNote;

using EBoardConfigManager.Helper;
using EBoardElementPluginMyNote.Models;
using EBoardElementPluginMyNote.ViewModels;
using EBoardSDK;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Eboard.Summoner;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public class MyNoteViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "MyNote";
    private readonly string pluginName = "MyNote";

    public MyNoteViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.OnePerScreen,
            copyConstraints: CopyConstraints.Denied);

        this.Protocol = new ProtocolViewModel();
        this.Notes = new NotesViewModel();
    }

    public ProtocolViewModel Protocol { get; }

    public NotesViewModel Notes { get; }

    public AreaViewModel<ProtocolViewModel>? AreaViewModel { get; set; }

    public override PluginCategories Category => PluginCategories.Element;

    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginMyNote;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(MyNoteModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(MyNoteViewModel);

    public Note GetProtocol => this.Protocol.Note;

    public List<ProtocolViewModel> GetNotes => this.Notes.GetNotes;

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is MyNoteModel myNoteModel)
        {
            this.ApplyModel(myNoteModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<MyNoteModel>(json!);

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
        var model = new MyNoteModel(this);

        this.SetModel(model);
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            if (this.AreaViewModel == null)
            {
                this.AreaViewModel = new AreaViewModel<ProtocolViewModel>(this.ElementViewModel);
            }

            if (this.AreaViewModel.ElementViewModel == null)
            {
                this.AreaViewModel.SetElementViewModel(this.ElementViewModel);
            }

            this.Protocol.SetElementViewModel(this.ElementViewModel);
            this.Notes.SetEBoardAndElementViewModel(this.ElementViewModel.ScreenViewModel, this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<MyNoteModel>(path);

            if (data != null && this.ElementViewModel != null)
            {
                this.ApplyModel(data);

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(MyNoteModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new MyNoteModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return await new SDKDataManager().SavePluginContent(model, path);
    }
    private void ApplyModel(MyNoteModel myNoteModel)
    {
        if (this.ElementViewModel == null || this.AreaViewModel == null)
        {
            return;
        }

        this.AreaViewModel.ShowMatrixControls = myNoteModel.ShowMatrixControls;

        this.Protocol.SetNote(myNoteModel.Note);

        this.Notes.SetNotes(myNoteModel.Notes);

        var counter = 0;

        foreach (var noteList in myNoteModel.NotesAreaList)
        {
            this.AreaViewModel?.AddHorizontal();

            var protocolViewModels = new List<ProtocolViewModel>();

            foreach (var note in noteList)
            {
                var protocolVM = new ProtocolViewModel(note);

                protocolVM.SetElementViewModel(this.ElementViewModel);

                protocolViewModels.Add(protocolVM);
            }

            this.AreaViewModel?.InsertViewModelList(protocolViewModels, counter);

            counter++;
        }

    }
}

// EOF