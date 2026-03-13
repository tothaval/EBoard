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

using EBoardSDK.Plugins.Elements.Link;
using System.Collections.Generic;
using System.Text.Json.Serialization;

internal class PluginAreaModel
{
    [JsonIgnore]
    private readonly PluginAreaViewModel pluginAreaViewModel;

    internal List<List<LinkModel>> Links { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaModel"/> class.
    /// </summary>
    internal PluginAreaModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginAreaModel"/> class.
    /// </summary>
    /// <param name="pluginAreaViewModel"></param>
    internal PluginAreaModel(PluginAreaViewModel pluginAreaViewModel)
    {
        this.pluginAreaViewModel = pluginAreaViewModel;

        //this.Links = new();

<<<<<<< Updated upstream
        //var areavm = shapeAreaViewModel.AreaViewModel;
=======
        if (areavm == null || areavm.AreaVerticalOuterViewModel == null)
        {
            return;
        }
>>>>>>> Stashed changes

        //foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        //{
        //    var list = new List<LinkModel>();

        //    foreach (var horizontalelement in item.Elements)
        //    {
        //        var linkmodel = new LinkModel(horizontalelement);

        //        list.Add(linkmodel);
        //    }

        //    this.Links.Add(list);
        //}
    }
}

// EOF