﻿/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  EBoardInitializationManager 
 * 
 *  helper class for loading and deserialization of EBoardDataSet(s) and ElementDataSet(s)
 *  on program startup
 *  
 *  steps:
 *  1. load EBoardDataSet(s)
 *  2. for each EBoardDataSet load ElementDataSet(s)
 */

using EBoard.Models;
using EBoard.ViewModels;
using System.IO;
using System.Windows.Controls;
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
                    if (file.Contains("eboard_") && file.EndsWith($".xml"))
                    {
                        string eboard_folder = file.Replace(".xml", "\\");


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
                                        if (elements[i].Contains("element_") && elements[i].EndsWith($".xml"))
                                        {
                                            var reader_elements = new StreamReader(elements[i]);

                                            var elementData = (ElementDataSet)xmlSerializer_ElementDataSet.Deserialize(reader_elements);

                                            if (elementData != null)
                                            {

                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ContainerManagement"))
                                                {
                                                    elementData.ElementContent = new ContainerManagement();                                                 
                                                }

                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ShapeManagement"))
                                                {
                                                    elementData.ElementContent = new ShapeManagement();                                                    
                                                }

                                                if (elementData.ElementContent != null)
                                                {
                                                    string element_folder = elements[i].Replace(".xml", "\\");

                                                    await elementData.ElementContent.Load($"{element_folder}", elementData);
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