namespace EBoardSDK.Plugins.Elements.StandardText;

using System.Xml.Serialization;

[Serializable]
public class StandardTextModel
{
    [XmlIgnore]
    private StandardTextViewModel standardTextViewModel;

    public string Text { get; set; }

    public StandardTextModel()
    {

    }

    public StandardTextModel(StandardTextViewModel standardTextViewModel)
    {
        this.standardTextViewModel = standardTextViewModel;

        Text = standardTextViewModel.Text;
    }
}
