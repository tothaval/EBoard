using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace EBoard.InnerComponents
{
    internal class AdornerThumb : Thumb, IThumb
    {
        public AdornerThumb()
        {
            Width = 9;
            Height = 9;

            Background = Brushes.DarkSlateGray;
        }

        public void ResetThumb(double width, double height, Brush background)
        {
            Width = width;
            Height = height;

            Background = background;
        }
    }
}
