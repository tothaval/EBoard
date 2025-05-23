﻿using EBoard.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.DataContext = mainViewModel;
        }


        private void SizeAndPositionUpdate()
        {
            ((MainViewModel)DataContext).PlacementManager.Position = new Point(Left, Top);

            ((MainViewModel)DataContext).BorderManager.Width = ActualWidth;
            ((MainViewModel)DataContext).BorderManager.Height = ActualHeight;
        }


        private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {          
            ((MainViewModel)DataContext).DeselectElements();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }


        }


        private void EboardMainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SizeAndPositionUpdate();

            e.Handled = true;
        }


        private void EboardMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SizeAndPositionUpdate();

            e.Handled = true;
        }


    }
}
// EOF