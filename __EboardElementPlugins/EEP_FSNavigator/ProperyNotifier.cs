// code from: TreeView File Explorer for Windows (by Michael Pendon)
// https://github.com/mikependon/RepoDB.Tutorials/tree/master

using System;
using System.ComponentModel;

namespace EEP_FSNavigator.TreeViewFileExplorer
{
    [Serializable]
    public abstract class PropertyNotifier : INotifyPropertyChanged
    {
        public PropertyNotifier() : base() { }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
