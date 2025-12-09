using EEP_FSNavigator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EEP_FSNavigator.TreeViewFileExplorer.ShellClasses;

namespace EEP_FSNavigator.Views
{
    /// <summary>
    /// Interaktionslogik für FSNFileSystemTreeView.xaml
    /// </summary>
    public partial class FSNFileSystemTreeView : UserControl
    {
        public FSNFileSystemTreeView()
        {
            this.InitializeComponent();

            // code below from: TreeView File Explorer for Windows (by Michael Pendon)
            // https://github.com/mikependon/RepoDB.Tutorials/tree/master
            this.InitializeFileSystemObjects();
        }

        #region Events

        private void FileSystemObject_AfterExplore(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void FileSystemObject_BeforeExplore(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.Wait;
        }

        #endregion

        #region Methods

        private void InitializeFileSystemObjects()
        {
            var drives = DriveInfo.GetDrives();
            DriveInfo
                .GetDrives()
                .ToList()
                .ForEach(drive =>
                {
                    var fileSystemObject = new FileSystemObjectInfo(drive);
                    fileSystemObject.BeforeExplore += this.FileSystemObject_BeforeExplore;
                    fileSystemObject.AfterExplore += this.FileSystemObject_AfterExplore;
                    this.treeView.Items.Add(fileSystemObject);
                });
            //PreSelect(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        //private void PreSelect(string path)
        //{
        //    if (!Directory.Exists(path))
        //    {
        //        return;
        //    }
        //    var driveFileSystemObjectInfo = GetDriveFileSystemObjectInfo(path);
        //    driveFileSystemObjectInfo.IsExpanded = true;
        //    PreSelect(driveFileSystemObjectInfo, path);
        //}

        //private void PreSelect(FileSystemObjectInfo fileSystemObjectInfo,
        //    string path)
        //{
        //    foreach (var childFileSystemObjectInfo in fileSystemObjectInfo.Children)
        //    {
        //        var isParentPath = IsParentPath(path, childFileSystemObjectInfo.FileSystemInfo.FullName);
        //        if (isParentPath)
        //        {
        //            if (string.Equals(childFileSystemObjectInfo.FileSystemInfo.FullName, path))
        //            {
        //                /* We found the item for pre-selection */
        //            }
        //            else
        //            {
        //                childFileSystemObjectInfo.IsExpanded = true;
        //                PreSelect(childFileSystemObjectInfo, path);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Helpers

        //private FileSystemObjectInfo GetDriveFileSystemObjectInfo(string path)
        //{
        //    var directory = new DirectoryInfo(path);
        //    var drive = DriveInfo
        //        .GetDrives()
        //        .Where(d => d.RootDirectory.FullName == directory.Root.FullName)
        //        .FirstOrDefault();
        //    return GetDriveFileSystemObjectInfo(drive);
        //}

        //private FileSystemObjectInfo GetDriveFileSystemObjectInfo(DriveInfo drive)
        //{
        //    foreach (var fso in treeView.Items.OfType<FileSystemObjectInfo>())
        //    {
        //        if (fso.FileSystemInfo.FullName == drive.RootDirectory.FullName)
        //        {
        //            return fso;
        //        }
        //    }
        //    return null;
        //}

        //private bool IsParentPath(string path,
        //    string targetPath)
        //{
        //    return path.StartsWith(targetPath);
        //}

        #endregion
    }
}
