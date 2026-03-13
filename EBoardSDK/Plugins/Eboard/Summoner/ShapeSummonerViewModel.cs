// <copyright file="ShapeSummonerViewModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Eboard.Summoner;

using EBoardSDK;
using EBoardSDK.Controls.FluidUIMenu;
using EBoardSDK.Plugins;
using EBoardSDK.Utilities;
using EBoardSDK.Utilities.Factories;
using System;
using System.Threading.Tasks;

public partial class ShapeSummonerViewModel : SummonerViewModel
{
    private readonly string pluginHeader = "Shape Summoner Element";
    private readonly string pluginName = "ShapeSummoner";

    /// <summary>
    /// Initializes a new instance of the <see cref="ShapeSummonerViewModel"/> class.
    /// </summary>
    public ShapeSummonerViewModel()
    {
    }

    /// <inheritdoc/>
    public override string Header => this.pluginHeader;

    /// <inheritdoc/>
    public override string Name => this.pluginName;

    /// <inheritdoc/>
    public override Type PluginViewModelType => typeof(ShapeSummonerViewModel);

    /// <inheritdoc/>
    public override void Dispose()
    {
        base.Dispose();

        if (this.PluginSelectionViewModel != null)
        {
            this.PluginSelectionViewModel.PropertyChanged -= this.PluginSelectionViewModel_PropertyChanged;
        }
    }

    /// <inheritdoc/>
    public override void RefreshInitialization()
    {
        if (this.ElementViewModel != null)
        {
            var pluginManager = new SDKPluginManager();

            if (this.PluginSelectionViewModel == null)
            {
                this.SetPluginSelectionViewModel(new PluginSelectionViewModel(this.FluidUI, null, singleSelectionTarget: true));
            }

            this.Plugins = pluginManager.FoundShapes;

            if (this.PluginSelectionViewModel != null)
            {
                this.PluginSelectionViewModel.PropertyChanged += this.PluginSelectionViewModel_PropertyChanged;
            }

            this.OnPropertyChanged(nameof(this.PluginSelectionViewModel));
        }
    }

    /// <inheritdoc/>
    public override async Task<EboardFeedbackMessage> Load(string path)
    {
        try
        {
            var data = await new SDKDataManager().LoadPluginContent<SummonerModel>(path);

            if (data != null)
            {
                if (data.Plugin != null && data.PluginTypeSelected)
                {
                    this.InsertModel(data);
                }

                return FeedbackMessageFactory.Success($"deserialized {path}");
            }
        }
        catch (Exception ex)
        {
            return FeedbackMessageFactory.Exception(ex.Message);
        }

        return FeedbackMessageFactory.Unknown($"Load() T {typeof(SummonerModel).FullName}");
    }

    /// <inheritdoc/>
    public async override Task<EboardFeedbackMessage> Save(string path)
    {
        var model = new SummonerModel(this);

        return await new SDKDataManager().SavePluginContent(model, path);
    }

    private void PluginSelectionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (this.PluginSelectionViewModel != null)
        {
            this.SelectedPlugin = this.PluginSelectionViewModel.SelectedPlugin;
        }
    }
}

// EOF