// <copyright file="ElementScreenIntegrationConstraints.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Models;

using EBoardSDK.Enums;
using EBoardSDK.Interfaces;
using EBoardSDK.Interfaces.ScreenIntegration;

public class ElementScreenIntegrationConstraints : IScreenIntegrationObject
{
    /*  purpose:    emanates an identity constraint to eboard screens, based on that, allowed interactions with eboard screens
     *              are configurable, if they implement this class; constraints must state if the element should be instantiated
     *              with a new instance on each screen each time, or only one instance on every screen of a certain type or
     *              global to all screens or else e.g.;
     *  goal:       creating limitable interactions of certain types of plugins with certain types of screens or with other plugins;
     *              container class for additions to plugin logic, so that upgrades to core eboard functionality can be handled
     *              more fluently, because values within this class can be always null, which in turn must result in such plugins
     *              to be only callable on general screens; a default value for instantiation amount on a screen could depend on
     *              a screen configuraton, which handles null values in plugin constraints data;
     *              all other element related data resides in the general plugin architecture (iplugin and the viewmodel implementation)
     *  reasoning:  security or usability concerns, e.g. a login screen element could emanate a global instantiation policy to eboard, so
     *              that only one instance can be called on every or only on one screen;
     *              utilization of eboard modularity to achhieve a feature, f.e. a game that stretches over several screens, where the player
     *              must switch to other screens to achieve certain goals, or a software that distributes the solutions of its requirements
     *              to its users to several screens and element plugins to bundle certain parts of its functional operations.
     * todo:        define a factory object for element instantiation on screens, that takes the values of this class into account
     */

    private readonly ElementInstantiationPolicy? elementInstantiationPolicy;

    // für mögliche interaktionen mit dem eboard durch das element, zum beispiel die nutzung speziell dafür vorgesehener funktionen
    // private readonly ElementViewModel elementViewModel; // TODO ElementViewModel, EBoardViewModel etc umziehen nach EboardSDK

    public ElementScreenIntegrationConstraints(ElementInstantiationPolicy? elementInstantiationPolicy = ElementInstantiationPolicy.ValueNotSet, IPlugin? plugin = null)
    {
        this.elementInstantiationPolicy = elementInstantiationPolicy;
    }

    public IList<IEboardIdentity>? AcceptedScreenIdentities { get; }

    public IList<EboardScreenType>? AcceptedScreenTypes { get; }

    public ElementInstantiationPolicy? InstantiationPolicy => elementInstantiationPolicy;

    public IList<IScreenIntegrationObject>? ScreenIntegrationObjects { get;  }
}

// EOF