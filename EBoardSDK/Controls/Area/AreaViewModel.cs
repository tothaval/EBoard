// <copyright file="AreaViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

namespace EBoardSDK.Controls.Area;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Models;
using EBoardSDK.Plugins;
using System;

public partial class AreaViewModel<T> : ObservableObject
        where T : EBoardElementPluginBaseViewModel
{
    public AreaVerticalOuterViewModel<T> AreaVerticalOuterViewModel { get; }


    [ObservableProperty]
    private bool isMatrixChangeable = false;

    public AreaViewModel()
    {
        this.AreaVerticalOuterViewModel = new ();

        this.IsMatrixChangeable = true;
    }

    public void AddHorizontal()
    {
        this.AreaVerticalOuterViewModel.AddLine();
    }

    public void CreateMatrix(int horizontalCount, int verticalCount)
    {
        this.AreaVerticalOuterViewModel.CreateMatrix(horizontalCount, verticalCount);

        this.IsMatrixChangeable = false;
    }

    public void InsertViewModelList(List<T> viewModels, int verticalIndex)
    {
        this.AreaVerticalOuterViewModel.InsertViewModelList(viewModels, verticalIndex);
    }

    [RelayCommand]
    public void AddLine()
    {
        this.AreaVerticalOuterViewModel.AddLine();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void RemoveLine()
    {
        this.AreaVerticalOuterViewModel.RemoveLine();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void AddRow()
    {
        this.AreaVerticalOuterViewModel.AddRow();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }

    [RelayCommand]
    public void RemoveRow()
    {
        this.AreaVerticalOuterViewModel.RemoveRow();
        this.OnPropertyChanged(nameof(this.AreaVerticalOuterViewModel));
    }
}
