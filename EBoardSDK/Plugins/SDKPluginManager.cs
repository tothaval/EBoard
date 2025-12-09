// <copyright file="SDKPluginManager.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Plugins;

using EBoardSDK.Plugins.Addons.SoundMix;
using EBoardSDK.Plugins.Areas.FileLinkArea;
using EBoardSDK.Plugins.Areas.TwoXThreeVImageArea;
using EBoardSDK.Plugins.Elements.BasicAV;
using EBoardSDK.Plugins.Elements.EmptyLinear;
using EBoardSDK.Plugins.Elements.EmptyRadial;
using EBoardSDK.Plugins.Elements.Gold;
using EBoardSDK.Plugins.Elements.Image;
using EBoardSDK.Plugins.Elements.Link;
using EBoardSDK.Plugins.Elements.StandardText;
using EBoardSDK.Plugins.Shapes.Ellipse;
using EBoardSDK.Plugins.Shapes.Rectangle;
using EBoardSDK.Plugins.Tools.Summoner;
using EBoardSDK.Plugins.Tools.Uptime;

public static class SDKPluginManager
{
    public static IList<EBoardElementPluginBaseViewModel> SDKPlugins => [

        new SoundMixMainViewModel(),

        new EmptyLinearViewModel(),
        new EmptyRadialViewModel(),
        new GoldViewModel(),
        new ImageViewModel(),
        new LinkViewModel(),
        new StandardTextViewModel(),
        new BasicAVMainViewModel(),

        new EllipseViewModel(),
        new RectangleViewModel(),

        new FileLinkAreaViewModel(),
        new TwoXThreeImageAreaViewModel(),

        new SummonerViewModel(),
        new UptimeViewModel(),
        ];
}
