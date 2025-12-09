// <copyright file="FSNScanService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// thanks to https://gargmanoj.wordpress.com/2009/03/27/wpf-101-a-simple-windows-explorer-like-directory-browser/ for the tutorial.
// this project mainly followed the tutorial steps, while aiming to integrate the solution into eboard.

namespace EEP_FSNavigator.Models;

using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
///  Class to get file system information.
/// </summary>
public class FSNScanService
{
    /// <summary>
    /// Gets the list of files in the directory Name passed.
    /// </summary>
    /// <param name="directory">The Directory to get the files from.</param>
    /// <returns>Returns the List of File info for this directory.
    /// Returns null if an exception is raised.</returns>
    public static IList<FileInfo> GetChildFiles(string directory)
    {
        try
        {
            return (from x in Directory.GetFiles(directory)
                    select new FileInfo(x)).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return new List<FileInfo>();
    }

    /// <summary>
    /// Gets the list of directories.
    /// </summary>
    /// <param name="directory">The Directory to get the files from.</param>
    /// <returns>Returns the List of directories info for this directory.
    /// Returns null if an exception is raised.</returns>
    public static IList<DirectoryInfo> GetChildDirectories(string directory)
    {
        try
        {
            return (from x in Directory.GetDirectories(directory)
                    select new DirectoryInfo(x)).ToList();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return new List<DirectoryInfo>();
    }

    /// <summary>
    /// Gets the root directories of the system.
    /// </summary>
    /// <returns>Return the list of root directories.</returns>
    public static IList<DriveInfo> GetRootDirectories()
    {
        return (from x in DriveInfo.GetDrives() select x).ToList();
    }
}

// EOF