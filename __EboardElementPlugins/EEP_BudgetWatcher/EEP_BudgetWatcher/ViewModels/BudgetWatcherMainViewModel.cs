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
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Plugins;
<<<<<<< Updated upstream
=======
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using EEP_BudgetWatcher.Models;
using EEP_BudgetWatcher.Resources;
using EEP_BudgetWatcher.Views;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class BudgetWatcherMainViewModel : EBoardElementPluginBaseViewModel
{
    private readonly BudgetChangeViewModel _BudgetChangeViewModel;

    [ObservableProperty]
    private bool _ShowBudget;

    [ObservableProperty]
    private bool _ShowBudgetOverview;

    [ObservableProperty]
    private bool _ShowNotes;

    [ObservableProperty]
    private bool _ShowSetup;

    private string pluginHeader = "BudgetWatcher Element";
    private string pluginName = "BudgetWatcher";

    public BudgetWatcherMainViewModel()
    {
        this.ElementScreenIntegrationConstraints = new EBoardSDK.Models.ElementScreenIntegrationConstraints(ElementInstantiationPolicy.Unconstrained);

        SetInitialResources();

        _BudgetChangeViewModel = new BudgetChangeViewModel();

        SetupField = new SetupFieldViewModel();
    }

    public override PluginCategories PluginCategory => PluginCategories.Addon;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

    public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

    public override string PluginName { get { return pluginName; } set { pluginName = value; } }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/EEP_BudgetWatcher;component/Themes/Default.xaml", uriKind: UriKind.Relative) };

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(BudgetWatcherMainView);

    public override Type ElementPluginViewModel => typeof(BudgetWatcherMainViewModel);

    public BudgetChangeViewModel BudgetChangeViewModel => _BudgetChangeViewModel;

    public SetupFieldViewModel SetupField { get; }

    public async override Task<EBoardFeedbackMessage> Load(string path)
    {
        if (new DirectoryInfo(path).Exists)
        {
            string contentfilename = "content.xml";

            path = Path.Combine(path, contentfilename);
        }

        try
        {
            var data = await Loader.LoadJsonFile<BudgetOverviewModel>(path)!;

            if (data != null)
            {
                BudgetChangeViewModel.ApplyData(data);

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

        List<Budget> budgets = [];

        BudgetChangeViewModel.Budgets.ToList().ForEach(budgetviewmodel =>
        {
            budgets.Add(budgetviewmodel.GetBudget);
        });

        var model = new BudgetOverviewModel(budgets);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
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