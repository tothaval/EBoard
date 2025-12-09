// code from: TreeView File Explorer for Windows (by Michael Pendon)
// https://github.com/mikependon/RepoDB.Tutorials/tree/master

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using EEP_FSNavigator.TreeViewFileExplorer.Enums;

namespace EEP_FSNavigator.TreeViewFileExplorer.ShellClasses
{
    public class FileSystemObjectInfo : BaseObject
    {
        public FileSystemObjectInfo(FileSystemInfo info)
        {
            if (this is DummyFileSystemObjectInfo)
            {
                return;
            }

            this.Children = new ObservableCollection<FileSystemObjectInfo>();
            this.FileSystemInfo = info;

            if (info is DirectoryInfo)
            {
                this.ImageSource = FolderManager.GetImageSource(info.FullName, ItemState.Close);
                this.AddDummy();
            }
            else if (info is FileInfo)
            {
                this.ImageSource = FileManager.GetImageSource(info.FullName);
            }

            this.PropertyChanged += new PropertyChangedEventHandler(this.FileSystemObjectInfo_PropertyChanged);
        }

        public FileSystemObjectInfo(DriveInfo drive)
            : this(drive.RootDirectory)
        {
        }

        #region Events

        public event EventHandler BeforeExpand;

        public event EventHandler AfterExpand;

        public event EventHandler BeforeExplore;

        public event EventHandler AfterExplore;

        private void RaiseBeforeExpand()
        {
            this.BeforeExpand?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseAfterExpand()
        {
            this.AfterExpand?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseBeforeExplore()
        {
            this.BeforeExplore?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseAfterExplore()
        {
            this.AfterExplore?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region EventHandlers

        void FileSystemObjectInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.FileSystemInfo is DirectoryInfo)
            {
                if (string.Equals(e.PropertyName, "IsExpanded", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.RaiseBeforeExpand();
                    if (this.IsExpanded)
                    {
                        this.ImageSource = FolderManager.GetImageSource(this.FileSystemInfo.FullName, ItemState.Open);
                        if (this.HasDummy())
                        {
                            this.RaiseBeforeExplore();
                            this.RemoveDummy();
                            this.ExploreDirectories();
                            this.ExploreFiles();
                            this.RaiseAfterExplore();
                        }
                    }
                    else
                    {
                        this.ImageSource = FolderManager.GetImageSource(this.FileSystemInfo.FullName, ItemState.Close);
                    }
                    this.RaiseAfterExpand();
                }
            }
        }

        #endregion

        #region Properties

        public ObservableCollection<FileSystemObjectInfo> Children
        {
            get { return this.GetValue<ObservableCollection<FileSystemObjectInfo>>("Children"); }
            private set { this.SetValue("Children", value); }
        }

        public ImageSource ImageSource
        {
            get { return this.GetValue<ImageSource>("ImageSource"); }
            private set { this.SetValue("ImageSource", value); }
        }

        public bool IsExpanded
        {
            get { return this.GetValue<bool>("IsExpanded"); }
            set { this.SetValue("IsExpanded", value); }
        }

        public FileSystemInfo FileSystemInfo
        {
            get { return this.GetValue<FileSystemInfo>("FileSystemInfo"); }
            private set { this.SetValue("FileSystemInfo", value); }
        }

        private DriveInfo Drive
        {
            get { return this.GetValue<DriveInfo>("Drive"); }
            set { this.SetValue("Drive", value); }
        }

        #endregion

        #region Methods

        private void AddDummy()
        {
            this.Children.Add(new DummyFileSystemObjectInfo());
        }

        private bool HasDummy()
        {
            return this.GetDummy() != null;
        }

        private DummyFileSystemObjectInfo GetDummy()
        {
            return this.Children.OfType<DummyFileSystemObjectInfo>().FirstOrDefault();
        }

        private void RemoveDummy()
        {
            this.Children.Remove(this.GetDummy());
        }

        private void ExploreDirectories()
        {
            if (this.Drive?.IsReady == false)
            {
                return;
            }
            if (this.FileSystemInfo is DirectoryInfo)
            {
                var directories = ((DirectoryInfo)this.FileSystemInfo).GetDirectories();
                foreach (var directory in directories.OrderBy(d => d.Name))
                {
                    if ((directory.Attributes & FileAttributes.System) != FileAttributes.System &&
                        (directory.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        var fileSystemObject = new FileSystemObjectInfo(directory);
                        fileSystemObject.BeforeExplore += this.FileSystemObject_BeforeExplore;
                        fileSystemObject.AfterExplore += this.FileSystemObject_AfterExplore;
                        this.Children.Add(fileSystemObject);
                    }
                }
            }
        }

        private void FileSystemObject_AfterExplore(object sender, EventArgs e)
        {
            this.RaiseAfterExplore();
        }

        private void FileSystemObject_BeforeExplore(object sender, EventArgs e)
        {
            this.RaiseBeforeExplore();
        }

        private void ExploreFiles()
        {
            if (this.Drive?.IsReady == false)
            {
                return;
            }
            if (this.FileSystemInfo is DirectoryInfo)
            {
                var files = ((DirectoryInfo)this.FileSystemInfo).GetFiles();
                foreach (var file in files.OrderBy(d => d.Name))
                {
                    if ((file.Attributes & FileAttributes.System) != FileAttributes.System &&
                        (file.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        this.Children.Add(new FileSystemObjectInfo(file));
                    }
                }
            }
        }

        #endregion
    }
}
