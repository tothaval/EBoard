// <copyright file="SummonerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Tools.Summoner;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.SharedMethods;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
/// TODO
///
/// hm okay, die Instanzierung erfolgt hier über hardgecodete pfade usw. hat auch vorteile.
/// müsste irgendwie per reflection oder indem ich dem plugin das elementviewmodel mitgebe
/// auf die mainwindow sache zugreifen, da könnte ich den string gegen die liste aller plugins
/// prüfen und dann das entsprechende auswählen.
/// .
/// bei einem hardgecodeten summoner müsste ich mich auf sdk plugins beschränken, da ich nicht
/// direkt wissen kann, wie die anderen namespaces und assemblies konkret benannt sind.
/// .
/// alternativ müsste ich aus den plugins eine datei erzeugen und aus dieser lesen, den rest
/// müsste WPF über die ResourceDictionaries hinbekommen.
/// .
/// ist vielleicht der am schnellsten umsetzbare ansatz direkt, mit allem was da ist.
/// 
/// leider hatte ich bisher keinen Erfolg damit, die benötigte Instanz zu erhalten.
/// Grund ist die abstrakte oder was auch immer Natur der Application Klasse.
/// 
/// also, elementviewmodel oder datei, was werde ich tun?
/// </summary>
public partial class SummonerViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private string userCommandString = ">";

    private List<EBoardElementPluginBaseViewModel> knownPlugins;

    partial void OnUserCommandStringChanged(string value)
    {
    }

    private double Angle { get; set; }

    [ObservableProperty]
    private IPlugin summonee;

    // public PluginDataSet PluginDataSet { get; set; } = new PluginDataSet();
    [ObservableProperty]
    private string imagePath;

    partial void OnImagePathChanged(string value)
    {
        ChangeElementBackgroundToImage(BrushTargets.Background, value);
    }

    [ObservableProperty]
    private int rotationAngleValue;

    partial void OnRotationAngleValueChanging(int oldValue, int newValue)
    {
        if (oldValue != newValue)
        {
            UpdateRotation(newValue);
        }
    }

    [ObservableProperty]
    private RotateTransform rotateTransformValue;

    partial void OnRotateTransformValueChanged(RotateTransform value)
    {
        Angle = RotationAngleValue;
    }

    [ObservableProperty]
    private Point transformOriginPoint;

    [ObservableProperty]
    private int cornerRadiusValueSummonee;

    partial void OnCornerRadiusValueSummoneeChanged(int value)
    {
        UpdateCornerRadius(value);
    }

    [ObservableProperty]
    private int heightValue;

    partial void OnHeightValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Summonee is not null)
        {
            // Summonee.Height = value;
        }

        UpdateContentHeight(value);
    }

    [ObservableProperty]
    private int widthValue;

    partial void OnWidthValueChanged(int value)
    {
        TransformOriginPoint = new Point(0, 0);

        if (Summonee is not null)
        {
            // Summonee.Width = value;
        }

        UpdateContentWidth(value);
    }

    /// <summary>
    /// Gets or sets until a real fluidUIDesign architecture is implemented, this serves as a mockup solution
    ///
    /// a real fluidUIDesign architecture should check a folder for certain files or a file, in which
    /// data on plugins is stored, then all that data needs to be loaded and instanciated if
    /// necessary during app start
    ///
    /// the ui should build menuitems dynamically from that data
    /// </summary>
    public List<string> PluginsCategoryElements { get; set; } = ["StandardText"];

    /// <summary>
    /// Gets or sets until a real fluidUIDesign architecture is implemented, this serves as a mockup solution
    /// </summary>
    public List<string> PluginsCategoryShapes { get; set; } = ["Ellipse", "Rectangle"];

    /// <summary>
    /// Gets or sets until a real fluidUIDesign architecture is implemented, this serves as a mockup solution
    /// </summary>
    public List<string> PluginsCategoryTools { get; set; } = [
        "SessionUptimeClock", "EmptyLinear", "EmptyRadial", "Summoner", "Uptime"];

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    private string pluginHeader = "Summoner Element";

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

    private string pluginName = "Summoner";

    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override string ElementPluginName => "Summoner";

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(SummonerView);

    public override Type ElementPluginViewModel => typeof(SummonerViewModel);

    public SummonerViewModel() => this.InstantiateProperties();

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path)
    {
        if (this.ImagePath != null && this.ImagePath != string.Empty)
        {
            this.Summonee?.ElementViewModel.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(new SharedMethod_UI().ChangeBackgroundToImage(this.Summonee.ElementViewModel.FluidUI.Design.Background, this.ImagePath), Enums.BrushTargets.Background);
        }
    }

    [RelayCommand]
    private void DeleteElement(object s)
    {
        this.Summonee = null;
    }

    /// <summary>
    /// until a real command architecture is implemented, this serves as a mockup solution
    ///
    /// a real command architecture should be done in a separate class or using an api that
    /// handles all validations etc.
    ///
    /// validation could also use onerrorinfo with community toolkit, but i need to look into that first
    /// </summary>
    [RelayCommand]
    private void ExecuteCommandString()
    {
        // this.Width = double.NaN;
        // this.Height = double.NaN;
        string command = this.UserCommandString.Substring(1);

        if (this.UserCommandString.StartsWith(">"))
        {
            var eboardscreen = this.EBoardViewModel;

            if (eboardscreen != null)
            {
                this.knownPlugins = eboardscreen.GetPlugins().ToList();
            }

            if (this.knownPlugins != null && this.knownPlugins.Count > 0)
            {
                var pluginBaseViewModel = this.knownPlugins.Where(x => x.PluginName.Equals(command)).FirstOrDefault();

                if (pluginBaseViewModel != null)
                {
                    var plugin = Activator.CreateInstance(pluginBaseViewModel.ElementPluginViewModel) as IPlugin;

                    if (plugin != null)
                    {
                        plugin.SetEBoardAndElementViewModel(this.EBoardViewModel, this.ElementViewModel);

                        this.Summonee = plugin;

                        // resource dictionary muss ggf. noch genutzt werden wegen datatemplates
                        return;
                    }
                }
            }

            this.Summonee = new StandardTextViewModel() { Text = $"unknown fluidUIDesign '{command}' called" };
            return;
        }

        this.Summonee = new StandardTextViewModel() { Text = $"unrecognized command" };
    }

    private void InstantiateProperties()
    {
    }

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        // this.PluginDataSet.References.Add(new("Type", this.Summonee?.Plugin?.GetType().FullName));
        // this.PluginDataSet.References.Add(new("Name", this.Summonee?.PluginName));
        // this.PluginDataSet.References.Add(new("Header", this.Summonee?.PluginHeader));

        // string contentDataPath = Path.Combine(path, linkDataFileName);

        // var model = new LinkModel() { LinkTargetName = this.LinkTargetName, LinkTargetPath = this.LinkTargetPath };

        // var serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(model, contentDataPath);

        // if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
        // {
        //    // TODO do stuff
        // }

        // return serializationResult;
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    [RelayCommand]
    private void ResetImage()
    {
        this.ImagePath = string.Empty;

        this.Summonee.ElementViewModel.FluidUIMenuViewModel.FluidUIDesignSetupViewModel.Apply_FluidUIDesignBrush(new SharedMethod_UI().ImagePathErrorDefaultBrush, Enums.BrushTargets.Background);
    }

    [RelayCommand]
    private void SetImage()
    {
        this.ImagePath = new SharedMethod_UI().UserSelectImage(this.ImagePath);
    }

    private void UpdateContentHeight(int height)
    {
        if (this.Summonee is not null)
        {
            // this.Summonee.Height = height;
        }
    }

    private void UpdateContentWidth(int width)
    {
        if (this.Summonee is not null)
        {
            // this.Summonee.Width = width;
        }
    }

    private void UpdateCornerRadius(int value)
    {
        if (this.Summonee is not null)
        {
            // TODO update implementation
            // Summonee.BorderManagement.CornerRadiusValue = value;
        }
    }

    private void UpdateRotation(int rotationAngle)
    {
        this.RotateTransformValue = new RotateTransform(rotationAngle * -1);

        this.TransformOriginPoint = new Point(0.5, 0.5);

        // this.Width = double.NaN;
    }
}

// EOF