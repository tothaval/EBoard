// <copyright file="ElementDataSetFactory.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Utilities.Factories;

using EBoardSDK.DataSets;
using EBoardSDK.Interfaces;
using EBoardSDK.Models;
using EBoardSDK.Models.DataSets;

public static class ElementDataSetFactory
{
    public static ElementDataSet GetElementDataSet(IPlugin? plugin = null, IPlugin? externalPlugin = null)
    {
        ElementDataSet elementDataSet = new ElementDataSet();

        elementDataSet.AddBorderDataSet(new BorderDataSet(new BorderManagement()));
        elementDataSet.AddBrushDataSet(new BrushDataSet(new BrushManagement()));
        elementDataSet.AddPlacementDataSet(new PlacementDataSet(new PlacementManagement()));

        if (plugin is not null)
        {
            elementDataSet.Plugin = plugin;
        }

        if (externalPlugin is not null)
        {
            elementDataSet.Plugin = externalPlugin;
        }

        return elementDataSet;
    }
}

// EOF