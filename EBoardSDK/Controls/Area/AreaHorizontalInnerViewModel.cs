// <copyright file="AreaHorizontalInnerViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Controls.Area;

using CommunityToolkit.Mvvm.ComponentModel;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System;
using System.Collections.ObjectModel;
using System.Linq;

public partial class AreaHorizontalInnerViewModel<T> : ObservableObject
    where T : EBoardElementPluginBaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<T> elements;

    [ObservableProperty]
    private T element;

    public AreaHorizontalInnerViewModel()
    {
        this.Elements = new();
    }

    public void InsertViewModelList(List<T> viewModels)
    {
        this.Elements.Clear();

        foreach (var model in viewModels)
        {
            this.AddElement(model);
        }
    }

    public void AddElement(T? elementViewModel = null)
    {
        if (elementViewModel != null)
        {
            this.Elements?.Add(elementViewModel);

            return;
        }

        var instance = Activator.CreateInstance<T>();

        if (instance != null)
        {
            this.Elements?.Add(instance);
        }

        this.OnPropertyChanged(nameof(this.Elements));
    }

    public void RemoveElement()
    {
        var last = this.Elements?.LastOrDefault();

        if (last != null)
        {
            this.Elements?.Remove(last);
            this.OnPropertyChanged(nameof(this.Elements));
        }
    }
}
