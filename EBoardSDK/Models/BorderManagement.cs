/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *
 *  BorderManagement
 *
 *  model class for border properties
 */
using EBoardSDK.Interfaces;
using EBoardSDK.Models.DataSets;
using System.Windows;

namespace EBoardSDK.Models;

public class BorderManagement : IElementBorder
{
    public Thickness BorderThickness { get; set; }

    public CornerRadius CornerRadius { get; set; }

    public double Height { get; set; }

    public Thickness Margin { get; set; }

    public Thickness Padding { get; set; }

    public double Width { get; set; }

    public BorderManagement() => this.SetInitialValues();

    public BorderManagement(BorderDataSet borderDataSet) => this.LoadBorderDataSet(borderDataSet);

    private async void LoadBorderDataSet(BorderDataSet borderDataSet)
    {
        if (borderDataSet != null)
        {
            this.BorderThickness = borderDataSet.BorderThickness;
            this.CornerRadius = borderDataSet.CornerRadius;
            this.Height = borderDataSet.Height;
            this.Margin = borderDataSet.Margin;
            this.Padding = borderDataSet.Padding;
            this.Width = borderDataSet.Width;

            await Task.CompletedTask;

            return;
        }

        this.SetInitialValues();

        await Task.CompletedTask;
    }

    private void SetInitialValues()
    {
        this.BorderThickness = new Thickness(2, 2, 2, 2);
        this.CornerRadius = new CornerRadius(5, 5, 5, 5);
        this.Height = Double.NaN;
        this.Margin = new Thickness(5);
        this.Padding = new Thickness(5);
        this.Width = Double.NaN;
    }
}
// EOF