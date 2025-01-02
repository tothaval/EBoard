using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBoard.Interfaces
{
    public interface IUIManager
    {
        public bool IsSelected { get; set; }


        public BorderManagement BorderManager { get; set; }


        public BrushManagement BrushManager { get; set; }


        public void ApplyBackgroundBrush(Brush brush);


        public void DeselectElement();


        public void SelectElement();
    }
}
