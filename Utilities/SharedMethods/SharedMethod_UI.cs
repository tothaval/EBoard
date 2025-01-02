using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace EBoard.Utilities.SharedMethods
{
    internal class SharedMethod_UI
    {

        public SolidColorBrush ImagePathErrorDefaultBrush => new SolidColorBrush(Colors.White);


        public Brush ChangeBackgroundToImage(Brush brush, string imagePath)
        {
            if (imagePath == null || imagePath == string.Empty)
            {
                return brush;
            }

            try
            {
                brush = new ImageBrush(new BitmapImage(
                    new Uri(imagePath, UriKind.Absolute)));
            }
            catch (Exception)
            {
                return ImagePathErrorDefaultBrush;
            }

            return brush;
        }
    }
}
