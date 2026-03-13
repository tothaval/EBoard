// <copyright file="EllipseViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Shapes.Ellipse;

using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins.Shapes.TextShape;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using EBoardSDK.ViewModels;
using Serilog;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public partial class EllipseViewModel : ShapeBaseViewModel
{
    private string pluginHeader = "Ellipse Shape";

    private string pluginName = "Ellipse";

    /// <summary>
    /// Initializes a new instance of the <see cref="EllipseViewModel"/> class.
    /// </summary>
    public EllipseViewModel()
    {
<<<<<<< Updated upstream
=======
        //this.SetMenuItemViewModel(new EllipseMenuItemViewModel(this));
        //this.SetMenuItem(new EllipseMenuItem(this.MenuItemViewModel!));
>>>>>>> Stashed changes
    }

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

<<<<<<< Updated upstream
    public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

    public override ResourceDictionary ResourceDictionary => new();

    public override Type? ElementPluginModel => null;

    public override Type ElementPluginView => typeof(EllipseView);

    public override Type ElementPluginViewModel => typeof(EllipseViewModel);
=======
    /// <inheritdoc/>
    public override Type? PluginModelType => typeof(ShapeModel);

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(EllipseViewModel);

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<ShapeModel>(path);

            if (data != null)
            {
                this.ApplyShapeModel(data);

                this.RefreshInitialization();

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(TextShapeModel).FullName}");
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new ShapeModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    /// <inheritdoc/>
    public override void InsertModel<T>(T model)
    {
        if (model == null)
        {
            return;
        }

        if (model is ShapeModel shapeModel)
        {
            this.ApplyShapeModel(shapeModel);

            return;
        }

        try
        {
            var json = model.ToString();

            var jsonParsed = JsonSerializer.Deserialize<ShapeModel>(json!);

            if (jsonParsed != null)
            {
                this.ApplyShapeModel(jsonParsed);
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
        var model = new ShapeModel(this);

        this.SetModel(model);
    }
>>>>>>> Stashed changes
}

// EOF