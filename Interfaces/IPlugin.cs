using EBoard.IOProcesses.DataSets;
using EBoard.Models;
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


        public string PluginHeader { get; set; }


        public string PluginName { get; set; }

        public bool ApplyBackgroundBrush(Brush brush);

        
        public Task Load(string path, ElementDataSet elementDataSet);


        public Task Save(string path, ElementDataSet elementDataSet);


        public bool SelectionChange(bool isSelected);



    }
}
