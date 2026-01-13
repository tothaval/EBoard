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

using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardElementPluginMyNote.Models;
using EBoardElementPluginMyNote.ViewModels;
using EBoardSDK;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.Protocol;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public class MyNoteViewModel : EBoardElementPluginBaseViewModel
{
    private string pluginHeader = "MyNote";
    private string pluginName = "MyNote";

    public MyNoteViewModel()
    {
        this.ElementScreenIntegrationConstraints = new EBoardSDK.Models.ElementScreenIntegrationConstraints(ElementInstantiationPolicy.Unconstrained);

        this.Protocol = new ProtocolViewModel();
        this.Notes = new NotesViewModel();
        this.AreaViewModel = new AreaViewModel<ProtocolViewModel>(this.ElementViewModel);
    }

    public ProtocolViewModel Protocol { get; }

    public NotesViewModel Notes { get; }

    public AreaViewModel<ProtocolViewModel> AreaViewModel { get; }

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EBoardElementPluginMyNote;component/ElementPluginResourceDictionary.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(MyNoteView);

    public override Type ElementPluginViewModel => typeof(MyNoteViewModel);

    public Note GetProtocol => this.Protocol.Note;

    public List<ProtocolViewModel> GetNotes => this.Notes.GetNotes;

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.AreaViewModel.SetElementViewModel(this.ElementViewModel);

            this.Protocol.SetEBoardAndElementViewModel(this.ElementViewModel.EBoardViewModel, this.ElementViewModel);
            this.Notes.SetEBoardAndElementViewModel(this.ElementViewModel.EBoardViewModel, this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<MyNoteModel>(path)!;

            if (data != null)
            {
                this.Protocol.SetNote(data.Note);

                this.Notes.SetNotes(data.Notes);

                var counter = 0;

                // TODO stuff
                foreach (var noteList in data.NotesAreaList)
                {
                    this.AreaViewModel.AddHorizontal();

                    var protocolViewModels = new List<ProtocolViewModel>();

                    foreach (var note in noteList)
                    {
                        // ggf ueber Konstruktor refaktoring verkuerzen
                        var protocolVM = new ProtocolViewModel(note);

                        protocolVM.SetEBoardAndElementViewModel(this.EBoardViewModel, this.ElementViewModel);

                        protocolViewModels.Add(protocolVM);
                    }

                    this.AreaViewModel.InsertViewModelList(protocolViewModels, counter);

                    counter++;
                }

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

        var model = new MyNoteModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }
}

// EOF