using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EBoard.InnerComponents
{
    public class ResizeAdorner : Adorner
    {
        VisualCollection AdornerVisuals;

        AdornerThumb thumb1, thumb2;
        Rectangle rectangle;

        double width = 10;
        double height = 10;

        public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
        {
            AdornerVisuals = new VisualCollection(this);
            thumb1 = new AdornerThumb();
            thumb2 = new AdornerThumb();

            rectangle = new Rectangle() {
                Stroke = Brushes.Coral,
                StrokeThickness = 2,
                StrokeDashArray = { 3, 2 }
            };

            thumb1.DragDelta += Thumb1_DragDelta;
            thumb2.DragDelta += Thumb2_DragDelta;

            AdornerVisuals.Add(rectangle);
            AdornerVisuals.Add(thumb1);
            AdornerVisuals.Add(thumb2);

        }

        private void Thumb1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;
            element.Height = height - e.VerticalChange < 0 ? 0 : height - e.VerticalChange;
            element.Width = width - e.HorizontalChange < 0 ? 0 : width - e.HorizontalChange;
        }

        private void Thumb2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;
            element.Height = height + e.VerticalChange < 0 ? 0 : height + e.VerticalChange;
            element.Width = width + e.HorizontalChange < 0 ? 0 : width + e.HorizontalChange;
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
            thumb2.Arrange(new Rect(AdornedElement.DesiredSize.Width - 5, AdornedElement.DesiredSize.Height - 5, width, height));

            return base.ArrangeOverride(finalSize);
        }
    }
}
