namespace EEP_FSNavigator.ViewModels;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// Enum to hold the Types of different file objects.
/// </summary>
public enum ObjectType
{
    /// <summary>
    /// Represents the machine the os is running on.
    /// </summary>
    MyComputer = 0,

    /// <summary>
    /// Represents a hard drive.
    /// </summary>
    Diskdrive = 1,

    /// <summary>
    /// Represents a directory.
    /// </summary>
    Directory = 2,

    /// <summary>
    /// Represents a file.
    /// </summary>
    File = 3,
}

/// <summary>
/// Class for containing the information about a Directory/File.
/// </summary>
public class FSNDirectoryInfo : DependencyObject
{
    public FSNDirectoryInfo()
    {
        this.SubDirectories = new List<FSNDirectoryInfo>();
        this.SubDirectories.Add(new FSNDirectoryInfo("TempDir"));
    }

    public FSNDirectoryInfo(string directoryName)
    {
        this.Name = directoryName;
    }

    public FSNDirectoryInfo(DirectoryInfo dirInfo)
        : this()
    {
        this.DirType = (int)ObjectType.Directory;
        this.Name = dirInfo.Name;
        this.Path = dirInfo.FullName;
        this.Root = dirInfo.Root.Name;
    }

    public FSNDirectoryInfo(FileInfo fileInfo)
    {
        this.DirType = (int)ObjectType.File;
        this.Ext = $"{fileInfo.Extension} File";
        this.Name = fileInfo.Name;
        this.Path = fileInfo.FullName;
        this.Size = $"{fileInfo.Length / 1024} KB";
    }

    public FSNDirectoryInfo(DriveInfo driveInfo)
        : this()
    {
        this.DirType = (int)ObjectType.Diskdrive;

        if (driveInfo.Name.EndsWith(@"\"))
        {
            this.Name = driveInfo.Name.Substring(0, driveInfo.Name.Length - 1);
        }
        else
        {
            this.Name = driveInfo.Name;
        }

        this.Path = driveInfo.Name;
    }

    public int DirType { get; set; }

    public string Ext { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public string Root { get; set; }

    public string Size { get; set; }

    public static readonly DependencyProperty PropertyChilds = DependencyProperty.Register("Childs", typeof(IList<FSNDirectoryInfo>), typeof(FSNDirectoryInfo));

    public IList<FSNDirectoryInfo> SubDirectories
    {
        get { return (IList<FSNDirectoryInfo>)this.GetValue(PropertyChilds); }
        set { this.SetValue(PropertyChilds, value); }
    }

    public static readonly DependencyProperty PropertyIsExpanded = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(FSNDirectoryInfo));

    public bool IsExpanded
    {
        get { return (bool)this.GetValue(PropertyIsExpanded); }
        set { this.SetValue(PropertyIsExpanded, value); }
    }

    public static readonly DependencyProperty PropertyIsSelected = DependencyProperty.Register("IsSelected", typeof(bool), typeof(FSNDirectoryInfo));

    public bool IsSelected
    {
        get { return (bool)this.GetValue(PropertyIsSelected); }
        set { this.SetValue(PropertyIsSelected, value); }
    }
}
