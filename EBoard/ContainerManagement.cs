/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ContainerManagement 
 * 
 *  data model for all eboard container elements that are based on usercontrols.
 *  all allowed data operations of an element have to be done using this class.
 */

using EBoard.IOProcesses.DataSets;

using EBoardSDK.SharedMethods;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;

using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace EBoard.Models;

internal class ContainerManagement : IElementContent
{
    public bool ContentIsUserControlAndNotShape => true;

    private FrameworkElement _Plugin;
    public FrameworkElement Plugin => _Plugin;

    public ObservableCollection<string> ContentDataStrings { get; set; } = new ObservableCollection<string>();

    public ObservableCollection<double> ContentDataValues { get; set; } = new ObservableCollection<double>();


    public ContainerManagement() { } 


    public ContainerManagement(FrameworkElement plugin) => _Plugin = plugin;



    public async Task Load(string path, IElementDataSet elementDataSet)
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


            Type? type = Type.GetType(containerData.UserControlType);

            UserControl userControl = (UserControl)Activator.CreateInstance(type);

            if (userControl is null)
            {
                _Plugin = new TextBlock()
                {
                    Text = "Error: element type loading failed",
                    Background = new SolidColorBrush(Colors.Black),
                    Foreground = new SolidColorBrush(Colors.White)

                };
            }
            else
            {
                string viewmodelName = string.Concat(containerData.UserControlType, "Model");

                Type? viewModelType = Type.GetType(viewmodelName);

                if (viewModelType is not null)
                {
                    var viewModel = Activator.CreateInstance(viewModelType);

                    userControl.DataContext = viewModel;

                    if (viewModel.GetType().GetInterfaces().Contains(typeof(IElementContentSaveAndLoad)))
                    {
                        await ((IElementContentSaveAndLoad)viewModel).Load(path, elementDataSet);
                    }

                }

                _Plugin = userControl;
            }

        }

        await Task.CompletedTask;

        return;
    }


    public ObservableCollection<string> GetStringValues()
    {
        //intermediate solution
        if (Plugin.GetType().Name.Equals("TextBox"))
        {
            ContentDataStrings.Add(((TextBox)Plugin).Text);
        }

        return ContentDataStrings;
    }


    public async Task Save(string path, IElementDataSet elementDataSet)
    {
        string containerDataPath = $"{path}containerdata.xml";

        // serialize content
        var xmlSerializer_ContainerDataSet = new XmlSerializer(typeof(ContainerDataSet));

        ContainerDataSet containerDataSet = new ContainerDataSet(elementDataSet);

        await using (var writer = new StreamWriter(containerDataPath))
        {
            xmlSerializer_ContainerDataSet.Serialize(writer, containerDataSet);
        }


        var viewModel = elementDataSet.Plugin.Plugin.DataContext;

        var interfaces = viewModel.GetType().GetInterfaces();

        if (interfaces.Contains(typeof(IElementContentSaveAndLoad)))
        {
            await ((IElementContentSaveAndLoad)viewModel).Save(path, elementDataSet);            
        }
        
        await Task.CompletedTask;
    }


}
// EOF