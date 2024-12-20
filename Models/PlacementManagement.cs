using EBoard.Interfaces;
using EBoard.IOProcesses.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EBoard.Models
{
    public class PlacementManagement : IElementPlacement
    {
        /// <summary>
        /// the rotation of an element
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// the position of an element
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// the z-index of an element
        /// </summary>
        public int Z { get; set; }


        public PlacementManagement()
        {
            SetInitialValues();
        }

        public PlacementManagement(PlacementDataSet placementDataSet)
        {
            LoadPlacementDataSet(placementDataSet);
        }


        private async void LoadPlacementDataSet(PlacementDataSet placementDataSet)
        {
            if (placementDataSet != null)
            {
                Angle = placementDataSet.Angle;
                Position = placementDataSet.Position;
                Z = placementDataSet.Z;

                await Task.CompletedTask;

                return;
            }

            SetInitialValues();

            await Task.CompletedTask;
        }


        private void SetInitialValues()
        {
            Angle = 0.0;
            Position = new Point(25.0,25.0);
            Z = 0;
        }



    }
}
