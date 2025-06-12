// <copyright file="EboardDataSet.cs" company=".">
// Stephan Kammel
// </copyright>

/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  EboardDataSet
 *
 *  serializable helper class to store and retrieve EBoardViewModel(s) data to
 *  or from hard drive storage.
 *
 *  Each Eboard has to store its own properties, as well as a list of all child
 *  elements - for now. Once Elements are getting more complex, it is probably
 *  better to trigger a separate serialization process/method for the elements and their data.
 *
 *  It is vital to further development efforts, to be able to save and load Eboard contents
 *  asap, because it reduces time needed to setup the ui and test a specific or recently added
 *  feature.
 */
namespace EBoardSDK.DataSets;

using EBoardSDK.ViewModels;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("EboardConfiguration")]
public class EboardDataSet
{
    [XmlIgnore]
    private EBoardViewModel eBoardViewModel;

    public EboardDataSet()
    {
    }

    public EboardDataSet(EBoardViewModel eBoardViewModel)
    {
        this.eBoardViewModel = eBoardViewModel;

        this.EBID = eBoardViewModel.EBID;
        this.EBoardName = eBoardViewModel.EBoardName;
        this.EBoardDepth = eBoardViewModel.EBoardDepth;

        this.BorderDataSet = new BorderDataSet(eBoardViewModel.BorderManagement);

        this.BrushDataSet = new BrushDataSet(eBoardViewModel.BrushManagement);

        if (this.BorderDataSet == null)
        {
            this.BorderDataSet = new BorderDataSet(new BorderManagement());
        }

        if (this.BrushDataSet == null)
        {
            this.BrushDataSet = new BrushDataSet(new BrushManagement());
        }
    }

    [XmlIgnore]
    public EBoardViewModel EBoardViewModel => this.eBoardViewModel;

    public string EBID { get; set; }

    public string EBoardName { get; set; }

    public int EBoardDepth { get; set; }

    public BorderDataSet BorderDataSet { get; set; }

    public BrushDataSet BrushDataSet { get; set; }

    [XmlIgnore]
    public ObservableCollection<ElementViewModel> Elements { get; set; } = new ObservableCollection<ElementViewModel> { };
}

// EOF