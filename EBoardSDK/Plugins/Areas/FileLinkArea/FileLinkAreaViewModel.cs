// <copyright file="FileLinkAreaViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Areas.FileLinkArea;

using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Controls.Area;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.SharedMethods;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class FileLinkAreaViewModel : EBoardElementPluginBaseViewModel
{
    public AreaViewModel<LinkViewModel> AreaViewModel { get; }

    public override PluginCategories PluginCategory => PluginCategories.Area;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "FileLink Area";

    public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

    private string pluginName = "FileLinkArea";

    public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

    public override string ElementPluginName => "FileLinkArea"; // mal konsolidieren, was benötigt wird und was doppelt ist.

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(FileLinkAreaView);

    public override Type ElementPluginViewModel => typeof(FileLinkAreaViewModel);

    public FileLinkAreaViewModel()
    {
        this.AreaViewModel = new AreaViewModel<LinkViewModel>();
    }

    [RelayCommand]
    private void Reset()
    {
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<FileLinkAreaModel>(path);

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

                        linkVM.InsertLinkModel(linkModel);

                        linkViewModels.Add(linkVM);
                    }

                    this.AreaViewModel.InsertViewModelList(linkViewModels, counter);

                    counter++;
                }

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

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
    }
}