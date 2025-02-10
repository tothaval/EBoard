using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Interfaces
{
    public interface IPluginData
    {
        public PluginDataSet PluginDataSet { get; set; }

    }
}
