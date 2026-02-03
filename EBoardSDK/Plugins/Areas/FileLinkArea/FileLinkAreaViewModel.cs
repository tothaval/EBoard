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
using EBoardSDK.Controls.Area;
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

public partial class FileLinkAreaViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "FileLink Area";
    private readonly string pluginName = "FileLinkArea";

    [ObservableProperty]
    private string executeAllLinksButtonContent = "\n*\n*\n*\n";

    /// <summary>
    /// Initializes a new instance of the <see cref="FileLinkAreaViewModel"/> class.
    /// </summary>
    public FileLinkAreaViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

    public AreaViewModel<LinkViewModel> AreaViewModel { get; set; }

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Area;

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
    public override Type? PluginModelType => typeof(FileLinkAreaModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(FileLinkAreaViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            this.AreaViewModel = new AreaViewModel<LinkViewModel>(this.ElementViewModel);

            this.OnPropertyChanged(nameof(this.AreaViewModel));
            this.OnPropertyChanged(nameof(this.ElementViewModel));
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<FileLinkAreaModel>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(FileLinkAreaModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new FileLinkAreaModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is FileLinkAreaModel fileLinkAreaModel)
        {
            this.ApplyModel(fileLinkAreaModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<FileLinkAreaModel>(json!);

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
        var model = new FileLinkAreaModel(this);

        this.SetModel(model);
    }

    private void ApplyModel(FileLinkAreaModel fileLinkAreaModel)
    {
        if (this.ElementViewModel == null || this.AreaViewModel == null)
        {
            return;
        }

        this.AreaViewModel.ShowMatrixControls = fileLinkAreaModel.ShowMatrixControls;

        var counter = 0;

        foreach (var linkModelList in fileLinkAreaModel.Links)
        {
            this.AreaViewModel.AddHorizontal();

            var linkViewModels = new List<LinkViewModel>();

            foreach (var linkModel in linkModelList)
            {
                var linkVM = new LinkViewModel();

                linkVM.SetElementViewModel(this.ElementViewModel);

                linkVM.InsertModel(linkModel);

                linkViewModels.Add(linkVM);
            }

            this.AreaViewModel.InsertViewModelList(linkViewModels, counter);

            counter++;
        }

        this.OnPropertyChanged(nameof(this.AreaViewModel));
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