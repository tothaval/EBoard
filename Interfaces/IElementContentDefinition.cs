using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoard.Interfaces
{
    public interface IElementContentDefinition
    {
        public bool ContentIsUserControlAndNotShape { get; }

        public FrameworkElement Element { get; }

    }
}
