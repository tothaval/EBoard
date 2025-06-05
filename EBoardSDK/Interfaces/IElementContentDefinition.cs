// <copyright file="IElementContentDefinition.cs" company=".">
// Stephan Kammel
// </copyright>

using System.Windows;

namespace EBoardSDK.Interfaces
{
    public interface IElementContentDefinition
    {
        //public bool ContentIsUserControlAndNotShape { get; }

        public FrameworkElement Plugin { get; }
    }
}
