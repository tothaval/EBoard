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
using EBoardSDK.Models;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Plugins.Tools.Coordinates;
using EBoardSDK.ViewModels;
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
/// also, elementviewmodel oder datei, was werde ich tun?.
/// </summary>
public partial class SummonerViewModel : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private bool pluginTypeSelected = false;

    [ObservableProperty]
    private PluginRepresentationItem? selectedPlugin = null;

    [ObservableProperty]
    private string userCommandString = ">";

    [ObservableProperty]
    private IPlugin? summonee;

    [ObservableProperty]
    private List<PluginRepresentationItem> plugins = [];

    private string pluginHeader = "Summoner Element";
    private string pluginName = "Summoner";

    /// <summary>
    /// Initializes a new instance of the <see cref="SummonerViewModel"/> class.
    /// </summary>
    public SummonerViewModel()
    {
    }

    public override bool NoDefaultBorders { get; } = false;

    public override PluginCategories PluginCategory => PluginCategories.Tool;

    public override ImageBrush PluginLogo { get; set; }

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

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

    public override Type ElementPluginView => typeof(SummonerView);

    public override Type ElementPluginViewModel => typeof(SummonerViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    public override void RefreshInitialization()
    {
        var pluginManager = new SDKPluginManager();

        this.Plugins = pluginManager.FoundPlugins;
    }

    protected async virtual void ExecuteCommandAsync()
    {
        string command = this.UserCommandString.Substring(1);

        if (this.UserCommandString.StartsWith(">"))
        {
            var plugin = await new SDKPluginManager().InvokePluginByName(command);

            if (plugin == null)
            {
                this.Summonee = new StandardTextViewModel() { Title = "plugin not found", Text = $"unknown '{command}' called" };
                return;
            }

            var elementViewModel = new ElementViewModel(this.EBoardViewModel);

            var fluiduicopy = new FluidUIDeepCopyManager().DeepCopyIFluidUIContext(this.ElementViewModel.FluidUI);
            elementViewModel.SetFluidUI(fluiduicopy);

            plugin.SetEBoardAndElementViewModel(this.EBoardViewModel, elementViewModel);
            plugin.RefreshInitialization();

            this.Summonee = plugin;
            this.PluginTypeSelected = true;
        }
    }

    partial void OnSelectedPluginChanged(PluginRepresentationItem? value)
    {
        this.UserCommandString = $">{value?.PluginName}";

        this.ExecuteCommandString();
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
    /// validation could also use onerrorinfo with community toolkit, but i need to look into that first.
    /// </summary>
    [RelayCommand]
    private void ExecuteCommandString()
    {
        this.ExecuteCommandAsync();
    }

    [RelayCommand]
    private void ResetPluginType()
    {
        this.PluginTypeSelected = false;
    }
}

// EOF