// <copyright file="SoundMixModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins.Addons.EEP_SoundMix;

using EBoardSDK.Plugins.Addons.SoundMix;
using EBoardSDK.Plugins.Areas.FileLinkArea;
using EBoardSDK.Plugins.Elements.BasicAV;
using EBoardSDK.Plugins.Elements.Link;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

[Serializable]
public class SoundMixModel
{
    [XmlIgnore]
    private readonly SoundMixMainViewModel soundMixMainViewModel;

    public List<List<BasicAVModel>> Links { get; set; }

    public SoundMixModel()
    {
    }

    public SoundMixModel(SoundMixMainViewModel soundMixMainViewModel)
    {
        this.soundMixMainViewModel = soundMixMainViewModel;
        this.Links = new();

        var areavm = soundMixMainViewModel.AreaViewModel;

        foreach (var item in areavm.AreaVerticalOuterViewModel.HorizontalInnerViewModels)
        {
            var list = new List<BasicAVModel>();

            foreach (var horizontalelement in item.Elements)
            {
                var linkmodel = new BasicAVModel(horizontalelement);

                list.Add(linkmodel);
            }

            this.Links.Add(list);
        }
    }
}

// EOF