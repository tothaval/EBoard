﻿// <copyright file="SolidColorBrushSetupViewModel.cs" company=".">
// Stephan Kammel
// </copyright>

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EBoardSDK.Enums;
using EBoardSDK.Models;
using System.Windows.Media;

namespace EBoardSDK.Controls
{
    public partial class SolidColorBrushSetupViewModel : ObservableObject, IDisposable
    {
        [ObservableProperty]
        private SolidColorBrush colorBrush;


        [ObservableProperty]
        private string colorStringValue;

        [ObservableProperty]
        private Color colorValue;

        partial void OnColorValueChanged(Color value)
        {
            ColorBrush = new SolidColorBrush(value);

            ColorStringValue = value.ToString();
        }

        [ObservableProperty]
        private byte greyscaleValue = 0;

        partial void OnGreyscaleValueChanged(byte value)
        {
            RedValue = value;
            GreenValue = value;
            BlueValue = value;
        }

        [ObservableProperty]
        private byte redValue = 0;

        partial void OnRedValueChanged(byte value)
        {
            ColorValue = Color.FromArgb(AlphaValue, RedValue, GreenValue, BlueValue);
        }

        [ObservableProperty]
        private byte greenValue = 0;

        partial void OnGreenValueChanged(byte value)
        {
            ColorValue = Color.FromArgb(AlphaValue, RedValue, GreenValue, BlueValue);
        }

        [ObservableProperty]
        private byte blueValue = 0;

        partial void OnBlueValueChanged(byte value)
        {
            ColorValue = Color.FromArgb(AlphaValue, RedValue, GreenValue, BlueValue);
        }

        [ObservableProperty]
        private byte alphaValue = 255;

        partial void OnAlphaValueChanged(byte value)
        {
            ColorValue = Color.FromArgb(value, ColorValue.R, ColorValue.G, ColorValue.B);
        }

        private Action okAction;

        [RelayCommand]
        private void Ok()
        {
            this.okAction?.Invoke();
        }

        private readonly BrushManagement brushManagement;

        public BrushManagement BrushManagement => this.brushManagement;

        public SolidColorBrushSetupViewModel(BrushManagement brushManagement, BrushTargets brushTargets, Action okResult)
        {
            this.brushManagement = brushManagement;
            this.okAction = okResult;

            Brush brush = new SolidColorBrush();

            switch (brushTargets)
            {
                case BrushTargets.Background:
                    brush = brushManagement.Background;
                    break;
                case BrushTargets.Border:
                    brush = brushManagement.Border;
                    break;
                case BrushTargets.Foreground:
                    brush = brushManagement.Foreground;
                    break;
                case BrushTargets.Highlight:
                    brush = brushManagement.Highlight;
                    break;
                default:
                    break;
            }

            this.ColorBrush = brush.GetType().Equals(typeof(SolidColorBrush)) ? (SolidColorBrush)brush : new SolidColorBrush();

            var c = this.ColorBrush.Color;

            this.AlphaValue = c.A;

            var cmid = ((int)c.R + (int)c.G + (int)c.B) / 3;

            this.GreyscaleValue = (byte)cmid;

            this.RedValue = c.R;
            this.GreenValue = c.G;
            this.BlueValue = c.B;

            this.BrushManagement.PropertyChangedEvent += this.BrushManagement_PropertyChangedEvent;
        }

        public void Dispose()
        {
            this.BrushManagement.PropertyChangedEvent -= this.BrushManagement_PropertyChangedEvent;

            GC.SuppressFinalize(this);
        }

        private void BrushManagement_PropertyChangedEvent()
        {
            this.OnPropertyChanged(nameof(this.BrushManagement));
        }
    }
}
