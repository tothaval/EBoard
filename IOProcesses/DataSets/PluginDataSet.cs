using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.IOProcesses.DataSets;

[Serializable]
public class PluginDataSet
{
    public string Text { get; set; } = string.Empty;
    
    public List<object> Values { get; set; } = new List<object>();

    public List<References> References { get; set; } = new List<References>();              
}


public record References(string Key, string Value);

