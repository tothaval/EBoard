/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BrushDataSet 
 * 
 *  serializable helper class to store and retrieve Brush related data to
 *  or from hard drive storage.
 */
using EBoardSDK.Models;
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
        _BrushManagement = brushManagement;

        if (_BrushManagement == null)
        {
            _BrushManagement = new BrushManagement();
        }

        BackgroundColor = new ColorDataSet(_BrushManagement.Background);
        ForegroundColor = new ColorDataSet(_BrushManagement.Foreground);
        BorderColor = new ColorDataSet(_BrushManagement.Border);
        HighlightColor = new ColorDataSet(_BrushManagement.Highlight);

    }


}
// EOF