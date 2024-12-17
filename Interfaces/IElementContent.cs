using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoard.Interfaces
{
    public interface IElementContent
    {
        public bool ContentIsUserControlAndNotShape { get; }

        public FrameworkElement Element { get; }

        public Task Load(string path, ElementDataSet elementDataSet);

        public Task Save(string path, ElementDataSet elementDataSet);
    }
}
