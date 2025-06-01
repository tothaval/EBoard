/*  EBoard (experimental UI design) (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PlacementManagement 
 * 
 *  model for placement property changes
 *  can be applied to MainWindowViewModel and ElementViewModel to define the initial
 *  values on load or upon creation, as well as to store changes during runtime.
 */
using EBoardSDK.Interfaces;
using EBoardSDK.Models.DataSets;
using System.Windows;

namespace EBoardSDK.Models;

public class PlacementManagement : IElementPlacement
{
    /// <summary>
    /// Gets or sets the rotation of an element
    /// </summary>
    public double Angle { get; set; }

    /// <summary>
    /// Gets or sets the position of an element
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    /// Gets or sets the z-index of an element
    /// </summary>
    public int Z { get; set; }

    public PlacementManagement() => this.SetInitialValues();

    public PlacementManagement(PlacementDataSet placementDataSet) => this.LoadPlacementDataSet(placementDataSet);

    private async void LoadPlacementDataSet(PlacementDataSet placementDataSet)
    {
        if (placementDataSet != null)
        {
            this.Angle = placementDataSet.Angle;
            this.Position = placementDataSet.Position;
            this.Z = placementDataSet.Z;

            await Task.CompletedTask;

            return;
        }

        this.SetInitialValues();

        await Task.CompletedTask;
    }

    private void SetInitialValues()
    {
        this.Angle = 0.0;
        this.Position = new Point(25.0, 25.0);
        this.Z = 0;
    }
}
// EOF