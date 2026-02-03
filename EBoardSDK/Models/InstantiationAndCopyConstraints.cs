// <copyright file="InstantiationAndCopyConstraints.cs" company=".">
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
namespace EBoardSDK.Models;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.ScreenIntegration;

/// <summary>
///  /*  purpose:    emanates an identity constraint to eboard screens, based on that, allowed interactions with eboard screens
///  * are configurable, if they implement this class; constraints must state if the element should be instantiated
///  * with a new instance on each screen each time, or only one instance on every screen of a certain type or
///  * global to all screens or else e.g.;
///  * goal:       creating limitable interactions of certain types of plugins with certain types of screens or with other plugins;
///  * container class for additions to fluidUIDesign logic, so that upgrades to core eboard functionality can be handled
///  * more fluently, because values within this class can be always null, which in turn must result in such plugins
///  * to be only callable on general screens; a default value for instantiation amount on a screen could depend on
///  * a screen configuraton, which handles null values in fluidUIDesign constraints data;
///  * all other element related data resides in the general fluidUIDesign architecture(iplugin and the viewmodel implementation)
///  * reasoning:  security or usability concerns, e.g.a login screen element could emanate a global instantiation policy to eboard, so
///  * that only one instance can be called on every or only on one screen;
///  * utilization of eboard modularity to achhieve a feature, f.e.a game that stretches over several screens, where the player
///  * must switch to other screens to achieve certain goals, or a software that distributes the solutions of its requirements
///  * to its users to several screens and element plugins to bundle certain parts of its functional operations.
///  * todo:        define a factory object for element instantiation on screens, that takes the values of this class into account
///  */.
/// </summary>
public class InstantiationAndCopyConstraints : IScreenIntegrationObject
{
    private readonly InstantiationPolicy? elementInstantiationPolicy;
    private readonly CopyConstraints? copyConstraints;

    // fuer moegliche interaktionen des plugins mit dem screen, zum beispiel die nutzung speziell dafuer vorgesehener funktionen

    /// <summary>
    /// Initializes a new instance of the <see cref="InstantiationAndCopyConstraints"/> class.
    /// </summary>
    /// <param name="elementInstantiationPolicy"></param>
    /// <param name="copyConstraints"></param>
    public InstantiationAndCopyConstraints(InstantiationPolicy? elementInstantiationPolicy = Enums.InstantiationPolicy.ValueNotSet, CopyConstraints? copyConstraints = Enums.CopyConstraints.ValueNotSet)
    {
        this.elementInstantiationPolicy = elementInstantiationPolicy;
        this.copyConstraints = copyConstraints;
    }

    public IList<IEboardIdentity>? AcceptedScreenIdentities { get; }

    public IList<EboardScreenType>? AcceptedScreenTypes { get; }

    public InstantiationPolicy? InstantiationPolicy => this.elementInstantiationPolicy;

    public CopyConstraints? CopyConstraints => this.copyConstraints;
}

// EOF