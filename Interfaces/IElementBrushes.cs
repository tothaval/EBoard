using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EBoard.Interfaces
{
    public interface IElementBrushes
    {

        public Brush ElementBackground { get; set; }
        
        
        public Brush ElementForeground { get; set; }


        public Brush ElementBorder { get; set; }


        public Brush ElementHighlight { get; set; }


        public Thickness ElementBorderThickness { get; set; }


        public string ElementImagePath { get; set; }

    }
}
