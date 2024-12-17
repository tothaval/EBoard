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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SizeAndPositionUpdate()
        {
            ((MainViewModel)DataContext).PositionX = Left;
            ((MainViewModel)DataContext).PositionY = Top;

            ((MainViewModel)DataContext).EBoardWidth = ActualWidth;
            ((MainViewModel)DataContext).EBoardHeight = ActualHeight;
        }



        private void EboardMainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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