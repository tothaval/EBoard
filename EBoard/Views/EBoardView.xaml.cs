using EBoard.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EBoard.Views
{
    /// <summary>
    /// Interaktionslogik für EBoardView.xaml
    /// </summary>
    public partial class EBoardView : UserControl
    {
        public EBoardView()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((EBoardViewModel)DataContext).DeselectElements();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Application.Current.MainWindow.DragMove();
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            e.Handled = true;
        }
    }
}