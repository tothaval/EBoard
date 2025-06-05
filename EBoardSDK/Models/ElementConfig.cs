// <copyright file="ElementConfig.cs" company=".">
// Stephan Kammel
// </copyright>

using EBoardSDK.Interfaces;
using EBoardSDK.Models.DataSets;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace EBoardSDK.Models
{
    public class ElementConfig
    {
        /// <summary>
        /// Gets or sets element ID, built using $"Element_{DateTime().Ticks} on first
        /// creation of an element.
        /// </summary>
        public string EID { get; set; } = string.Empty;

        public int ID { get; set; } = -1;

        public BorderDataSet BorderDataSet { get; set; } = new(new BorderManagement());

        public BrushDataSet BrushDataSet { get; set; } = new(new BrushManagement());

        public PlacementDataSet PlacementDataSet { get; set; } = new(new PlacementManagement());

        [JsonIgnore]
        [XmlIgnore]
        public IPlugin Plugin { get; set; }

        /// <summary>
        /// Gets or sets determines if ElementContent is
        /// of type ShapeManagement(false)
        /// or ContentManagement(true)
        /// </summary>
        //public bool IsContentNotShape { get; set; }

        /// <summary>
        /// a string representation of an assembly, where the element type can be found
        /// </summary>
        public string PluginHeader { get; set; } = string.Empty;

        // unification effort due to reduction, build abstraction layer can be removed and simplified
        // loading can be simplified, but element as plugin container needs some rework
        // every plugin can implement certain features, and maybe should be required to do so.
        public string PluginName { get; set; } = string.Empty;

        public string PluginType { get; set; } = string.Empty;

        public string AssemblyName { get; set; } = string.Empty;
    }
}
