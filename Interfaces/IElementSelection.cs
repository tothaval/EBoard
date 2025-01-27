using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBoard.Interfaces;

public interface IElementSelection
{
    public bool IsSelected { get; set; }


    public void Select();
}
