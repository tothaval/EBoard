using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EBoardSDK.Controls
{
    public partial class SolidColorBrushSetupViewModel : ObservableObject
    {
        [ObservableProperty]
        private SolidColorBrush colorBrush;

        [ObservableProperty]
        private Color colorValue;
        partial void OnColorValueChanged(Color value)
        {
            ColorBrush = new SolidColorBrush(value);
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
            okAction?.Invoke();
        }

        public SolidColorBrushSetupViewModel(SolidColorBrush thebeforebrush, Action okResult)
        {
            okAction = okResult;

            ColorBrush = thebeforebrush;

            var c = thebeforebrush.Color;

            AlphaValue = c.A;

            var cmid = ((int)c.R + (int)c.G + (int)c.B) / 3;

            GreyscaleValue = (byte)cmid;

            RedValue = c.R;
            GreenValue = c.G;
            BlueValue = c.B;
        }
    }
}
