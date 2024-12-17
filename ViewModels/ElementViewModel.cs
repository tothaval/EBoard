/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementViewModel 
 * 
 *  helper class for

 */

using EBoard.Commands;
using EBoard.Commands.ContextMenuCommands;
using EBoard.Interfaces;
using EBoard.Models;
using EBoard.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EBoard.ViewModels
{
    public class ElementViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private EBoardViewModel _EBoardViewModel;

        private ContentViewModel _ContentViewModel;
        public ContentViewModel ContentViewModel => _ContentViewModel;

        private ShapeViewModel _ShapeViewModel;
        public ShapeViewModel ShapeViewModel => _ShapeViewModel;


        private string _EID;
        /// <summary>
        /// Element ID, created upon first creation,
        /// built using $"Element_{DateTime().Ticks}"        
        /// </summary>
        public string EID => _EID;


        private double _ElementRotation;
        public double ElementRotation
        {
            get { return _ElementRotation; }
            set
            {
                _ElementRotation = value;
                OnPropertyChanged(nameof(ElementRotation));
            }
        }


        private bool _IsShape = false;
        public bool IsShape
        {
            get { return _IsShape; }
            set
            {
                _IsShape = value;
                OnPropertyChanged(nameof(IsShape));
            }
        }


        private bool _IsContent = false;
        public bool IsContent
        {
            get { return _IsContent; }
            set
            {
                _IsContent = value;
                OnPropertyChanged(nameof(IsContent));
            }
        }


        private double _X;
        public double X
        {
            get { return _X; }
            set
            {
                _X = value;
                OnPropertyChanged(nameof(_X));
            }
        }


        private double _Y;
        public double Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                OnPropertyChanged(nameof(Y));
            }
        }


        private double _Z;
        public double Z
        {
            get { return _Z; }
            set
            {
                _Z = value;
                OnPropertyChanged(nameof(Z));
            }
        }

        #endregion


        #region Commands

        //public ICommand LeftClickCommand { get; }

        public ICommand ImageCommand { get; }

        public ICommand RightClickCommand { get; }

        #endregion


        public ElementViewModel(EBoardViewModel eBoardViewModel)
        {
            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);

            ImageCommand = new ImageCommand(this);

            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            _EBoardViewModel = eBoardViewModel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eBoardViewModel"></param>
        /// <param name="x">x axis</param>
        /// <param name="y">y axis</param>
        /// <param name="z">depth</param>
        /// <param name="elementHeaderText">element header</param>
        /// <param name="brush">background brush</param>
        /// <param name="control">element content</param>
        public ElementViewModel(
            EBoardViewModel eBoardViewModel,
            ElementDataSet elementDataSet
            )
        {

            //LeftClickCommand = new RelayCommand((s) => DragMove(s), (s) => true);
            ImageCommand = new ImageCommand(this);
            RightClickCommand = new RelayCommand((s) => RemoveElement(s), (s) => true);

            _EBoardViewModel = eBoardViewModel;

            _EID = elementDataSet.EID;

            X = elementDataSet.X; Y = elementDataSet.Y; Z = elementDataSet.Z;



            if (_EID == null || _EID.Equals("-1"))
            {
                DateTime dateTime = DateTime.Now;

                _EID = $"Element_{dateTime.Ticks}";
            }


            if (elementDataSet.ElementContent != null)
            {
                if (elementDataSet.ElementContent.ContentIsUserControlAndNotShape)
                {
                    _ContentViewModel = new ContentViewModel(elementDataSet);
                    IsContent = true;

                    OnPropertyChanged(nameof(ContentViewModel));
                }
                else
                {
                    _ShapeViewModel = new ShapeViewModel(elementDataSet);
                    IsShape = true;

                    OnPropertyChanged(nameof(ShapeViewModel));
                }
            }
        }


        // Methods
        #region Methods


        private void RemoveElement(object s)
        {
            _EBoardViewModel.Elements.Remove(this);
        }

        #endregion

    }
}
// EOF