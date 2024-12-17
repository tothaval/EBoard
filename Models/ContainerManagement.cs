/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContainerManagement 
 * 
 *  data model for all eboard container elements except shapes
 *  all allowed data operations of an element have to be done using this class
 *  
 *  should i use an interface?
 *  
 *  shapes and usercontrols sind in wpf getrennt, im Falle einer Änderung müsste jedes
 *  Element untersucht und einem der beiden Manager übergeben werden  
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
using System.Xml.Serialization;

namespace EBoard.Models
{
    internal class ContainerManagement : IElementContent
    {
        public bool ContentIsUserControlAndNotShape => true;

        private FrameworkElement _Element;
        public FrameworkElement Element => _Element;


        public ContainerManagement(FrameworkElement element)
        {
            _Element = element;
        }

        public ContainerManagement(ElementDataSet elementDataSet)
        {
        }


        public async Task Load(string path, ElementDataSet elementDataSet)
        {

            await Task.CompletedTask;
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


            // serialize content
            //var xmlSerializer_ContainerContentDataSet = new XmlSerializer(typeof(ContainerDataSet));

            //ContainerDataSet containerDataSet = new ContainerDataSet(elementDataSet);

            //await using (var writer = new StreamWriter(path))
            //{
            //    xmlSerializer_ContainerContentDataSet.Serialize(writer, containerDataSet);
            //}

            await Task.CompletedTask;
        }


    }
}
