/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardViewModel 
 * 
 *  view model class for EBoardView
 *  
 *  it is basically a canvas within a frame and some properties, that can be edited,
 *  stored(WIP) and loaded(WIP)
 */
using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class EBoardViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private string _EBID;
        /// <summary>
        /// EBoard ID, created upon first creation,
        /// built using $"EBoard_{DateTime().Ticks}"        
        /// </summary>
        public string EBID => _EBID;


        private bool _EBoardActive;
        public bool EBoardActive
        {
            get { return _EBoardActive; }
            set
            {
                _EBoardActive = value;
                OnPropertyChanged(nameof(EBoardActive));
            }
        }


        private Brush _eBoardBackgroundBrush;
        public Brush EBoardBackgroundBrush
        {
            get { return _eBoardBackgroundBrush; }
            set
            {
                _eBoardBackgroundBrush = value;
                OnPropertyChanged(nameof(EBoardBackgroundBrush));
            }
        }


        private int _EBoardDepth;
        public int EBoardDepth
        {
            get { return _EBoardDepth; }
            set
            {
                _EBoardDepth = value;
                OnPropertyChanged(nameof(EBoardDepth));
            }
        }


        private double _EBoardHeight;
        public double EBoardHeight
        {
            get { return _EBoardHeight; }
            set
            {
                _EBoardHeight = value;
                OnPropertyChanged(nameof(EBoardHeight));
            }
        }


        private double _EBoardWidth;
        public double EBoardWidth
        {
            get { return _EBoardWidth; }
            set
            {
                _EBoardWidth = value;
                OnPropertyChanged(nameof(EBoardWidth));
            }
        }


        private string _EBoardName;
        public string EBoardName
        {
            get { return _EBoardName; }
            set
            {
                _EBoardName = value;
                OnPropertyChanged(nameof(EBoardName));
            }
        } 
        #endregion


        // Collections
        #region Collections

        private ObservableCollection<ElementViewModel> elements;
        public ObservableCollection<ElementViewModel> Elements
        {
            get { return elements; }
            set
            {
                elements = value;
                OnPropertyChanged(nameof(Elements));
            }
        } 

        #endregion


        public EBoardViewModel(string name, double width = 0, double height = 0, int depth = 0, string eboardID = "-1")
        {
            EBoardName = name;
            EBoardDepth = depth;
            EBoardHeight = height;
            EBoardWidth = width;
            _EBID = eboardID;

            if (name.Equals(""))
            {
                name = "eboard";
            }

            if (width == 0)
            {   
                EBoardWidth = 800;
            }
            if (height == 0)
            {
                EBoardHeight = 640;
            }
            if (depth == 0)
            {
                EBoardDepth = 64;
            }

            _eBoardBackgroundBrush = new SolidColorBrush(Colors.CornflowerBlue);

            elements = new ObservableCollection<ElementViewModel>();

            if (_EBID == null || (eboardID != null && eboardID.Equals("-1")))
            {

                DateTime dateTime = DateTime.Now;

                _EBID = $"EBoard_{dateTime.Ticks}";
            }


        }

        internal void AddElement(ElementViewModel elementViewModel)
        {
            elements.Add(elementViewModel);
        }

    }
}
// EOF