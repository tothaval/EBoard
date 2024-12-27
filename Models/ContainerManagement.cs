/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContainerManagement 
 * 
 *  data model for all eboard container elements that are based on usercontrols.
 *  all allowed data operations of an element have to be done using this class.
 */

using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace EBoard.Models
{
    internal class ContainerManagement : IElementContent
    {
        public bool ContentIsUserControlAndNotShape => true;

        private FrameworkElement _Element;
        public FrameworkElement Element => _Element;

        public ObservableCollection<string> ContentDataStrings { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<double> ContentDataValues { get; set; } = new ObservableCollection<double>();


        public ContainerManagement(FrameworkElement element)
        {
            _Element = element;
        }

        public ContainerManagement()
        {
        }


        public async Task Load(string path, ElementDataSet elementDataSet)
        {

            string contentDataPath = $"{path}containerdata.xml";
            var xmlSerializer = new XmlSerializer(typeof(ContainerDataSet));
            var reader = new StreamReader(contentDataPath);

            var containerData = (ContainerDataSet)xmlSerializer.Deserialize(reader);

            //Type? type = Type.GetType($"AidingElementsUserInterface.Elements.{node_Type.InnerText}, AidingElementsUserInterface");

            //Type? type = Type.GetType(elementData.ElementTypeString);
            //liefern beide null, weil Type nicht gefunden wird.
            //muss wahrscheinlich die assembly dazu. beim nächsten mal gehts weiter.
            //UserControl userControl = (UserControl)Activator.CreateInstance(type);


            if (containerData != null)
            {

                //Type? type = Type.GetType(containerData.UserControlType);

                // Frage: generisches Instanzieren überhaupt zweckmäßig... hm, wenn externe elemente
                // eingebunden werden sollen, dann schon. also müssten diese das Interface implementieren,
                // dieser Typ wird gespeichert, es wird ein string der jeweiligen Assembly gespeichert
                // diese Elemente dann mit Activator.CreateInstance arbeiten und per Interfacemethode
                // dann containerData übergeben

                if (containerData.UserControlType.Equals("System.Windows.Controls.TextBox"))
                {
                    if (containerData.ContentDataStrings.Count > 0)
                    {
                        _Element = new TextBox()
                        {
                            Text = containerData.ContentDataStrings[0],
                            AcceptsReturn = true,
                            AcceptsTab = true,
                            TextWrapping = TextWrapping.Wrap
                        };
                    }
                } 
            }

            await Task.CompletedTask;


            return;


        }


        public ObservableCollection<string> GetStringValues()
        {

            //intermediate solution
            if (Element.GetType().Name.Equals("TextBox"))
            {
                ContentDataStrings.Add(((TextBox)Element).Text);
            }



            return ContentDataStrings;
        }


        public async Task Save(string path, ElementDataSet elementDataSet)
        {
            string containerDataPath = $"{path}containerdata.xml";

            // serialize content
            var xmlSerializer_ContainerDataSet = new XmlSerializer(typeof(ContainerDataSet));

            ContainerDataSet containerDataSet = new ContainerDataSet(elementDataSet);

            await using (var writer = new StreamWriter(containerDataPath))
            {
                xmlSerializer_ContainerDataSet.Serialize(writer, containerDataSet);
            }

            await Task.CompletedTask;
        }


    }
}
// EOF