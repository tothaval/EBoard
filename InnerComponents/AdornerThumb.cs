using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace EBoard.InnerComponents
{
    internal class AdornerThumb : Thumb, IThumb
    {
        public AdornerThumb()
        {
            Width = 10;
            Height = 10;

            Background = Brushes.Coral;
        }

        public void ResetThumb(double width, double height, Brush background)
        {
            Width = width;
            Height = height;

            Background = background;
        }
    }
}
