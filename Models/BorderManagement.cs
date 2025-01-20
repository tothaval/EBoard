/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BorderManagement 
 * 
 *  model class for border properties
 */
using EBoard.IOProcesses.DataSets;
using System.Windows;
using EBoard.Interfaces;

namespace EBoard.Models;

public class BorderManagement : IElementBorder
{
            
    public Thickness BorderThickness { get; set; }


    public CornerRadius CornerRadius { get; set; }


    public double Height { get; set; }


    public Thickness Margin { get; set; }


    public Thickness Padding { get; set; }


    public double Width { get; set; }


    public BorderManagement()
    {
        SetInitialValues();
    }


    public BorderManagement(BorderDataSet borderDataSet)
    {
        LoadBorderDataSet(borderDataSet);
    }


    private async void LoadBorderDataSet(BorderDataSet borderDataSet)
    {
        if (borderDataSet != null)
        {
            BorderThickness = borderDataSet.BorderThickness;
            CornerRadius = borderDataSet.CornerRadius;
            Height = borderDataSet.Height;
            Margin = borderDataSet.Margin;
            Padding = borderDataSet.Padding;
            Width = borderDataSet.Width;

            await Task.CompletedTask;

            return;
        }

        SetInitialValues();

        await Task.CompletedTask;
    }


    private void SetInitialValues()
    {
        BorderThickness = new Thickness(2, 2, 2, 2);
        CornerRadius = new CornerRadius(5);
        Height = 100.0;
        Margin = new Thickness(5);
        Padding = new Thickness(5);
        Width = 200.0;
    }



}
// EOF