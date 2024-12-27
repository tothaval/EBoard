/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IElementContent 
 * 
 *  interface for basic element content logic
 */
using EBoard.Models;
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
// EOF