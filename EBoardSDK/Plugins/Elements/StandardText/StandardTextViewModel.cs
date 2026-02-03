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
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces.FluidUIFont;
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIFont;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.Protocol.Models;
using EBoardSDK.SharedMethods;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using Serilog;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

public partial class StandardTextViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Standard Text Element";
    private readonly string pluginName = "StandardText";

    private string savePath = string.Empty;

    [ObservableProperty]
    private bool isTitleSet;

    public bool CanShowText => (!this.ShowOnlyText && !this.ShowOnlyTitle) || this.ShowOnlyText;

    public bool CanShowTitle => (!this.ShowOnlyText && !this.ShowOnlyTitle) || this.ShowOnlyTitle;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanShowText))]
    [NotifyPropertyChangedFor(nameof(CanShowTitle))]
    private bool showOnlyText = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanShowText))]
    [NotifyPropertyChangedFor(nameof(CanShowTitle))]
    private bool showOnlyTitle = false;

    [ObservableProperty]
    private VerticalAlignment textVerticalAlignment = VerticalAlignment.Top;

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

    private StandardTextFontSetupViewModel? textFontSetup;

    [ObservableProperty]
    private string title = "title";

    [ObservableProperty]
    private TextAlignment titleAlignmentValue = TextAlignment.Center;

    [ObservableProperty]
    private IFluidUIFontModel titleFont = new FluidUIFontModel();

    private StandardTextFontSetupViewModel? titleFontSetup;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandardTextViewModel"/> class.
    /// </summary>
    public StandardTextViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);

        this.IsTitleSet = true;

        this.SetMenuItemViewModel(new StandardTextMenuItemViewModel(this));
        this.SetMenuItem(new StandardTextMenuItem(this.MenuItemViewModel!));

        this.OnPropertyChanged(nameof(this.TextAlignmentValue));
    }

    public bool IsTitleTextboxEditing => !this.IsTitleSet;

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Assembly? PluginAssembly => Assembly.GetAssembly(this.PluginViewModelType);

    /// <inheritdoc/>
    public override ResourceDictionary ResourceDictionary => new();

    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(StandardTextModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(StandardTextViewModel);

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

    /// <inheritdoc/>
    public override ImageBrush Logo { get; set; } = new();

    public string SavePath => this.savePath;

    public StandardTextFontSetupViewModel TextFontSetup => this.textFontSetup;

    public StandardTextFontSetupViewModel TitleFontSetup => this.titleFontSetup;

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<StandardTextModel>(path);

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

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(StandardTextModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new StandardTextModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is StandardTextModel standardTextModel)
        {
            this.ApplyModel(standardTextModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<StandardTextModel>(json!);

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
        var model = new StandardTextModel(this);

        this.SetModel(model);
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.TitleFont == null)
        {
            this.TitleFont = new FluidUIFontModel();
        }

        if (this.ElementViewModel != null && this.ElementViewModel.FluidUI.Font != null)
        {
            this.textFontSetup = new StandardTextFontSetupViewModel(this.ElementViewModel, this, this.ElementViewModel.FluidUI.Font);
            this.titleFontSetup = new StandardTextFontSetupViewModel(this.ElementViewModel, this, this.TitleFont);
        }

        this.OnPropertyChanged(nameof(this.TextFontSetup));
        this.OnPropertyChanged(nameof(this.TitleFontSetup));
    }

    public void UpdateFont()
    {
        this.ElementViewModel?.UpdateFont();
        this.OnPropertyChanged(nameof(this.TitleFont));
    }

    internal async void LoadStandardTextModelFromFile()
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
                this.ApplyModel(data);
            }

            var path = new FileInfo(filename).Directory;

            if (path != null && path.Exists)
            {
                this.savePath = path.FullName;
            }
        }
    }

    internal async void SaveStandardTextModelToFile()
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

    private void ApplyModel(StandardTextModel data)
    {
        this.savePath = data.SavePath;

        this.ShowOnlyText = data.ShowOnlyText;
        this.ShowOnlyTitle = data.ShowOnlyTitle;

        this.Text = data.Text;
        this.TextAlignmentValue = data.TextAlignmentValue;
        this.TextVerticalAlignment = data.TextVerticalAlignment;

        this.Title = data.Title;
        this.TitleFont = data.TitleFont;
        this.TitleAlignmentValue = data.TitleAlignmentValue;

        this.RefreshInitialization();
    }

    partial void OnShowOnlyTextChanged(bool value)
    {
        if (this.ShowOnlyTitle && value)
        {
            this.ShowOnlyTitle = false;
        }
    }

    partial void OnShowOnlyTitleChanged(bool value)
    {
        if (this.ShowOnlyText && value)
        {
            this.ShowOnlyText = false;
        }
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
    private void SetTitle()
    {
        if (this.ElementViewModel != null && this.ElementViewModel.FluidUI.Design != null)
        {
            this.TitleTextBoxBorderBrush = this.ElementViewModel.FluidUI.Design.Highlight;
            this.TitleTextBoxBrush = this.ElementViewModel.FluidUI.Design.Background;
        }

        this.IsTitleSet = false;
        this.TitleTextBoxBorderThickness = 2;
    }
}

// EOF