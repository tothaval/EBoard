// <copyright file="StandardTextModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Elements.StandardText;

using System.Xml.Serialization;

[Serializable]
public class StandardTextModel
{
    [XmlIgnore]
    private StandardTextViewModel standardTextViewModel;

    public int FontSize { get; set; } = 10;

    public int FontSizeTitle { get; set; } = 20;

    public string Text { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public StandardTextModel()
    {
    }

    public StandardTextModel(StandardTextViewModel standardTextViewModel)
    {
        this.standardTextViewModel = standardTextViewModel;

        this.FontSize = standardTextViewModel.FontSize;
        this.FontSizeTitle = standardTextViewModel.FontSizeTitle;
        this.Text = standardTextViewModel.Text;
        this.Title = standardTextViewModel.Title;
    }
}
