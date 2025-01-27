using System.Windows;

namespace EBoard.Interfaces
{
    public interface IElementTransform
    {

        public CornerRadius CornerRadius { get; set; }
        

        // until proper corner radius change for every corner can be set by the user
        public int CornerRadiusValue { get; set; }

        
        public double Height { get; set; }
        

        public double Width { get; set; }

    }
}
// EOF