using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoard.Interfaces
{
    public interface IPlugin : IElementTransform
    {

        public BorderManagement BorderManagement { get; set; }


        public BrushManagement BrushManagement { get; set; }


        public UserControl Plugin { get; }


        public IPluginDataSet PluginDataSet { get; set; }


        public string PluginHeader { get; set; }


        public string PluginName { get; set; }

        public bool ApplyBackgroundBrush(Brush brush);


        public Task<bool> Load(string path, IPluginDataSet pluginDataSet);


        public Task<bool> OnEboardShutdown(string path);


        public bool SelectionChange(bool isSelected);



    }
}
