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

namespace EBoardSDK.Plugins.Elements.StandardText
{
    public partial class StandardTextViewModel : ObservableObject, IPlugin, IPluginData
    {
        [ObservableProperty]
        private string text;


        public PluginDataSet PluginDataSet { get; set; } = new PluginDataSet();



        public BorderManagement BorderManagement { get; set; }


        public BrushManagement BrushManagement { get; set; }


        // !!!! prüfen ob sinnvoll und relevant, ggf. ersetzen
        // später ggf. per Factory oder via Singleton, falls nötig
        // ist für die option, im load des programms den view typ sauber
        // instanzieren zu können.
        private StandardTextView plugin = new StandardTextView();
        public UserControl Plugin => plugin;


        [ObservableProperty]
        private CornerRadius cornerRadius;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private int cornerRadiusValue;


        partial void OnCornerRadiusValueChanged(int value)
        {
            BorderManagement.CornerRadius = new CornerRadius(value);
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double height;

        partial void OnHeightChanged(double value)
        {
            BorderManagement.Height = value;
        }


        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BorderManagement))]
        private double width;

        partial void OnWidthChanged(double value)
        {
            BorderManagement.Width = value;
        }

        [ObservableProperty]
        private string pluginHeader = "Standard Text";


        [ObservableProperty]
        private string pluginName = "StandardText";


        public StandardTextViewModel() => InstantiateProperties();


        public bool ApplyBackgroundBrush(Brush brush)
        {
            try
            {
                BrushManagement.Background = brush;

                OnPropertyChanged(nameof(BrushManagement));

                OnPropertyChanged(nameof(BrushManagement.Background));

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }


        private void InstantiateProperties()
        {
            BorderManagement = new BorderManagement();
            BrushManagement = new BrushManagement();

            if (PluginHeader.Equals(string.Empty))
            {
                PluginHeader = "Standard Text";
            }
        }


        public Task Load(string path, IElementDataSet elementDataSet)
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


        public async Task Save(string path, IElementDataSet elementDataSet)
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


        public bool SelectionChange(bool isSelected)
        {
            if (isSelected)
            {
                BrushManagement.SwitchBorderToHighlight();

                OnPropertyChanged(nameof(BrushManagement));

                return true;
            }

            BrushManagement.SwitchBorderToBorder();

            OnPropertyChanged(nameof(BrushManagement));

            return false;
        }


    }
}
// EOF