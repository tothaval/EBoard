// <copyright file="DateTimePicker.xaml.cs" company=".">
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
namespace EboardElementPluginCountDownTimer.Components;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaktionslogik für DateTimePicker.xaml
/// </summary>
public partial class DateTimePicker : UserControl
{
    public int Year
    {
        get { return (int)GetValue(YearProperty); }
        set { SetValue(YearProperty, value); }
    }
    public static readonly DependencyProperty YearProperty =
        DependencyProperty.Register("Year", typeof(int), typeof(DateTimePicker), new PropertyMetadata(2024));

    public int Month
    {
        get { return (int)GetValue(MonthProperty); }
        set { SetValue(MonthProperty, value); }
    }
    public static readonly DependencyProperty MonthProperty =
        DependencyProperty.Register("Month", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public int Day
    {
        get { return (int)GetValue(DayProperty); }
        set { SetValue(DayProperty, value); }
    }
    public static readonly DependencyProperty DayProperty =
        DependencyProperty.Register("Day", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public int Hour
    {
        get { return (int)GetValue(HourProperty); }
        set { SetValue(HourProperty, value); }
    }
    public static readonly DependencyProperty HourProperty =
        DependencyProperty.Register("Hour", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public int Minute
    {
        get { return (int)GetValue(MinuteProperty); }
        set { SetValue(MinuteProperty, value); }
    }
    public static readonly DependencyProperty MinuteProperty =
        DependencyProperty.Register("Minute", typeof(int), typeof(DateTimePicker), new PropertyMetadata(1));

    public ObservableCollection<int> Months
    {
        get { return (ObservableCollection<int>)GetValue(MonthsProperty); }
        set { SetValue(MonthsProperty, value); }
    }
    public static readonly DependencyProperty MonthsProperty =
        DependencyProperty.Register("Months", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Days
    {
        get { return (ObservableCollection<int>)GetValue(DaysProperty); }
        set { SetValue(DaysProperty, value); }
    }
    public static readonly DependencyProperty DaysProperty =
        DependencyProperty.Register("Days", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Hours
    {
        get { return (ObservableCollection<int>)GetValue(HoursProperty); }
        set { SetValue(HoursProperty, value); }
    }
    public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public ObservableCollection<int> Minutes
    {
        get { return (ObservableCollection<int>)GetValue(MinutesProperty); }
        set { SetValue(MinutesProperty, value); }
    }
    public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(ObservableCollection<int>), typeof(DateTimePicker), new PropertyMetadata(null));

    public DateTimePicker()
    {
        InitializeComponent();
    }
}

// EOF