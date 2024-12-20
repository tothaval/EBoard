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
        public string ImagePath { get; set; }

        public void ChangeElementBackgroundToImage();
    }
}
