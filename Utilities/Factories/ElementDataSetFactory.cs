using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using EBoard.IOProcesses.DataSets.Interfaces;
using EBoard.Models;
using EBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.CodeDom;
using EBoard.Plugins.Tools.Views;
using EBoard.Plugins.Tools.ViewModels;

namespace EBoard.Utilities.Factories;

public static class ElementDataSetFactory
{
    public static ElementDataSet GetContentElementDataSet(
        string elementId = "-1",
        string elementHeader = "Prototype Element ",
        IElementContent? elementContent = null)
    {

        ElementDataSet elementDataSet = new ElementDataSet
        {
            EID = elementId,
            ElementHeader = elementHeader,
            IsContentNotShape = true
        };

        if (elementContent != null)
        {
            elementDataSet.ElementContent = elementContent;
        }

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        return elementDataSet;
    }

    public static ElementDataSet GetElementDataSet(string elementId,
        string elementHeader, bool isContentNotShape,  IElementContent? elementContent = null)
    {
        ElementDataSet elementDataSet = new ElementDataSet
        {
            EID = elementId,
            ElementHeader = elementHeader,
            IsContentNotShape = isContentNotShape
        };

        if (elementContent != null)
        {
            elementDataSet.ElementContent = elementContent;
        }

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        return elementDataSet;
    }

    public static ElementDataSet GetShapeElementDataSet(
        string elementId = "-1",
        string elementHeader = "Shape Element ",
        Shape? shape = null)
    {
        ElementDataSet elementDataSet = new ElementDataSet
        {
            EID = elementId,
            ElementHeader = elementHeader,
            IsContentNotShape = false
        };

        if (shape != null)
        {
            elementDataSet.ElementContent = new ShapeManagement(shape);
        }

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        return elementDataSet;
    }

    internal static ElementDataSet GetElementDataSetByCommandParameter(string commandParameter)
    {
        string[] shards = commandParameter.Split('.');

        switch (shards[0])
        {
            //case "Elements":

            //    break;

            //case "Shapes":                    

            //    //Shape shape = FactoryPattern<shards[0], >

            //    //return GetShapeElementDataSet()
            //    break;

            case "Tools":

                return GetContentElementDataSet(
                    elementHeader: "Session Uptime Clock",
                    elementContent: new ContainerManagement(new SessionUptimeClockView() { DataContext = new SessionUptimeClockViewModel()}));

            default:
                return GetContentElementDataSet();
        }


    }

    //public static class FactoryPattern<K, T> where T : class, K, new()
    //{
    //    public static K GetInstance()
    //    {
    //        K objK;

    //        objK = new T();

    //        return objK;
    //    }
    //}


}
