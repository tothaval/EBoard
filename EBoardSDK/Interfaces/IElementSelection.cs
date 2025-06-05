// <copyright file="IElementSelection.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Interfaces;

public interface IElementSelection
{
    public bool IsSelected { get; set; }

    public void Select();
}
