/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ElementDataSet 
 * 
 *  serializable helper class to store and retrieve ElementViewModel(s) data to
 *  or from hard drive storage.
 *  
 *  Each Eboard has to store its own properties, as well as a list of all child
 *  elements - for now. Once Elements are getting more complex, it is probably
 *  better to trigger a separate serialization process/method for the elements and their data.
 *  
 *  It is vital to further development efforts, to be able to save and load Eboard contents
 *  asap, because it reduces time needed to setup the ui and test a specific or recently added 
 *  feature.
 */

using EBoard.Interfaces;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Serialization;

namespace EBoard.Models
{
    [Serializable]
    [XmlRoot("ElementDataSet")]
    public class ElementDataSet
    {
        [XmlIgnore]
        private readonly EBoardViewModel _EBoardViewModel;

        [XmlIgnore]
        private ElementViewModel _ElementViewModel { get; set; }

        [XmlIgnore]
        public ElementViewModel ElementViewModel => _ElementViewModel;


        [XmlIgnore]
        private BrushManagement _BrushManager { get; set; }

        [XmlIgnore]
        public BrushManagement BrushManager => _BrushManager;


        [XmlIgnore]
        public IElementContent ElementContent { get; set; }


        /// <summary>
        /// Element ID, built using $"Element_{DateTime().Ticks} on first
        /// creation of an element.
        /// </summary>
        public string EID { get; set; }


        /// <summary>
        /// a string representation of an assembly, where the element type can be found
        /// </summary>
        public string ElementAssemblyString { get; set; }
        

        /// <summary>
        /// a string representation of an element type
        /// </summary>
        public string ElementTypeString { get; set; }


        /// <summary>
        /// the header text of an ElementView
        /// </summary>
        public string ElementHeader { get; set; }


        /// <summary>
        /// the x axis value of an elements left border
        /// </summary>
        public double X { get; set; }


        /// <summary>
        /// the y axis value of an elements top border
        /// </summary>
        public double Y { get; set; }


        /// <summary>
        /// the panel z index value of an element
        /// </summary>
        public double Z { get; set; }


        /// <summary>
        /// the x axis length of the element in pixel
        /// </summary>
        public double ElementWidth { get; set; }


        /// <summary>
        /// the y axis length of the element in pixel
        /// </summary>
        public double ElementHeight { get; set; }


        public ElementDataSet()
        {
            _EBoardViewModel = new EBoardViewModel("new", 1000.0, 500.0, 100);
    
            _ElementViewModel = new ElementViewModel(_EBoardViewModel);                       
        }


        public ElementDataSet(EBoardViewModel eBoardViewModel, ElementViewModel elementViewModel)
        {
            _EBoardViewModel = eBoardViewModel;
            _ElementViewModel = elementViewModel;         
        }

        public void AddBrushManager(BrushManagement brushManager)
        {
            _BrushManager = brushManager;
        }


        public async Task ConvertData()
        {
            EID = ElementViewModel.EID;

            if (ElementViewModel.IsContent)
            {
                ElementContent = ElementViewModel.ContentViewModel.Control;
                ElementHeader = ElementViewModel.ContentViewModel.ElementHeaderText;

                ElementTypeString = ElementContent.GetType().FullName;

                ElementWidth = ElementViewModel.ContentViewModel.ElementWidth;
                ElementHeight = ElementViewModel.ContentViewModel.ElementHeight;

                _BrushManager = ElementViewModel.ContentViewModel.BrushManager;
            }

            if (ElementViewModel.IsShape)
            {
                ElementContent = ElementViewModel.ShapeViewModel.Control;
                ElementHeader = ElementViewModel.ShapeViewModel.ElementHeaderText;

                ElementTypeString = ElementContent.GetType().FullName;

                ElementWidth = ElementViewModel.ShapeViewModel.ElementWidth;
                ElementHeight = ElementViewModel.ShapeViewModel.ElementHeight;

                _BrushManager = ElementViewModel.ShapeViewModel.BrushManager;
            }

            X = ElementViewModel.X;
            Y = ElementViewModel.Y;
            Z = ElementViewModel.Z;


            await Task.CompletedTask;
        }


    }
}
// EOF