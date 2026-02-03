// <copyright file="PluginAreaModel.cs" company=".">
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
namespace EBoardSDK.Plugins.Areas.PluginArea;

using EBoardSDK.Plugins.Areas.ShapeArea;
using EBoardSDK.Plugins.Eboard.Summoner;
using System.Collections.Generic;

/// <summary>
/// Serializable data model for <see cref="PluginAreaView"/> and <see cref="ShapeAreaView"/>.
/// </summary>
public class PluginAreaModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaModel"/> class.
    /// </summary>
    public PluginAreaModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaModel"/> class.
    /// </summary>
    /// <param name="pluginAreaViewModel">Desired is the instance that has to be stored.</param>
    public PluginAreaModel(PluginAreaViewModel pluginAreaViewModel)
    {
        this.ShowMatrixControls = pluginAreaViewModel.AreaViewModel.ShowMatrixControls;

        var areavm = pluginAreaViewModel.AreaViewModel;

        if (areavm == null)
        {
            return;
        }

        foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        {
            var list = new List<SummonerModel>();

            foreach (var horizontalelement in item.Elements)
            {
                var model = new SummonerModel(horizontalelement);

                list.Add(model);
            }

            this.Summonees.Add(list);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaModel"/> class.
    /// </summary>
    /// <param name="shapeAreaViewModel">Desired is the instance that has to be stored.</param>
    public PluginAreaModel(ShapeAreaViewModel shapeAreaViewModel)
    {
        this.ShowMatrixControls = shapeAreaViewModel.AreaViewModel.ShowMatrixControls;

        var areavm = shapeAreaViewModel.AreaViewModel;

        foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        {
            var list = new List<SummonerModel>();

            foreach (var horizontalelement in item.Elements)
            {
                var model = new SummonerModel(horizontalelement);

                list.Add(model);
            }

            this.Summonees.Add(list);
        }
    }

    public List<List<SummonerModel>> Summonees { get; set; } = new ();

    public bool ShowMatrixControls { get; set; } = true;
}

// EOF