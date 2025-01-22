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
using EBoard.Plugins.Tools.SessionUptimeClock;
using EBoard.Plugins.Elements.StandardText;

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

        if (elementContent is not null)
        {
            elementDataSet.ElementContent = elementContent;
        }

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        return elementDataSet;
    }

    public static ElementDataSet GetElementDataSet(string elementId,
        string elementHeader, bool isContentNotShape, IElementContent? elementContent = null)
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

        string pluginView = string.Concat($"EBoard.Plugins.{shards[0]}.{shards[1]}.{shards[1]}View");
        string pluginViewModel = string.Concat($"EBoard.Plugins.{shards[0]}.{shards[1]}.{shards[1]}ViewModel");

        Type? type_PluginView = Type.GetType(pluginView);
        Type? type_PluginViewModel = Type.GetType(pluginViewModel);

        UserControl? pluginViewInstance = (UserControl)Activator.CreateInstance(type_PluginView);

        if (pluginViewInstance is not null)
        {
            pluginViewInstance.DataContext = Activator.CreateInstance(type_PluginViewModel);

            ElementDataSet elementDataSet = GetContentElementDataSet(
                        elementHeader: shards[2],
                        elementContent: new ContainerManagement(pluginViewInstance));

            return elementDataSet;
        }

        return ElementDataSetFactory.GetContentElementDataSet(
                        elementContent: new ContainerManagement(new TextBox()
                        {
                            Text = $"Plugin Instantiation Error",
                            AcceptsReturn = true,
                            AcceptsTab = true,
                            TextWrapping = TextWrapping.Wrap
                        }));

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
