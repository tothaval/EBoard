// code from: TreeView File Explorer for Windows (by Michael Pendon)
// https://github.com/mikependon/RepoDB.Tutorials/tree/master

using System.Runtime.InteropServices;

namespace EEP_FSNavigator.TreeViewFileExplorer.Structs
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ShellFileInfo
    {
        public IntPtr hIcon;

        public int iIcon;

        public uint dwAttributes;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
}
