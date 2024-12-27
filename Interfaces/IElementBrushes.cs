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

        public Brush Background { get; set; }
        
        
        public Brush Foreground { get; set; }


        public Brush Border { get; set; }


        public Brush Highlight { get; set; }


        public string ImagePath { get; set; }

    }
}
