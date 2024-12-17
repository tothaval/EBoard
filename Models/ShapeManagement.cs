/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeManagement 
 * 
 *  data model for shape elements - oder shape element hier so definieren, dass es immer
 *  den hintergrund der usercontrol kriegt. da müssten die daten von der usercontrol hier
 *  transformiert werden in die neuen shape werte
 *  
 *  should i use an interface or abstract base class to further channel the logic?
 */
using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace EBoard.Models
{
    internal class ShapeManagement : IElementContent
    {
        public bool ContentIsUserControlAndNotShape => false;

        private FrameworkElement _Element;
        public FrameworkElement Element => _Element;

        public ShapeManagement()
        {
            
        }

        public ShapeManagement(Shape content)
        {
            _Element = content;
        }


        public Task Load(string path, ElementDataSet elementDataSet)
        {
            
            string shapeDataPath = $"{path}shapedata.xml";
            var xmlSerializer = new XmlSerializer(typeof(ShapeDataSet));
            var reader = new StreamReader(shapeDataPath);

            var shapeData = (ShapeDataSet)xmlSerializer.Deserialize(reader);



            if (shapeData.ShapeType.Equals("System.Windows.Shapes.Rectangle"))
            {
                _Element = new Rectangle()
                {
                    Width = elementDataSet.ElementWidth,
                    Height = elementDataSet.ElementHeight,
                    Fill = elementDataSet.BrushManager.ElementBackground,
                    Stroke = elementDataSet.BrushManager.ElementBorder,
                    StrokeThickness = elementDataSet.BrushManager.ElementBorderThickness.Left
                };
            }

            if (shapeData.ShapeType.Equals("System.Windows.Shapes.Ellipse"))
            {
                _Element = new Ellipse()
                {
                    Width = elementDataSet.ElementWidth,
                    Height = elementDataSet.ElementHeight,
                    Fill = elementDataSet.BrushManager.ElementBackground,
                    Stroke = elementDataSet.BrushManager.ElementBorder,
                    StrokeThickness = elementDataSet.BrushManager.ElementBorderThickness.Left
                };
            }


            return Task.CompletedTask;
        }


        public async Task Save(string path, ElementDataSet elementDataSet)
        {

            string brushDataPath = $"{path}brushdata.xml";

            // serialize brushes
            var xmlSerializer_BrushDataSet = new XmlSerializer(typeof(BrushDataSet));

            BrushDataSet brushDataSet = new BrushDataSet(elementDataSet.BrushManager);

            await using (var writer = new StreamWriter(brushDataPath))
            {
                xmlSerializer_BrushDataSet.Serialize(writer, brushDataSet);
            }


            string shapeDataPath = $"{path}shapedata.xml";

            // serialize content
            var xmlSerializer_ShapeDataSet = new XmlSerializer(typeof(ShapeDataSet));

            ShapeDataSet shapeDataSet = new ShapeDataSet(elementDataSet);

            await using (var writer = new StreamWriter(shapeDataPath))
            {
                xmlSerializer_ShapeDataSet.Serialize(writer, shapeDataSet);
            }

            await Task.CompletedTask;
        }
    }
}
