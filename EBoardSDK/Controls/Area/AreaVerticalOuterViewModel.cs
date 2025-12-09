// <copyright file="AreaVerticalOuterViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Controls.Area;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using EBoardSDK.Plugins.Elements.Link;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;

/// <summary>
/// TODO
/// sinnvoll refaktorn und Methoden auslagern.
/// 
/// // make CreateNewArea a task?
/// </summary>
public partial class AreaVerticalOuterViewModel<T> : ObservableObject
        where T : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<AreaHorizontalInnerViewModel<T>> horizontalInnerViewModels;

    [ObservableProperty]
    private int verticalCount = 0;

    [ObservableProperty]
    private int horizontalCount = 0;

    public AreaVerticalOuterViewModel()
    {
        this.HorizontalInnerViewModels = new ObservableCollection<AreaHorizontalInnerViewModel<T>>();
    }

    public void CreateMatrix(int x, int y)
    {
        this.HorizontalCount = x;
        this.VerticalCount = y;

        this.CreateNewArea();
    }

    public void InsertViewModelList(List<T> viewModels, int verticalIndex)
    {
        if (verticalIndex < this.HorizontalInnerViewModels.Count)
        {
            this.HorizontalCount = viewModels.Count;

            this.HorizontalInnerViewModels[verticalIndex].InsertViewModelList(viewModels);
        }
    }

    public void AddLine()
    {
        this.VerticalCount++;

        this.CreateNewArea();

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }

    public void RemoveLine()
    {
        if (this.VerticalCount > 0)
        {
            this.VerticalCount--;
        }

        if (this.HorizontalInnerViewModels.Count == 0)
        {
            return;
        }

        var vm = this.HorizontalInnerViewModels.LastOrDefault();

        if (vm != null)
        {
            this.HorizontalInnerViewModels.Remove(vm);

            this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
        }
    }

    public void AddRow()
    {
        this.HorizontalCount++;

        this.CreateNewArea();

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }

    public void RemoveRow()
    {
        if (this.HorizontalCount > 0)
        {
            this.HorizontalCount--;
        }

        if (this.HorizontalInnerViewModels.Count == 0)
        {
            return;
        }

        foreach (var item in this.HorizontalInnerViewModels)
        {
            item.RemoveElement();
        }
    }

    private void AddAreaHorizontalInnerViewModel()
    {
        var areahorizontal = new AreaHorizontalInnerViewModel<T>();

        this.HorizontalInnerViewModels.Add(areahorizontal);
    }

    private void CreateNewArea()
    {
        if (this.VerticalCount == 0)
        {
            this.VerticalCount++;
        }

        if (this.HorizontalCount == 0)
        {
            this.HorizontalCount++;
        }

        var areaVMcount = this.HorizontalInnerViewModels.Count;

        for (int i = 0; i < this.VerticalCount; i++)
        {
            if (i >= areaVMcount)
            {
                this.AddAreaHorizontalInnerViewModel();
            }
        }

        foreach (var item in this.HorizontalInnerViewModels)
        {
            var elementsCount = item.Elements.Count;

            for (int i = 0; i < this.HorizontalCount; i++)
            {
                if (i >= elementsCount)
                {
                    item.AddElement();
                }
            }
        }

        this.OnPropertyChanged(nameof(this.HorizontalInnerViewModels));
    }
}
