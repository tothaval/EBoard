/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BrushDataSet
 *
 *  serializable helper class to store and retrieve Brush related data to
 *  or from hard drive storage.
 */
using System.Xml.Serialization;

namespace EBoardSDK.Models.DataSets;

[Serializable]
public class BrushDataSet
{
    [XmlIgnore]
    private BrushManagement _BrushManagement = new BrushManagement();

    public ColorDataSet BackgroundColor { get; set; }

    public ColorDataSet ForegroundColor { get; set; }

    public ColorDataSet BorderColor { get; set; }

    public ColorDataSet HighlightColor { get; set; }

    public BrushDataSet()
    {
    }

    public BrushDataSet(BrushManagement brushManagement)
    {
        this._BrushManagement = brushManagement;

        if (this._BrushManagement == null)
        {
            this._BrushManagement = new BrushManagement();
        }

        this.BackgroundColor = new ColorDataSet(this._BrushManagement.Background);
        this.ForegroundColor = new ColorDataSet(this._BrushManagement.Foreground);
        this.BorderColor = new ColorDataSet(this._BrushManagement.Border);
        this.HighlightColor = new ColorDataSet(this._BrushManagement.Highlight);
    }
}
// EOF