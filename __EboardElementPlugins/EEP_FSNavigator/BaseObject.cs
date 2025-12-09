// code from: TreeView File Explorer for Windows (by Michael Pendon)
// https://github.com/mikependon/RepoDB.Tutorials/tree/master

using System;
using System.Collections.Generic;

namespace EEP_FSNavigator.TreeViewFileExplorer
{
    [Serializable]
    public abstract class BaseObject : PropertyNotifier
    {
        private IDictionary<string, object> m_values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public T GetValue<T>(string key)
        {
            var value = this.GetValue(key);
            return (value is T) ? (T)value : default(T);
        }

        private object GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return this.m_values.ContainsKey(key) ? this.m_values[key] : null;
        }

        public void SetValue(string key, object value)
        {
            if (!this.m_values.ContainsKey(key))
            {
                this.m_values.Add(key, value);
            }
            else
            {
                this.m_values[key] = value;
            }
            this.OnPropertyChanged(key);
        }
    }
}
