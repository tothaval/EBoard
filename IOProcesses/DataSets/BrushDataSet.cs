using EBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets
{
    [Serializable]
    public class BrushDataSet
    {
        [XmlIgnore]
        private BrushManagement _BrushManagement;

        public ColorDataSet BackgroundColor { get; set; }

        public ColorDataSet ForegroundColor { get; set; }


        public ColorDataSet BorderColor { get; set; }


        public ColorDataSet HighlightColor { get; set; }


        public Thickness BorderThickness { get; set; }


        public BrushDataSet()
        {

        }


        public BrushDataSet(BrushManagement brushManagement)
        {
            _BrushManagement = brushManagement;

            if (_BrushManagement == null)
            {
                _BrushManagement = new BrushManagement();
            }

            BackgroundColor = new ColorDataSet(_BrushManagement.ElementBackground);
            ForegroundColor = new ColorDataSet(_BrushManagement.ElementForeground);
            BorderColor = new ColorDataSet(_BrushManagement.ElementBorder);
            HighlightColor = new ColorDataSet(_BrushManagement.ElementHighlight);

            BorderThickness = _BrushManagement.ElementBorderThickness;
        }


    }
}
