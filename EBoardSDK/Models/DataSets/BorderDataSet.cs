// <copyright file="BorderDataSet.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BorderDataSet
 *
 *  serializable helper class to store and retrieve Border related data to
 *  or from hard drive storage.
 */
using System.Windows;
using System.Xml.Serialization;

namespace EBoardSDK.Models.DataSets;

[Serializable]
public class BorderDataSet
{
    [XmlIgnore]
    private BorderManagement _BorderManagement = new BorderManagement();

    public Thickness BorderThickness { get; set; }

    public CornerRadius CornerRadius { get; set; }

    public double Height { get; set; }

    public Thickness Margin { get; set; }

    public Thickness Padding { get; set; }

    public double Width { get; set; }

    public BorderDataSet()
    {
    }

    public BorderDataSet(BorderManagement borderManagement)
    {
        this._BorderManagement = borderManagement;

        if (this._BorderManagement == null)
        {
            this._BorderManagement = new BorderManagement();
        }

        this.BorderThickness = this._BorderManagement.BorderThickness;
        this.CornerRadius = this._BorderManagement.CornerRadius;
        this.Height = this._BorderManagement.Height;
        this.Margin = this._BorderManagement.Margin;
        this.Padding = this._BorderManagement.Padding;
        this.Width = this._BorderManagement.Width;
    }
}

// EOF