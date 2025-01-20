/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IElementPlacement 
 *  
 *  interface for PlacementManager class
 */
using System.Windows;

namespace EBoard.Interfaces;

interface IElementPlacement
{

    public double Angle { get; set; }

    public Point Position { get; set; }

    public int Z { get; set; }

}
// EOF