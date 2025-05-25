using System;
using System.Collections.Generic;
namespace EBoardConfigManager.Models;

public class PresetDirectories
{
#if RELEASE

    public string DefaultConfigFolder => Path.Combine(Environment.ProcessPath, "config");

    public string DefaultPluginsFolder => Path.Combine(Environment.ProcessPath, "plugins");
    
    public string DefaultScreensFolder => Path.Combine(Environment.ProcessPath, "screens");
#endif

#if DEBUG
    public string DefaultConfigFolder => @"..\..\..\__DEBUG_config\";

    public string DefaultPluginsFolder => @"..\..\..\__DEBUG_plugins\";

    public string DefaultScreensFolder => @"..\..\..\__DEBUG_screens\";
#endif
}
