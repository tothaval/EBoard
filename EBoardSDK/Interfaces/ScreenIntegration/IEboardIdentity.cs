// <copyright file="IEboardIdentity.cs" company=".">
// Stephan Kammel
// </copyright>

using EBoardSDK.Enums;

namespace EBoardSDK.Interfaces.ScreenIntegration;

public interface IEboardIdentity
{
    public int EBoardDepth { get; set; }

    public string EBID { get; }

    public string? EBoardName { get; }

    public IList<ElementInstantiationPolicy>? InstantiationPolicies { get; }

    public IList<EboardScreenType>? ScreenTypes { get; }
}
