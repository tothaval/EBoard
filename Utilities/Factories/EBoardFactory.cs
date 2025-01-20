using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBoard.Utilities.Factories;

public static class EBoardFactory
{

    public static EBoardViewModel GetEBoardViewModelByEBoardDataSet(EboardDataSet eboardDataSet)
    {
        EBoardViewModel eBoardViewModel = new EBoardViewModel(eboardDataSet);

        return eBoardViewModel;
    }


    public static EboardDataSet GetNewEBoardDataSetDefined(string name, int depth, double width, double height)
    {
        EboardDataSet eboardDataSet = new EboardDataSet
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardName = name,
            EBoardDepth = depth,
            BorderDataSet = new BorderDataSet(new BorderManagement() { Width = width, Height = height }),
            BrushDataSet = new BrushDataSet(new BrushManagement())
        };
                    
        return eboardDataSet;
    }

    public static EboardDataSet GetNewEBoardDataSet()
    {
        EboardDataSet eboardDataSet = new EboardDataSet
        {
            EBID = $"EBoard_{DateTime.Now.Ticks}",
            EBoardName = "new eboard",
            EBoardDepth = 100,
            BorderDataSet = new BorderDataSet(new BorderManagement() { Width = 640, Height = 320 }),
            BrushDataSet = new BrushDataSet(new BrushManagement())
        };

        return eboardDataSet;
    }

}
