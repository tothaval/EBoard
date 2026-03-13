// <copyright file="StandardTextViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIText;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIFont;
<<<<<<< Updated upstream
using EBoardSDK.Plugins.Shapes.TextShape;
using EBoardSDK.SharedMethods;
using EBoardSDK.ViewModels;
using System.Collections.ObjectModel;
=======
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
>>>>>>> Stashed changes
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class StandardTextViewModel : EBoardElementPluginBaseViewModel
{
    private string savePath = string.Empty;

    [ObservableProperty]
    private bool isTitleSet;

    [ObservableProperty]
    private Brush titleTextBoxBorderBrush = new SolidColorBrush();

    [ObservableProperty]
    private int titleTextBoxBorderThickness;

    [ObservableProperty]
    private Brush titleTextBoxBrush = new SolidColorBrush();

    [ObservableProperty]
    private string text = "text";

    [ObservableProperty]
    private TextAlignment textAlignmentValue = TextAlignment.Left;

    [ObservableProperty]
    private string title = "title";

    [ObservableProperty]
    private TextAlignment titleAlignmentValue = TextAlignment.Center;

    [ObservableProperty]
    private IFluidUIFontModel textFont = new FluidUIFontModel();

    [ObservableProperty]
    private IFluidUIFontModel titleFont = new FluidUIFontModel();

    private StandardTextFontSetupViewModel? textFontSetup;

    private StandardTextFontSetupViewModel? titleFontSetup;

    private string pluginHeader = "Standard Text Element";

    private string pluginName = "StandardText";

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardTextViewModel"/> class.
    /// </summary>
    public StandardTextViewModel() => this.InstantiateProperties();

    public ObservableCollection<TextAlignment> TextAlignments { get; set; } = new ObservableCollection<TextAlignment>() { TextAlignment.Left, TextAlignment.Right, TextAlignment.Center, TextAlignment.Justify };

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(StandardTextView);

    public override Type ElementPluginViewModel => typeof(StandardTextViewModel);

    public bool IsTitleTextboxEditing => !this.IsTitleSet;

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public string SavePath => this.savePath;

    public StandardTextFontSetupViewModel TextFontSetup => this.textFontSetup;

    public StandardTextFontSetupViewModel TitleFontSetup => this.titleFontSetup;

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<StandardTextModel>(path);

            if (data != null)
            {
                this.ApplyStandardTextModel(data);

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

        var model = new StandardTextModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    public override void RefreshInitialization()
    {
        if (this.TextFont == null)
        {
            this.TextFont = new FluidUIFontModel();
        }

        if (this.TitleFont == null)
        {
            this.TitleFont = new FluidUIFontModel();
        }

        this.textFontSetup = new StandardTextFontSetupViewModel(this.ElementViewModel, this, isTitle: false);
        this.titleFontSetup = new StandardTextFontSetupViewModel(this.ElementViewModel, this, isTitle: true);

        this.OnPropertyChanged(nameof(this.TextFontSetup));
        this.OnPropertyChanged(nameof(this.TitleFontSetup));
    }

    public void UpdateFont()
    {
        this.OnPropertyChanged(nameof(this.TextFont));
        this.OnPropertyChanged(nameof(this.TitleFont));
    }

    private void ApplyStandardTextModel(StandardTextModel data)
    {
        this.savePath = data.SavePath;

        this.Text = data.Text;
        this.TextFont = data.TextFont;
        this.TextAlignmentValue = data.TextAlignmentValue;

        this.Title = data.Title;
        this.TitleFont = data.TitleFont;
        this.TitleAlignmentValue = data.TitleAlignmentValue;

        this.RefreshInitialization();
    }

    private void InstantiateProperties()
    {
        this.IsTitleSet = true;

        this.OnPropertyChanged(nameof(this.TextAlignmentValue));
        this.OnPropertyChanged(nameof(this.TextAlignments));
    }

    [RelayCommand]
    private void ConfirmTitle()
    {
        this.IsTitleSet = true;

        this.TitleTextBoxBorderBrush = new SolidColorBrush(Colors.Transparent);

        this.TitleTextBoxBrush = new SolidColorBrush(Colors.Transparent);

        this.TitleTextBoxBorderThickness = 0;
    }

    [RelayCommand]
    private async void LoadStandardTextModelFromFile()
    {
        if (this.Text.Length > 0)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save the text first?", $"save this {this.Text.Length} character(s)?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                this.SaveStandardTextModelToFile();
            }
        }

        var folder = new StandardTextModel(this).SavePath;
        var helper = new SharedMethod_UI();

        var filename = await helper.GetFileToLoad(folder, $"files $(*.{helper.StandardTextFileExtension})|*.{helper.StandardTextFileExtension}", this.savePath);

        if (filename != null && Loader.FileExists(filename))
        {
            var data = await Loader.LoadJsonFile<StandardTextModel>(filename);

            if (data != null)
            {
                this.ApplyStandardTextModel(data);
            }

            var path = new FileInfo(filename).Directory;

            if (path != null && path.Exists)
            {
                this.savePath = path.FullName;
            }
        }
    }

    [RelayCommand]
    private void MakeTextShape()
    {
        var manager = new FluidUIDeepCopyManager();
        var copy = manager.DeepCopyIFluidUIContext(this.ElementViewModel.FluidUI);

        var elementViewModel = new ElementViewModel(this.EBoardViewModel);
        var textshape = new TextShapeViewModel($"{this.Title}\n{this.Text}");
        textshape.SetEBoardAndElementViewModel(this.EBoardViewModel, elementViewModel);
        textshape.SetFluidUIContext(copy);
        textshape.RefreshInitialization();

        elementViewModel.Plugin = textshape;

        new SDKPluginManager().InvokeElementViewModelOnEboard(elementViewModel);
    }

    [RelayCommand]
    private async void SaveStandardTextModelToFile()
    {
        var model = new StandardTextModel(this);
        var helper = new SharedMethod_UI();

        var path = await new SharedMethod_UI().SetSaveFileName(helper.StandardTextFileExtension, this.savePath);

        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        _ = Saver.SaveJsonFile(path, model);

        var dir = new FileInfo(path).Directory;

        if (dir != null)
        {
            this.savePath = dir.FullName;
        }
    }

    [RelayCommand]
    private void SetTitle()
    {
        if (this.ElementViewModel.FluidUI.Design != null)
        {
            this.TitleTextBoxBorderBrush = this.ElementViewModel.FluidUI.Design.Highlight;
            this.TitleTextBoxBrush = this.ElementViewModel.FluidUI.Design.Background;
        }

        this.IsTitleSet = false;
        this.TitleTextBoxBorderThickness = 2;
    }
}

// EOF