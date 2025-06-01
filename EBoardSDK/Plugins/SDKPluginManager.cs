namespace EBoardSDK.Plugins;

using EBoardSDK.Plugins.Addons.SoundMix;
using EBoardSDK.Plugins.Elements.EmptyLinear;
using EBoardSDK.Plugins.Elements.EmptyRadial;
using EBoardSDK.Plugins.Elements.Image;
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
        new ImageViewModel(),
        new StandardTextViewModel(),

        new EllipseViewModel(),
        new RectangleViewModel(),

        new SummonerViewModel(),
        new UptimeViewModel(),
        ];
}
