/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IThumb 
 * 
 *  interface for Adorner logic
 */

using System.Windows.Media;

namespace EBoard.InnerComponents
{
    internal interface IThumb
    {
        public void ResetThumb(double width, double height, Brush background);
    }
}
// EOF