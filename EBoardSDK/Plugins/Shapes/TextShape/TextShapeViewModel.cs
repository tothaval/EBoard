// <copyright file="TextShapeViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Shapes.TextShape;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardConfigManager.Enums;
using EBoardConfigManager.Helper;
using EBoardSDK.Enums;
using EBoardSDK.ViewModels;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class TextShapeViewModel : ShapeBaseViewModel
{
    private string pluginHeader = "Text Shape Element";

    private string pluginName = "TextShape";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ResetText))]
    private bool textEntered = false;

    [ObservableProperty]
    private string textString = "enter text";

    [ObservableProperty]
    private double scaleX = 1.0;

    [ObservableProperty]
    private double scaleY = 1.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeViewModel"/> class.
    /// </summary>
    public TextShapeViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TextShapeViewModel"/> class.
    /// </summary>
    /// <param name="text"></param>
    public TextShapeViewModel(string text)
    {
        this.TextString = text;
        this.TextEntered = true;
    }

    public bool ResetText => !this.TextEntered;

    public override PluginCategories PluginCategory => PluginCategories.Shape;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    public override bool NoDefaultBorders { get; } = true;

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(TextShapeView);

    public override Type ElementPluginViewModel => typeof(TextShapeViewModel);

    public override async Task<EBoardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await Loader.LoadJsonFile<TextShapeModel>(path);

            if (data != null)
            {
                var fluidUIViewModel = new EboardFluidUIBaseViewModel();
                data.FluidUIContext?.Design?.LoadBrushesFromColorData();
                fluidUIViewModel.SetFluidUI(data.FluidUIContext);

                this.TextString = data.TextString;
                this.TextEntered = data.TextEntered;
                this.ScaleX = data.ScaleX;
                this.ScaleY = data.ScaleY;

                this.SetFluidUIViewModel(fluidUIViewModel);

                this.RefreshInitialization();

                this.OnPropertyChanged(nameof(this.TextEntered));
                this.OnPropertyChanged(nameof(this.TextString));
                this.OnPropertyChanged(nameof(this.ScaleX));
                this.OnPropertyChanged(nameof(this.ScaleY));

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

        var model = new TextShapeModel(this);

        var result = Saver.SaveJsonFile(path, model);

        return new EBoardFeedbackMessage()
        {
            ResultMessage = $"{path} :: saving eboard config: {result}",
            TaskResult = result.Equals(Result.Success) ? EBoardTaskResult.Success : EBoardTaskResult.Unknown,
        };
    }

    [RelayCommand]
    private void CreateTextShape()
    {
        this.TextEntered = true;
    }

    [RelayCommand]
    private void EditText()
    {
        this.TextEntered = false;
    }
}

// EOF