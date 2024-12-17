using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EBoard.Interfaces
{
    public interface IElementBackgroundImage
    {
        public string ElementImagePath { get; set; }

        public Brush FallbackBackgroundBrush { get; }

        public void ChangeElementBackgroundToImage();
    }
}
