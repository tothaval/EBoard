// <copyright file="ICollectiveClickableObject.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces
{
    public interface ICollectiveClickableObject
    {
        public Action CollectiveClickEvent { get; }
    }
}
