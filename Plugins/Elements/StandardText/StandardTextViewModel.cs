using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.Models;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace EBoard.Plugins.Elements.StandardText
{
    public partial class StandardTextViewModel : PluginViewModel
    {
        [ObservableProperty]
        private string text;


        public StandardTextViewModel()
        {
            if (PluginHeader.Equals(string.Empty))
            {
                PluginHeader = "Standard Text";
            }
        }
               

        public override Task<bool> Load(string path, IPluginDataSet pluginDataSet)
        {
            PluginDataSet = pluginDataSet;

            PluginData pluginDataText = PluginDataSet.PluginDataStorage.Find(x => x.Key.Equals("Text"));

            Text = pluginDataText.Value;

            PluginData pluginDataPath = PluginDataSet.PluginDataStorage.Find(x => x.Key.Equals("Path"));

            BorderManagement = new BorderManagement(pluginDataSet.BorderDataSet);
            BrushManagement = new BrushManagement(pluginDataSet.BrushDataSet);

            PluginHeader = PluginDataSet.PluginHeader;
            PluginName = PluginDataSet.PluginName;            

            // just for demo purposes, more complex data could be stored in the content folder for
            // the element or it could store the path to the storage and retrieve it this way.
            // in case of this yet still simple plugin, it is not necessary, although it would be
            // better, if content was saved elsewhere. in case element folders got deleted, the
            // plugin content could still exist. 
            string contentFolderPath = pluginDataPath.Value;


            return Task.FromResult(true);

        }


        public override async Task<bool> OnEboardShutdown(string path)
        {
            PluginDataSet = new PluginDataSet(this);
            
            PluginDataSet.AddPluginData(new PluginData(){ Key = "Path", Value = path});
       
            PluginDataSet.AddPluginData(new PluginData() { Key = "Text", Value = Text });

            await Task.CompletedTask;

            return true;
        }


    }
}
// EOF