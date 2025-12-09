// code from: TreeView File Explorer for Windows (by Michael Pendon)
// https://github.com/mikependon/RepoDB.Tutorials/tree/master

using System.IO;

namespace EEP_FSNavigator.TreeViewFileExplorer.ShellClasses
{
    internal class DummyFileSystemObjectInfo : FileSystemObjectInfo
    {
        public DummyFileSystemObjectInfo()
            : base(new DirectoryInfo("DummyFileSystemObjectInfo"))
        {
        }
    }
}
