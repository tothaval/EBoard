// <copyright file="KrakenMainViewModel.cs" company=".">
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
namespace Kraken;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Utilities.Factories;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;

public partial class KrakenMainViewModel : PluginBaseViewModel
{
    private readonly string pluginHeader = "Kraken Element";
    private readonly string pluginName = "Kraken";

    [ObservableProperty]
    private string eboardViewModelTitle = string.Empty;

    [ObservableProperty]
    private string eboardViewModelData = string.Empty;

    public KrakenMainViewModel()
    {
        this.ScreenInstantiationConstraints = new InstantiationAndCopyConstraints(
            elementInstantiationPolicy: InstantiationPolicy.Unconstrained,
            copyConstraints: CopyConstraints.FullCopy);
    }

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
    public override ResourceDictionary ResourceDictionary => new() { Source = new Uri("/Kraken;component/KrakenResource.xaml", uriKind: UriKind.Relative) };

    /// <inheritdoc/>
    public override Type? PluginModelType => null;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(KrakenMainViewModel);

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        base.RefreshInitialization();

        if (this.ElementViewModel != null)
        {
            var screen = this.ElementViewModel.ScreenViewModel;

            if (screen != null && screen.FluidUI.DataBlock != null)
            {
                this.EboardViewModelTitle = screen.FluidUI.DataBlock.Title;

                var db = screen.FluidUI.DataBlock;

                StringBuilder stringBuilder = new ();

                foreach (var item in screen.Elements)
                {
                    stringBuilder.Append($"{item.Plugin?.Name}<:::<{item.FluidUI.DataBlock?.Title}\n{item.FluidUI.DataBlock?.Text}\n\n");
                }

                this.EboardViewModelData = $"{db.Text}\n\n{db.IndexTextQuadValue1.Value1}\n\n" +
                    $"{db.KeyTextQuadValueA.Value1}\n\n{db.IndexTextList.Count}\n\n{db.KeyTextList.Count}\n\n" +
                    $"{screen.EBID}\n\n" +
                    $"{screen.Elements.Count}" +
                    $"{stringBuilder}";
            }
        }
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Load(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Load(string path)"));
    }

    /// <inheritdoc/>
    public override Task<EboardFeedbackMessage> Save(string path)
    {
        return Task.FromResult(FeedbackMessageFactory.EmptyCallSuccess("Save(string path)"));
    }
}

// EOF