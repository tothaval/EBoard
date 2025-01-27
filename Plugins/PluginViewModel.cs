using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Plugins.Elements.StandardText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace EBoard.Plugins
{
    public partial class PluginViewModel : ObservableObject, IPlugin
    {
        public BorderManagement BorderManagement { get; set; }


        public BrushManagement BrushManagement { get; set; }


        // später ggf. per Factory oder via Singleton, falls nötig
        // ist für die option, im load des programms den view typ sauber
        // instanzieren zu können.
        private StandardTextView plugin = new StandardTextView();
        public UserControl Plugin => plugin;


        public IPluginDataSet PluginDataSet { get; set; }


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

        [ObservableProperty]
        private string pluginHeader = "Standard Text";


        [ObservableProperty]
        private string pluginName = "StandardText";




        public PluginViewModel()
        {
            BorderManagement = new BorderManagement();
            BrushManagement = new BrushManagement();


            if (PluginHeader.Equals(string.Empty))
            {
                PluginHeader = "Standard Text";
            }
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


        public virtual Task<bool> Load(string path, IPluginDataSet pluginDataSet)
        {
            PluginDataSet = pluginDataSet;

            BorderManagement = new BorderManagement(pluginDataSet.BorderDataSet);
            BrushManagement = new BrushManagement(pluginDataSet.BrushDataSet);

            PluginHeader = PluginDataSet.PluginHeader;
            PluginName = PluginDataSet.PluginName;

            return Task.FromResult(true);
        }


        public virtual async Task<bool> OnEboardShutdown(string path)
        {
            PluginDataSet = new PluginDataSet(this);

            await Task.CompletedTask;

            return true;
        }


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
// EOF