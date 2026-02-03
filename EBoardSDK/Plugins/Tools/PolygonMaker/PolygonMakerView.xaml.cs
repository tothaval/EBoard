// <copyright file="PolygonMakerView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Plugins.Tools.PolygonMaker;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaktionslogik für PolygonMakerView.xaml.
/// </summary>
public partial class PolygonMakerView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolygonMakerView"/> class.
    /// </summary>
    public PolygonMakerView()
    {
        this.InitializeComponent();
    }

    private void ListBoxItem_DragOver(object sender, DragEventArgs e)
    {
        if (sender is FrameworkElement element)
        {
            var target = element.DataContext;
            var item = e.Data.GetData(DataFormats.Serializable);

            var datacontext = this.DataContext as PolygonMakerViewModel;

            if (datacontext != null)
            {
                datacontext.InsertDropItemToPointsCollection(item, target);
            }
        }
    }

    private void ListBoxItem_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed &&
           sender is FrameworkElement frameworkElement)
        {
            object item = frameworkElement.DataContext;

            DragDropEffects dragDropResult = DragDrop.DoDragDrop(
                frameworkElement,
                new DataObject(DataFormats.Serializable, item),
                DragDropEffects.Move);

            if (dragDropResult == DragDropEffects.None)
            {
                var datacontext = this.DataContext as PolygonMakerViewModel;

                if (datacontext != null)
                {
                    datacontext.AddDropItemToPointsCollection(item);
                }
            }
        }
    }

    private void PointCollectionListBox_DragOver(object sender, System.Windows.DragEventArgs e)
    {
        var item = e.Data.GetData(DataFormats.Serializable);

        var datacontext = this.DataContext as PolygonMakerViewModel;

        if (datacontext != null)
        {
            datacontext.AddDropItemToPointsCollection(item);
        }
    }

    private void PointCollectionListBox_DragLeave(object sender, System.Windows.DragEventArgs e)
    {
        HitTestResult result = VisualTreeHelper.HitTest(this.PointCollectionListBox, e.GetPosition(this.PointCollectionListBox));

        if (result == null)
        {
            var item = e.Data.GetData(DataFormats.Serializable);

            var datacontext = this.DataContext as PolygonMakerViewModel;

            if (datacontext != null)
            {
                datacontext.RemoveDropItemFromPointsCollection(item);
            }
        }
    }
}

// EOF