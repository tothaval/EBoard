/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardInitializationManager 
 * 
 *  helper class for loading and deserialization of EBoardConfig, EBoardDataSet(s) and ElementDataSet(s)
 *  on program startup
 *  
 *  steps:
 *  1. load EBoardConfig
 *  2. load EBoardDataSet(s)
 *  3. for each EBoardDataSet load ElementDataSet(s)
 */

using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace EBoard.IOProcesses
{
    internal class EBoardInitializationManager
    {

        private readonly EBoardBrowserViewModel _EBoardBrowserViewModel;

        private readonly IOProcessesInitializationManager _IOProcessesInitializationManager;


        public EBoardInitializationManager(IOProcessesInitializationManager iOProcesses, MainViewModel mainViewModel)
        {
            _IOProcessesInitializationManager = iOProcesses;
            _EBoardBrowserViewModel = mainViewModel.EBoardBrowserViewModel;
        }


        private async Task DeserializeEBoardDataSets(string folderPath)
        {
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folderPath, filter, SearchOption.TopDirectoryOnly).ToList();

            if (files != null && files.Count > 0)
            {
                foreach (string file in files)
                {
                    if (file.EndsWith($"eboard_{files.IndexOf(file)}.xml"))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(EboardDataSet));

                        using (var reader = new StreamReader(file))
                        {
                            try
                            {
                                var member = (EboardDataSet)xmlSerializer.Deserialize(reader);

                                if (member != null)
                                {
                                    var xmlSerializer_ElementDataSet = new XmlSerializer(typeof(ElementDataSet));

                                    List<string> elements = Directory.GetFiles($"{folderPath}eboard_{files.IndexOf(file)}", filter, SearchOption.TopDirectoryOnly).ToList();

                                    for (int i = 0; i < elements.Count; i++)
                                    {
                                        if (elements[i].EndsWith($"element_{i}.xml"))
                                        {
                                            var reader_elements = new StreamReader(elements[i]);

                                            var elementData = (ElementDataSet)xmlSerializer_ElementDataSet.Deserialize(reader_elements);

                                            if (elementData != null)
                                            {

                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ContainerManagement"))
                                                {
                                                    elementData.ElementContent = new ContainerManagement(new TextBox());                                                    
                                                }

                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ShapeManagement"))
                                                {
                                                    elementData.ElementContent = new ShapeManagement();
                                                    await elementData.ElementContent.Load($"{folderPath}eboard_{files.IndexOf(file)}\\element_{i}\\", elementData);
                                                }
                                                                                                                                  

                                                //Type? type = Type.GetType($"AidingElementsUserInterface.Elements.{node_Type.InnerText}, AidingElementsUserInterface");

                                                //Type? type = Type.GetType(elementData.ElementTypeString);
                                                //liefern beide null, weil Type nicht gefunden wird.
                                                //muss wahrscheinlich die assembly dazu. beim nächsten mal gehts weiter.
                                                //UserControl userControl = (UserControl)Activator.CreateInstance(type);

                                                member.EBoardViewModel.Elements.Add(
                                                    new ElementViewModel(
                                                        member.EBoardViewModel,
                                                        elementData
                                                        )
                                                    );
                                            }
                                        }
                                    }

                                    await _EBoardBrowserViewModel.InstantiateEBoardDataSet(member);
                                }

                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }
        }


        public async Task LoadEBoardDataSets()
        {
            await DeserializeEBoardDataSets(_IOProcessesInitializationManager.EBoardFolder);
        }
    }
}
// EOF