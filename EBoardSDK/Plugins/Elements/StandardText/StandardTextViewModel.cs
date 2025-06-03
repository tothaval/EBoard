using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using EBoardSDK.SharedMethods;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EBoardSDK.Plugins.Elements.StandardText
{
    public partial class StandardTextViewModel : EBoardElementPluginBaseViewModel
    {
        [ObservableProperty]
        private int fontSize;

        [ObservableProperty]
        private int fontSizeTitle;

        [ObservableProperty]
        private bool isTitleSet;

        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private Brush titleTextBoxBorderBrush;

        [ObservableProperty]
        private int titleTextBoxBorderThickness;

        [ObservableProperty]
        private Brush titleTextBoxBrush;

        public override bool NoDefaultBorders { get; } = false;

        public override PluginCategories PluginCategory => PluginCategories.Element;

        public override ImageBrush PluginLogo { get; set; }

        public override UserControl Plugin => (UserControl)Activator.CreateInstance(this.ElementPluginView)!;

        private string pluginHeader = "Standard Text Element";

        public override string PluginHeader { get { return this.pluginHeader; } set { this.pluginHeader = value; } }

        private string pluginName = "StandardText";

        public override string PluginName { get { return this.pluginName; } set { this.pluginName = value; } }

        public override string ElementPluginName => "StandardText";

        public override Assembly? ElementPluginAssembly => Assembly.GetAssembly(this.ElementPluginViewModel);

        public override ResourceDictionary ResourceDictionary => new();

        public override Type? ElementPluginModel => null;

        public override Type ElementPluginView => typeof(StandardTextView);

        public override Type ElementPluginViewModel => typeof(StandardTextViewModel);

        public bool IsTitleTextboxEditing => !this.IsTitleSet;

        public StandardTextViewModel() => this.InstantiateProperties();

        private void InstantiateProperties()
        {
            this.IsTitleSet = true;

            this.BorderManagement = new BorderManagement();
            this.BrushManagement = new BrushManagement();

            if (this.PluginHeader.Equals(string.Empty))
            {
                this.PluginHeader = "Standard Text";
            }

            if (string.IsNullOrWhiteSpace(this.Title))
            {
                this.Title = this.PluginHeader;
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
                    this.FontSize = data.FontSize;
                    this.FontSizeTitle = data.FontSizeTitle;
                    this.Text = data.Text;
                    this.Title = data.Title;

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

        [RelayCommand]
        private void ConfirmTitle()
        {
            this.IsTitleSet = true;

            this.TitleTextBoxBorderBrush = new SolidColorBrush(Colors.Transparent);

            this.TitleTextBoxBrush = new SolidColorBrush(Colors.Transparent);

            this.TitleTextBoxBorderThickness = 0;
        }

        [RelayCommand]
        private void SetTitle()
        {
            this.IsTitleSet = false;

            this.TitleTextBoxBorderBrush = this.BrushManagement.Foreground;
            this.TitleTextBoxBrush = this.BrushManagement.Highlight;

            this.TitleTextBoxBorderThickness = 2;
        }
    }
}

// EOF