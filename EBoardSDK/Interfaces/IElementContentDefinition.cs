// <copyright file="IElementContentDefinition.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces
{
    using System.Windows;

    public interface IElementContentDefinition
    {
        // public bool ContentIsUserControlAndNotShape { get; }
        public FrameworkElement Plugin { get; }
    }
}
