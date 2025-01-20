﻿/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IElementBrushes 
 * 
 *  interface for BrusManagement
 */
using System.Windows.Media;

namespace EBoard.Interfaces;

public interface IElementBrushes
{

    public Brush Background { get; set; }
    
    
    public Brush Foreground { get; set; }


    public Brush Border { get; set; }


    public Brush Highlight { get; set; }


    public string ImagePath { get; set; }

}
// EOF