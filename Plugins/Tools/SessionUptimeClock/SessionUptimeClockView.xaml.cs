﻿using EBoard.Plugins.Tools.SessionUptimeClock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EBoard.Plugins.Tools.SessionUptimeClock;

/// <summary>
/// Interaktionslogik für SessionUptimeClockView.xaml
/// </summary>
public partial class SessionUptimeClockView : UserControl
{
    public SessionUptimeClockView()
    {
        InitializeComponent();

        DataContext = new SessionUptimeClockViewModel();
    }
}
