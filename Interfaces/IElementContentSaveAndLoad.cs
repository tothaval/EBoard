using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Interfaces
{
    public interface IElementContentSaveAndLoad
    {
        public Task Load(string path, ElementDataSet elementDataSet);

        public Task Save(string path, ElementDataSet elementDataSet);
    }
}
