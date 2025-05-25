/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IElementPlacement 
 *  
 *  interface for PlacementManager class
 */
namespace EBoardSDK.Interfaces;

using System.Windows;

interface IElementPlacement
{
    public double Angle { get; set; }

    public Point Position { get; set; }

    public int Z { get; set; }
}
// EOF