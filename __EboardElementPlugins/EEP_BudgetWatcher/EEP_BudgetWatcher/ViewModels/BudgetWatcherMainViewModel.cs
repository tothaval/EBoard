// <copyright file="BudgetWatcherMainViewModel.cs" company=".">
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
namespace EEP_BudgetWatcher.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Eboard.Summoner;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EEP_BudgetWatcher.Models;
using EEP_BudgetWatcher.Resources;
using Serilog;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class BudgetWatcherMainViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "BudgetWatcher Element";
    private readonly string pluginName = "BudgetWatcher";

    private readonly BudgetChangeViewModel _BudgetChangeViewModel;

    [ObservableProperty]
    private bool _ShowBudget;

    [ObservableProperty]
    private bool _ShowBudgetOverview;

    [ObservableProperty]
    private bool _ShowNotes;

    [ObservableProperty]
    private bool _ShowSetup;

    public BudgetWatcherMainViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.OnlyFluidUI);

        SetInitialResources();

        _BudgetChangeViewModel = new BudgetChangeViewModel();

        SetupField = new SetupFieldViewModel();
    }

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Addon;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EEP_BudgetWatcher;component/Themes/Default.xaml", uriKind: UriKind.Relative) };

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(BudgetOverviewModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(BudgetWatcherMainViewModel);

    public BudgetChangeViewModel BudgetChangeViewModel => _BudgetChangeViewModel;

    public SetupFieldViewModel SetupField { get; }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            string contentfilename = "content.xml";

            path = Path.Combine(path, contentfilename);
        }

        try
        {
            var data = await new SDKDataManager().LoadPluginContent<BudgetOverviewModel>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(BudgetOverviewModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        List<Budget> budgets = [];

        BudgetChangeViewModel.Budgets.ToList().ForEach(budgetviewmodel =>
        {
            budgets.Add(budgetviewmodel.GetBudget);
        });

        var model = new BudgetOverviewModel(budgets);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is BudgetOverviewModel budgetOverviewModel)
        {
            this.ApplyModel(budgetOverviewModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<BudgetOverviewModel>(json!);

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
        List<Budget> budgets = [];

        BudgetChangeViewModel.Budgets.ToList().ForEach(budgetviewmodel =>
        {
            budgets.Add(budgetviewmodel.GetBudget);
        });

        var model = new BudgetOverviewModel(budgets);

        this.SetModel(model);
    }

    private void ApplyModel(BudgetOverviewModel budgetOverviewModel)
    {
        BudgetChangeViewModel.ApplyData(budgetOverviewModel);
    }

    private void SetInitialResources()
    {
        new ResourceSet().SetResources();
    }

    [RelayCommand]
    private void AddBudget()
    {
        BudgetChangeViewModel.AddBudget(new ViewLess.BudgetViewModel(new Budget()));
    }

    [RelayCommand]
    private void RemoveBudget()
    {
        BudgetChangeViewModel.RemoveBudget(BudgetChangeViewModel.BudgetViewModel);
    }
}

// EOF