using EBoard.IOProcesses.DataSets;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EBoard.IOProcesses
{
    class EBoardConfigLoader
    {
        private readonly IOProcessesInitializationManager _IOProcessesInitializationManager;


        private EboardConfig _EBoardConfig;
        public EboardConfig EBoardConfig => _EBoardConfig;


        public EBoardConfigLoader(IOProcessesInitializationManager iOProcesses)
        {
            _IOProcessesInitializationManager = iOProcesses;
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
                            }

                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        public async Task<EboardConfig> LoadEBoardConfig()
        {
            await DeserializeEBoardConfig(_IOProcessesInitializationManager.EBoardConfigFolder);

            return EBoardConfig;
        }

    }
}
