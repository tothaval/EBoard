using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoardSDK.Plugins
{
    public abstract partial class EBoardElementPluginBaseViewModel : ObservableObject, IEBoardElement
    {
        public PluginDataSet PluginDataSet { get; set; } = new ();

        public BorderManagement BorderManagement { get; set; } = new();

        public BrushManagement BrushManagement { get; set; } = new();

        public abstract UserControl Plugin { get; }

        public abstract string PluginHeader { get; set; }
        public abstract string PluginName { get; set; }

        public abstract string ElementPluginName { get; }

        public abstract Assembly? ElementPluginAssembly { get; }

        public abstract Type? ElementPluginModel { get; }

        public abstract Type ElementPluginView { get; }

        public abstract Type ElementPluginViewModel { get; }

        public abstract ResourceDictionary ResourceDictionary { get; }

        [ObservableProperty]
        private CornerRadius cornerRadius;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private int cornerRadiusValue;


        partial void OnCornerRadiusValueChanged(int value)
        {
            BorderManagement.CornerRadius = new CornerRadius(value);
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double height;

        partial void OnHeightChanged(double value)
        {
            BorderManagement.Height = value;
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double width;

        partial void OnWidthChanged(double value)
        {
            BorderManagement.Width = value;
        }

        public bool ApplyBackgroundBrush(Brush brush)
        {
            try
            {
                BrushManagement.Background = brush;

                OnPropertyChanged(nameof(BrushManagement));

                OnPropertyChanged(nameof(BrushManagement.Background));

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public abstract Task Load(string path, IElementDataSet elementDataSet);

        public abstract Task Save(string path, IElementDataSet elementDataSet);

        public bool SelectionChange(bool isSelected)
        {
            if (isSelected)
            {
                BrushManagement.SwitchBorderToHighlight();

                OnPropertyChanged(nameof(BrushManagement));

                return true;
            }

            BrushManagement.SwitchBorderToBorder();

            OnPropertyChanged(nameof(BrushManagement));

            return false;
        }
    }
}
