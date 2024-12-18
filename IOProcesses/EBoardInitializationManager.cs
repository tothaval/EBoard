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
        //private readonly MainViewModel _MainViewModel;
        private EboardConfig _EBoardConfig;
        public EboardConfig EBoardConfig => _EBoardConfig;


        public EBoardInitializationManager(IOProcessesInitializationManager iOProcesses, MainViewModel mainViewModel)
        {
            _EBoardBrowserViewModel = mainViewModel.EBoardBrowserViewModel;
            _IOProcessesInitializationManager = iOProcesses;
            //_MainViewModel = mainViewModel;
        }


        private async Task DeserializeEBoardConfig(string folderPath)
        {
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folderPath, filter, SearchOption.TopDirectoryOnly).ToList();

            foreach (string file in files)
            {
                if (file.EndsWith("eboardconfig.xml"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(EboardConfig));

                    using (var writer = new StreamReader(file))
                    {
                        try
                        {
                            var member = (EboardConfig)xmlSerializer.Deserialize(writer);

                            if (member != null)
                            {
                                _EBoardConfig = member;
                                //_EBoardBrowserViewModel.EBoardCount = member.EBoardCount; 
                                //_EBoardBrowserViewModel.SelectedEBoard = _EBoardBrowserViewModel.EBoards[member.EBoardIndex];

                                //_MainViewModel.MainWindowMenuBarVM.EBoardBrowserSwitch = member.EBoardBrowserSwitch;
                            }

                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
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

                                                // refactor into separate method, return BrushManagement
                                                string brushDataPath = $"{folderPath}eboard_{files.IndexOf(file)}\\element_{i}\\brushdata.xml";
                                                var xmlSerializer_BrushDataSet = new XmlSerializer(typeof(BrushDataSet));
                                                var reader_brushdata = new StreamReader(brushDataPath);
                                                var brushData = (BrushDataSet)xmlSerializer_BrushDataSet.Deserialize(reader_brushdata);
                                                BrushManagement brushManagement = new BrushManagement(brushData);

                                                elementData.AddBrushManager(brushManagement);



                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ContainerManagement"))
                                                {
                                                    elementData.ElementContent = new ContainerManagement(new TextBox());

                                                    
                                                }

                                                if (elementData.ElementTypeString.Equals("EBoard.Models.ShapeManagement"))
                                                {

                                                    elementData.ElementContent = new ShapeManagement();
                                                await elementData.ElementContent.Load($"{folderPath}eboard_{files.IndexOf(file)}\\element_{i}\\", elementData);
                                                }

                                                                                   


                                                // needs a lot of optimization, but works for now
                                                // the content type should be initialized like in AEUI using some c# or wpf library
                                                // brush and text can not be changed from within eboard right now,
                                                // but the header text and x:y position can be changed
                                                // also a problem is, that ElementViewModel has no method yet, that handles content

                                                //Type? type = Type.GetType($"AidingElementsUserInterface.Elements.{node_Type.InnerText}, AidingElementsUserInterface");

                                                //Type? type = Type.GetType(elementData.ElementTypeString);
                                                //liefern beide null, weil Type nicht gefunden wird.
                                                //muss wahrscheinlich die assembly dazu. beim nächsten mal gehts weiter.
                                                //UserControl userControl = (UserControl)Activator.CreateInstance(type);


                                                // TypeCheck und Instanzierung des Contents im ElementViewModel machen
                                                // da ist es besser aufgehoben als hier

                                                // zudem ein Interface ElementContentIO erstellen, dass von Content
                                                // implementiert werden muss, damit dieser als solcher gülltig ist.

                                                // Interface soll ContentSave und ContentLoad Methoden bereitstellen,
                                                // sowie Properties für Dateipfade, Ordnernamen und -pfade etc.

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


        public async Task Load()
        {
            await DeserializeEBoardConfig(_IOProcessesInitializationManager.EBoardConfigFolder);

            await DeserializeEBoardDataSets(_IOProcessesInitializationManager.EBoardFolder);

        }

    }
}
// EOF