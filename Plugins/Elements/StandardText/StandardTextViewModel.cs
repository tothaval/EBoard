using CommunityToolkit.Mvvm.ComponentModel;
using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.Utilities.Factories;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace EBoard.Plugins.Elements.StandardText
{
    public partial class StandardTextViewModel : ObservableObject, IElementContentSaveAndLoad
    {

        [ObservableProperty]
        private string text;


        public Task Load(string path, ElementDataSet elementDataSet)
        {
            string contentDataPath = $"{path}content.xml";

            var xmlSerializer = new XmlSerializer(typeof(StandardTextModel));

            using (var reader = new StreamReader(contentDataPath))
            {
                try
                {
                    var member = (StandardTextModel)xmlSerializer.Deserialize(reader);

                    if (member != null)
                    {
                        Text = member.Text;
                    }

                    return Task.FromResult(member);
                }
                catch (Exception ex)
                {
                    throw new FileLoadException(ex.Message);
                }
            }

        }

        public async Task Save(string path, ElementDataSet elementDataSet)
        {           

            string contentDataPath = $"{path}content.xml";

            // serialize content
            var xmlSerializer = new XmlSerializer(typeof(StandardTextModel));

            StandardTextModel standardTextModel = new StandardTextModel(this);

            await using (var writer = new StreamWriter(contentDataPath))
            {
                xmlSerializer.Serialize(writer, standardTextModel);
            }

            return;
        }
    }
}
