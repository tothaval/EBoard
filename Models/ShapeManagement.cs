/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ShapeManagement 
 * 
 *  data model for shape elements
 *  
 *  they are problematic in regards of property changes, because they are handled
 *  differently by the WPF framework compared to UserControls, so right now there
 *  is a bit of code duplication, which shall be refactored later in development
 */
using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Xml.Serialization;

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


        public async Task Load(string path, ElementDataSet elementDataSet)
        {
            
            string shapeDataPath = $"{path}shapedata.xml";
            var xmlSerializer = new XmlSerializer(typeof(ShapeDataSet));
            var reader = new StreamReader(shapeDataPath);

            var shapeData = (ShapeDataSet)xmlSerializer.Deserialize(reader);



            if (shapeData.ShapeType.Equals("System.Windows.Shapes.Rectangle"))
            {
                _Element = new Rectangle()
                {
                    Width = elementDataSet.BorderDataSet.Width,
                    Height = elementDataSet.BorderDataSet.Height,
                    Fill = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    Stroke = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    StrokeThickness = elementDataSet.BorderDataSet.BorderThickness.Left
                };
            }

            if (shapeData.ShapeType.Equals("System.Windows.Shapes.Ellipse"))
            {
                _Element = new Ellipse()
                {
                    Width = elementDataSet.BorderDataSet.Width,
                    Height = elementDataSet.BorderDataSet.Height,
                    Fill = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    Stroke = await elementDataSet.BrushDataSet.BackgroundColor.GetBrush(),
                    StrokeThickness = elementDataSet.BorderDataSet.BorderThickness.Left
                };
            }

            return;
        }


        public async Task Save(string path, ElementDataSet elementDataSet)
        {
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
// EOF