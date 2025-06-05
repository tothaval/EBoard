// <copyright file="IScreenIntegrationObject.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces.ScreenIntegration;

using EBoardSDK.Enums;

public interface IScreenIntegrationObject
{
    public ElementInstantiationPolicy? InstantiationPolicy { get; }

    public IList<EboardScreenType>? AcceptedScreenTypes { get; }

    public IList<IEboardIdentity>? AcceptedScreenIdentities { get; }

    // ...
}
