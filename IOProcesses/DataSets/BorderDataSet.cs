/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BorderDataSet 
 * 
 *  serializable helper class to store and retrieve Border related data to
 *  or from hard drive storage.
 */
using EBoard.Models;
using System.Windows;
using System.Xml.Serialization;

namespace EBoard.IOProcesses.DataSets;

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
        _BorderManagement = borderManagement;

        if (_BorderManagement == null)
        {
            _BorderManagement = new BorderManagement();
        }


        BorderThickness = _BorderManagement.BorderThickness;
        CornerRadius = _BorderManagement.CornerRadius;
        Height = _BorderManagement.Height;
        Margin = _BorderManagement.Margin;
        Padding = _BorderManagement.Padding;
        Width = _BorderManagement.Width;
    }
}
// EOF