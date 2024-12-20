using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoard.Interfaces
{
    public interface IElementBorder
    {
        public Thickness BorderThickness { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public CornerRadius CornerRadius { get; set; }

        public Thickness Margin { get; set; }

        public Thickness Padding { get; set; }
    }
}
