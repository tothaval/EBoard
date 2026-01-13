// <copyright file="FileLinkAreaViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Areas.FileLinkArea;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Plugins.Elements.Link;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class FileLinkAreaViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string executeAllLinksButtonContent = "\n*\n*\n*\n";

    private string pluginHeader = "FileLink Area";
    private string pluginName = "FileLinkArea";

    /// <summary>
    /// Initializes a new instance of the <see cref="FileLinkAreaViewModel"/> class.
    /// </summary>
    public FileLinkAreaViewModel()
    {
    }

    public AreaViewModel<LinkViewModel> AreaViewModel { get; set; }

    public override PluginCategories PluginCategory => PluginCategories.Area;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(FileLinkAreaView);

    public override Type ElementPluginViewModel => typeof(FileLinkAreaViewModel);

    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.AreaViewModel = new AreaViewModel<LinkViewModel>(this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
            this.OnPropertyChanged(nameof(this.ElementViewModel));
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<FileLinkAreaModel>(path);

            // TODO ggf hier und andernorts generisch machen und refactorn
            if (data != null)
            {
                var counter = 0;

                // TODO stuff
                foreach (var linkModelList in data.Links)
                {
                    this.AreaViewModel.AddHorizontal();

                    var linkViewModels = new List<LinkViewModel>();

                    foreach (var linkModel in linkModelList)
                    {
                        // ggf ueber Konstruktor refaktoring verkuerzen
                        var linkVM = new LinkViewModel();

                        linkVM.SetEBoardAndElementViewModel(this.EBoardViewModel, this.ElementViewModel);

                        linkVM.InsertLinkModel(linkModel);

                        linkViewModels.Add(linkVM);
                    }

                    this.AreaViewModel.InsertViewModelList(linkViewModels, counter);

                    counter++;
                }

                this.OnPropertyChanged(nameof(this.AreaViewModel));

                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
            }
        }
        catch (Exception ex)
        {
            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
        }

        return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = "" };
    }

    public async override Task<EBoardFeedbackMessage> Save(string path)
    {
        EBoardFeedbackMessage? serializationResult = null;

        var model = new FileLinkAreaModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    [RelayCommand]
    private void ExecuteAllLinks()
    {
        var allLinkViewModels = this.AreaViewModel.GetAllViewModelsList();

        foreach (var item in allLinkViewModels)
        {
            foreach (var linkViewModel in item)
            {
                linkViewModel.ExecuteClick();
            }
        }
    }
}

// EOF