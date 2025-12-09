// <copyright file="SoundMixMainView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>
namespace EBoardSDK.Plugins.Elements.BasicAV;

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/// <summary>
/// Interaktionslogik für SoundMixMainView.xaml
/// </summary>
public partial class BasicAVMainView : UserControl
{
    public BasicAVMainView()
    {
        this.InitializeComponent();
    }

    private void Grid_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

        e.Handled = true;
    }

    private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.StatusBarDragStart();
        }
    }

    private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        var dataContext = this.DataContext as BasicAVMainViewModel;

        if (dataContext != null)
        {
            dataContext.StatusBarDragCompleted();
        }
    }
}
