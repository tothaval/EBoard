// <copyright file="SummonerModel.cs" company=".">
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

using EBoardSDK.Models;

/// <summary>
/// Serializable data model for <see cref="SummonerView"/>.
/// </summary>
public class SummonerModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SummonerModel"/> class.
    /// </summary>
    public SummonerModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SummonerModel"/> class.
    /// </summary>
    /// <param name="summonerViewModel">Desired is the instance that has to be stored.</param>
    public SummonerModel(SummonerViewModel summonerViewModel)
    {
        this.PluginTypeSelected = summonerViewModel.PluginTypeSelected;
        this.Plugin = summonerViewModel.PluginTypeSelected ? summonerViewModel.SelectedPlugin : null;

        if (summonerViewModel.Summonee != null)
        {
            summonerViewModel.Summonee.PrepareCopy();

            if (summonerViewModel.Summonee.PluginModel != null)
            {
                this.SummoneeModel = summonerViewModel.Summonee.PluginModel;
            }
        }

        //if (summonerViewModel.Summonee?.PluginModelType != null)
        //{
        //    var test = Activator.CreateInstance(summonerViewModel.Summonee.PluginModelType, summonerViewModel.Summonee);

        //    if (test != null)
        //    {
        //        this.SummoneeModel = test;
        //    }
        //}
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SummonerModel"/> class.
    /// </summary>
    /// <param name="summonerViewModel">Desired is the instance that has to be stored.</param>
    public SummonerModel(ShapeSummonerViewModel summonerViewModel)
    {
        this.PluginTypeSelected = summonerViewModel.PluginTypeSelected;
        this.Plugin = summonerViewModel.PluginTypeSelected ? summonerViewModel.SelectedPlugin : null;

        if (summonerViewModel.Summonee != null)
        {
            summonerViewModel.Summonee.PrepareCopy();

            if (summonerViewModel.Summonee.PluginModel != null)
            {
                this.SummoneeModel = summonerViewModel.Summonee.PluginModel;
            }
        }

        //if (summonerViewModel.Summonee?.PluginModelType != null)
        //{
        //    var test = Activator.CreateInstance(summonerViewModel.Summonee.PluginModelType, summonerViewModel.Summonee);

        //    if (test != null)
        //    {
        //        this.SummoneeModel = test;
        //    }
        //}
    }

    public bool PluginTypeSelected { get; set; } = false;

    public PluginRepresentationItem? Plugin { get; set; } = null;

    public object SummoneeModel { get; set; } = new object();
}

// EOF