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

            rectangle = new Rectangle() {
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
            _ElementView.CurrentHeight = height - e.VerticalChange < 0 ? 0 : height - e.VerticalChange;
            _ElementView.CurrentWidth = width - e.HorizontalChange < 0 ? 0 : width - e.HorizontalChange;
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _ElementView.CurrentHeight = height + e.VerticalChange < 0 ? 0 : height + e.VerticalChange;
            _ElementView.CurrentWidth = width + e.HorizontalChange < 0 ? 0 : width + e.HorizontalChange;
        }

        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            rectangle.Arrange(new Rect(-2.5, -2.5, AdornedElement.DesiredSize.Width + 5, AdornedElement.DesiredSize.Height + 5));

            thumb1.Arrange(new Rect(-5,-5, width, height));
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, - 5, width, height));
            thumb3.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, width, height));
            thumb4.Arrange(new Rect(- 5, AdornedElement.DesiredSize.Height - 5, width, height));

            return base.ArrangeOverride(finalSize);
        }
    }
}
