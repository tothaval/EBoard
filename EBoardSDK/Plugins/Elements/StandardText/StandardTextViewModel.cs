using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using EBoardSDK.Plugins.Elements.StandardText;

using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;

using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using EBoardSDK.SharedMethods;
using static System.Net.Mime.MediaTypeNames;
using EBoardSDK.Plugins.Tools.EmptyRadial;
using System.Reflection;

namespace EBoardSDK.Plugins.Elements.StandardText
{
    public partial class StandardTextViewModel : EBoardElementPluginBaseViewModel
    {
        [ObservableProperty]
        private string text;

        public override UserControl Plugin => (UserControl)Activator.CreateInstance(ElementPluginView)!;

        private string pluginHeader = "Standard Text Element";
        public override string PluginHeader { get { return pluginHeader; } set { pluginHeader = value; } }

        private string pluginName = "StandardText";

        public override string PluginName { get { return pluginName; } set { pluginName = value; } }

        public override string ElementPluginName => "StandardText";

        public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

        public override ResourceDictionary ResourceDictionary => new();

        public override Type? ElementPluginModel => null;

        public override Type ElementPluginView => typeof(EmptyRadialView);

        public override Type ElementPluginViewModel => typeof(EmptyRadialViewModel);

        public StandardTextViewModel() => InstantiateProperties();

        private void InstantiateProperties()
        {
            BorderManagement = new BorderManagement();
            BrushManagement = new BrushManagement();

            if (this.PluginHeader.Equals(string.Empty))
            {
                PluginHeader = "Standard Text";
            }
        }

        public override async Task<EBoardFeedbackMessage> Load(string path)
        {
            if (new DirectoryInfo(path).Exists)
            {
                string contentfilename = "content.xml";

                path = Path.Combine(path, contentfilename);
            }

            try
            {
                var data = await new SharedMethod_Plugins().DeserializeConfigFiles<StandardTextModel>(path)!;

                if (data != null)
                {
                    Text = data.Text;

                    return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Success, ResultMessage = $"deserialized {path}" };
                }
            }
            catch (Exception ex)
            {
                return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Exception, ResultMessage = ex.Message };
            }

            return new EBoardFeedbackMessage() { TaskResult = EBoardTaskResult.Unknown, ResultMessage = "" };
        }


        public override async Task<EBoardFeedbackMessage> Save(string path)
        {
            string contentDataPath = $"{path}content.xml";
            var standardTextModel = new StandardTextModel(this);

            EBoardFeedbackMessage? serializationResult = null;

            if (new DirectoryInfo(path).Exists)
            {
                path = Path.Combine(path, contentDataPath);
            }

            serializationResult = await new SharedMethod_Plugins().SerializeConfigFiles(standardTextModel, path);

            if (!serializationResult.TaskResult.Equals(EBoardTaskResult.Success))
            {
                // TODO do stuff
            }

            return serializationResult!;
        }

    }
}
// EOF