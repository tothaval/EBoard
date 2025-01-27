/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ResizeAdorner 
 * 
 *  helper class for Adorner logic
 *  
 *  handles resizing with resize adorners
 *  
 *  only the top thumbs are implemented atm
 *  
 *  it is unsure whether adorners will stay, they probably must be removed to keep
 *  a consistent appearance throughout the application, because it is not possible
 *  to have ui elements above the adorner layer, they will be allways visible if 
 *  allways active and probably won't look good, if they are just visible on an option
 */
using EBoard.ViewModels;
using EBoard.Views;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EBoard.InnerComponents
{
    /// <summary>
    /// 
    /// integrate missing events for thumb3 and thumb4
    /// 
    /// rename thumbs to thumbCORNER or something alike
    /// 
    /// fix resize behaviour, which is kind of weird atm.
    /// 
    /// apply adorners to ElementView only, change width
    /// and height properties of element via adorners
    /// 
    /// </summary>
    public class ResizeAdorner : Adorner
    {
        VisualCollection AdornerVisuals;

        private ElementView _ElementView;

        AdornerThumb thumb1, thumb2, thumb3, thumb4;
        Rectangle rectangle;

        double width = 10;
        double height = 10;

        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _ElementView = (ElementView)adornedElement;

            AdornerVisuals = new VisualCollection(this);
            thumb1 = new AdornerThumb();
            thumb2 = new AdornerThumb();
            thumb3 = new AdornerThumb();
            thumb4 = new AdornerThumb();

            rectangle = new Rectangle()
            {
                Stroke = Brushes.DarkSlateGray,
                StrokeThickness = 1,
                StrokeDashArray = { 4, 4 }
            };

            thumb1.DragDelta += Thumb1_DragDelta;
            thumb2.DragDelta += Thumb2_DragDelta;

            AdornerVisuals.Add(rectangle);
            AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);
            AdornerVisuals.Add(thumb3);
            AdornerVisuals.Add(thumb4);
        }

        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ElementViewModel elementViewModel = (ElementViewModel)_ElementView.DataContext;
            
            elementViewModel.Plugin.Height = height - e.VerticalChange < 0 ? 0 : height - e.VerticalChange;
            elementViewModel.Plugin.Width = width - e.HorizontalChange < 0 ? 0 : width - e.HorizontalChange;
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ElementViewModel elementViewModel = (ElementViewModel)_ElementView.DataContext;
            
            elementViewModel.Plugin.Height = height + e.VerticalChange < 0 ? 0 : height + e.VerticalChange;
            elementViewModel.Plugin.Width = width + e.HorizontalChange < 0 ? 0 : width + e.HorizontalChange;            
        }

        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            rectangle.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));

            thumb1.Arrange(new Rect(-5, -5, width, height));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, -5, width, height));
            thumb3.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, width, height));
            thumb4.Arrange(new Rect(-5, AdornedElement.DesiredSize.Height - 5, width, height));

            return base.ArrangeOverride(finalSize);
        }
    }
}
// EOF