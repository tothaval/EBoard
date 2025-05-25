/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IElementBorder 
 * 
 *  interface for BorderManagement
 */
using System.Windows;

namespace EBoardSDK.Interfaces;

public interface IElementBorder
{
    public Thickness BorderThickness { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public CornerRadius CornerRadius { get; set; }

    public Thickness Margin { get; set; }

    public Thickness Padding { get; set; }
}
// EOF