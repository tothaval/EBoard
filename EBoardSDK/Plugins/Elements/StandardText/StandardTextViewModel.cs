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
using EBoardSDK.Enums;
using EBoardSDK.SharedMethods;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class StandardTextViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private int fontSize;

    [ObservableProperty]
    private int fontSizeTitle;

    [ObservableProperty]
    private bool isTitleSet;

    [ObservableProperty]
    private string text;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private Brush titleTextBoxBorderBrush;

    [ObservableProperty]
    private int titleTextBoxBorderThickness;

    [ObservableProperty]
    private Brush titleTextBoxBrush;

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Standard Text Element";

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    private string pluginName = "StandardText";

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override string ElementPluginName => "StandardText";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(StandardTextView);

    public override Type ElementPluginViewModel => typeof(StandardTextViewModel);

    public bool IsTitleTextboxEditing => !this.IsTitleSet;

    public StandardTextViewModel() => this.InstantiateProperties();

    private void InstantiateProperties()
    {
        this.IsTitleSet = true;

        //this.BorderManagement = new BorderManagement();
        //this.BrushManagement = new BrushManagement();

        if (this.PluginHeader.Equals(string.Empty))
        {
            this.PluginHeader = "Standard Text";
        }

        if (string.IsNullOrWhiteSpace(this.Title))
        {
            this.Title = this.PluginHeader;
        }
    }

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SharedMethod_Plugins().DeserializeConfigFiles<StandardTextModel>(path)!;

            if (data != null)
            {
                this.FontSize = data.FontSize;
                this.FontSizeTitle = data.FontSizeTitle;
                this.Text = data.Text;
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

        var model = new StandardTextModel(this);

        serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, path);

        if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        {
            // TODO do stuff
        }

        return serializationResult!;
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
        this.IsTitleSet = false;

        this.TitleTextBoxBorderBrush = this.ElementViewModel.FluidUI.Design.Foreground;
        this.TitleTextBoxBrush = this.ElementViewModel.FluidUI.Design.Highlight;

        this.TitleTextBoxBorderThickness = 2;
    }
}

// EOF