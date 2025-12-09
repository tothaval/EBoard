// <copyright file="TwoXThreeImageAreaModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;

using EBoardSDK.Plugins.Elements.StandardText;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class TwoXThreeImageAreaModel
{
    [XmlIgnore]
    private readonly TwoXThreeImageAreaViewModel twoXThreeImageAreaViewModel;

    public string Link1 { get; set; } = string.Empty;

    public string Link2 { get; set; } = string.Empty;

    public string Link3 { get; set; } = string.Empty;

    public string Link4 { get; set; } = string.Empty;

    public string Link5 { get; set; } = string.Empty;

    public string Link6 { get; set; } = string.Empty;

    public TwoXThreeImageAreaModel()
    {
    }

    public TwoXThreeImageAreaModel(TwoXThreeImageAreaViewModel twoXThreeImageAreaViewModel)
    {
        this.twoXThreeImageAreaViewModel = twoXThreeImageAreaViewModel;

        this.Link1 = twoXThreeImageAreaViewModel.ImageViewModel1.LinkTargetPath ?? string.Empty;
        this.Link2 = twoXThreeImageAreaViewModel.ImageViewModel2.LinkTargetPath ?? string.Empty;
        this.Link3 = twoXThreeImageAreaViewModel.ImageViewModel3.LinkTargetPath ?? string.Empty;
        this.Link4 = twoXThreeImageAreaViewModel.ImageViewModel4.LinkTargetPath ?? string.Empty;
        this.Link5 = twoXThreeImageAreaViewModel.ImageViewModel5.LinkTargetPath ?? string.Empty;
        this.Link6 = twoXThreeImageAreaViewModel.ImageViewModel6.LinkTargetPath ?? string.Empty;
    }
}

// EOF