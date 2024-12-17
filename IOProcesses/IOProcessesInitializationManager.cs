/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IOProcessesInitializationManager 
 * 
 *  helper class for checking, if needed files and folders do exist and to create them if not.
 *  
 *  steps:
 *  1. check folders, create if necessary
 *  2. check files, create if necessary
 *  3. return true to caller once finished
 */
using System.Diagnostics;
using System.IO;

namespace EBoard.IOProcesses
{
    internal class IOProcessesInitializationManager
    {
        public string EBoardConfigFolder { get; set; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\eboardconfig\\";

        public string EBoardFolder { get; set; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\eboards\\";



        public IOProcessesInitializationManager()
        {
            CheckEBoardConfigFolder();
            CheckEBoardFolder();
                

        }


        // async methods
        #region async methods

        public Task CleanFolder()
        {
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(EBoardFolder, filter, SearchOption.TopDirectoryOnly).ToList();            

            if (files.Count > 0)
            {
                foreach (string file in files)
                {
                    File.Delete(file);                    
                }
            }

            List<string> folders = Directory.GetDirectories(EBoardFolder).ToList();

            if (folders.Count > 0)
            {
                foreach (string folder in folders)
                {
                    Directory.Delete(folder, true);
                }
            }

            return Task.CompletedTask;
        }            

        #endregion async methods


        // methods
        #region methods

        private void CheckEBoardConfigFolder()
        {
            if (!Directory.Exists(EBoardConfigFolder))
            {
                Directory.CreateDirectory(EBoardConfigFolder);
            }
        }

        private void CheckEBoardFolder()
        {
            if (!Directory.Exists(EBoardFolder))
            {
                Directory.CreateDirectory(EBoardFolder);
            }
        } 

        #endregion


    }
}
// EOF