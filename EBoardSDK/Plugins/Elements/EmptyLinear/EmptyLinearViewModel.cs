// <copyright file="EmptyLinearViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Elements.EmptyLinear;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Controls.BrushSetup.LinearBrushSetup;
using EBoardSDK.Enums;
<<<<<<< Updated upstream
=======
using EBoardSDK.Models;
using EBoardSDK.Models.FluidUIDesign;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
>>>>>>> Stashed changes
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class EmptyLinearViewModel : EBoardElementPluginBaseViewModel
{
<<<<<<< Updated upstream
=======
    private readonly string pluginName = "EmptyLinear";
    private readonly string pluginHeader = "Empty Linear";

    private LinearBrushSetupViewModel linearBrushViewModel;

>>>>>>> Stashed changes
    [ObservableProperty]
    private string content = "\t\t\t\n\n\n";

    [ObservableProperty]
<<<<<<< Updated upstream
    private LinearGradientBrush background = new LinearGradientBrush(
=======
    private LinearGradientBrush background = new(
>>>>>>> Stashed changes
        [new GradientStop(Colors.AliceBlue, 0.0), new GradientStop(Colors.Navy, 0.5)],
        new Point(0, 0),
        new Point(0.5, 1));

    private string pluginName = "EmptyLinear";

    private string pluginHeader = "Empty Linear";

    /// <summary>
    /// Initializes a new instance of the <see cref="EmptyLinearViewModel"/> class.
    /// </summary>
    public EmptyLinearViewModel() => this.InstantiateProperties();

    public override PluginCategories PluginCategory => PluginCategories.Element;

    public override bool NoDefaultBorders { get; } = false;

    public override ImageBrush PluginLogo { get; set; } = new();

    public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

    public override string PluginHeader
    {
        get { return this.pluginHeader; }
        set { this.pluginHeader = value; }
    }

<<<<<<< Updated upstream
    public override string PluginName
    {
        get { return this.pluginName; }
        set { this.pluginName = value; }
    }

    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EmptyLinearView);

    public override Type ElementPluginViewModel => typeof(EmptyLinearViewModel);

    public override Task<EBoardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty load call" });
    }

    public override Task<EBoardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = "empty save call" });
    }

    private void InstantiateProperties()
    {
=======
    public LinearBrushSetupViewModel LinearBrush => this.linearBrushViewModel;

    /// <inheritdoc/>
    public override PluginCategories Category => PluginCategories.Element;

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
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(EmptyLinearViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        base.RefreshInitialization();

        if (this.ElementViewModel != null && this.LinearBrush == null)
        {
            this.linearBrushViewModel = new LinearBrushSetupViewModel(this.ElementViewModel, BrushTargets.Background, okAction: this.SetBackground);

            this.LinearBrush?.SetBrush(this.Background);
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<FluidUIBrushModel>(path);

            if (data != null)
            {
                var manager = new FluidUIBrushManager(data);
                var brush = await manager.GetBrush();

                if (this.ElementViewModel != null)
                {
                    this.linearBrushViewModel = new LinearBrushSetupViewModel(this.ElementViewModel, BrushTargets.Background, okAction: this.SetBackground);
                }

                if (brush != null && brush.GetType().Equals(typeof(LinearGradientBrush)))
                {
                    this.LinearBrush.SetBrush(brush);

                    this.Background = this.LinearBrush.Brush;
                    this.OnPropertyChanged(nameof(this.LinearBrush));

                    return FeedbackMessageFactory.Success($"deserialized {path}");
                }
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
        var model = new FluidUIBrushModel();

        model.BrushTypeName = "LinearGradientBrush";
        model.BrushTypeEnum = BrushTypes.LinearGradientBrush;

        model.GradientPoints.Add(new System.Windows.Point(this.LinearBrush.StartPoint.X, this.LinearBrush.StartPoint.Y));
        model.GradientPoints.Add(new System.Windows.Point(this.LinearBrush.EndPoint.X, this.LinearBrush.EndPoint.Y));

        foreach ((Color, double) item in this.LinearBrush.GsList)
        {
            model.GradientColors.Add(item.Item1);
            model.GradientStops.Add(item.Item2);
        }

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    private void SetBackground()
    {
        this.Background = this.LinearBrush.Brush;
>>>>>>> Stashed changes
    }
}

// EOF