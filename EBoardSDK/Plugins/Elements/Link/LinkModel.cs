// <copyright file="LinkModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Elements.Link;

[Serializable]
public class LinkModel
{
    public string LinkTargetName { get; set; }

    public string LinkTargetPath { get; set; }

    public LinkModel()
    {
    }

    public LinkModel(LinkViewModel linkViewModel)
    {
        this.LinkTargetName = linkViewModel.LinkTargetName ?? string.Empty;
        this.LinkTargetPath = linkViewModel.LinkTargetPath ?? string.Empty;
    }
}

// EOF