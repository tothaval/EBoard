﻿// <copyright file="IElementBackgroundImage.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  IElementBackgroundImage
 *
 *  interface for viewmodels if they should be able to display a background image
 */

using EBoardSDK.Enums;

namespace EBoardSDK.Interfaces;

public interface IElementBackgroundImage
{
    public string ImagePath { get; set; }

    public void ChangeElementBackgroundToImage(BrushTargets brushTargets, string path);
}

// EOF