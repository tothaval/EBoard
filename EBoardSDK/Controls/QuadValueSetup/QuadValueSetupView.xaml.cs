// <copyright file="QuadValueSetupView.xaml.cs" company=".">
// Stephan Kammel
// </copyright>
/// license
///
/// <b>ad-hoc license terms eboard prototype</b><br>
/// <br>
/// <br>
/// contact: kammel@posteo.de
/// <br>
/// <p>
/// until a license has been chosen, you may 
/// use the software or parts of it under the following conditions:<br><br>
/// 1.)
/// If you want to distribute or use the source code or a derived binary
/// of the EBoard project for commercial purposes, you need to contact
/// the project team for authorization and payment details.
/// You may use the source or a derived binary for non commercial 
/// purposes free of charge. In order to do so, copy this adhoc terms
/// and a link to the repository to any source code file that uses code
/// derived from this project and to the folder that holds the compiled source code.
///
/// 2.)
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
/// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
/// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
/// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
/// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
/// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
/// OTHER DEALINGS IN THE SOFTWARE.
/// </p>
namespace EBoardSDK.Controls.QuadValueSetup;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

/// <summary>
/// Interaktionslogik für QuadValueSetupView.xaml
/// </summary>
public partial class QuadValueSetupView : UserControl
{
    public QuadValueSetupView()
    {
        this.InitializeComponent();
    }

    public Style ButtonStyle
    {
        get { return (Style)this.GetValue(ButtonStyleProperty); }
        set { this.SetValue(ButtonStyleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ButtonStyleProperty =
        DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(QuadValueSetupView), new PropertyMetadata(null));

    public int Maximum
    {
        get { return (int)this.GetValue(MaximumProperty); }
        set { this.SetValue(MaximumProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(int), typeof(QuadValueSetupView), new PropertyMetadata(20));

    public string Value1
    {
        get { return (string)this.GetValue(Value1Property); }
        set { this.SetValue(Value1Property, value); }
    }

    // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Value1Property =
        DependencyProperty.Register("Value1", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("1"));

    public string Value2
    {
        get { return (string)this.GetValue(Value12Property); }
        set { this.SetValue(Value12Property, value); }
    }

    // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Value12Property =
        DependencyProperty.Register("Value2", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("2"));

    public string Value3
    {
        get { return (string)this.GetValue(Value3Property); }
        set { this.SetValue(Value3Property, value); }
    }

    // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Value3Property =
        DependencyProperty.Register("Value3", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("3"));

    public string Value4
    {
        get { return (string)this.GetValue(Value4Property); }
        set { this.SetValue(Value4Property, value); }
    }

    // Using a DependencyProperty as the backing store for Value1.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty Value4Property =
        DependencyProperty.Register("Value4", typeof(string), typeof(QuadValueSetupView), new PropertyMetadata("4"));

    public ICommand OKCommand
    {
        get { return (ICommand)this.GetValue(OKCommandProperty); }
        set { this.SetValue(OKCommandProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OKCommand.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OKCommandProperty =
        DependencyProperty.Register("OKCommand", typeof(ICommand), typeof(QuadValueSetupView), new PropertyMetadata(null));

    public Brush ForegroundBrush
    {
        get { return (Brush)this.GetValue(ForegroundBrushProperty); }
        set { this.SetValue(ForegroundBrushProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ForegroundBrush.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ForegroundBrushProperty =
        DependencyProperty.Register("ForegroundBrush", typeof(Brush), typeof(QuadValueSetupView), new PropertyMetadata(null));
}

// EOF