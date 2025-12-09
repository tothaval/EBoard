// <copyright file="FileLinkAreaModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Areas.FileLinkArea;

using EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;
using EBoardSDK.Plugins.Elements.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class FileLinkAreaModel
{
    [XmlIgnore]
    private readonly FileLinkAreaViewModel fileLinkAreaViewModel;

    public List<List<LinkModel>> Links { get; set; }

    public FileLinkAreaModel()
    {
    }

    public FileLinkAreaModel(FileLinkAreaViewModel fileLinkAreaViewModel)
    {
        this.fileLinkAreaViewModel = fileLinkAreaViewModel;
        this.Links = new();

        var areavm = fileLinkAreaViewModel.AreaViewModel;

        foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        {
            var list = new List<LinkModel>();

            foreach (var horizontalelement in item.Elements)
            {
                var linkmodel = new LinkModel(horizontalelement);

                list.Add(linkmodel);
            }

            this.Links.Add(list);
        }
    }
}

// EOF