﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace TestTask.B1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Model.Generator gen = new Model.Generator();
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
            Trace.WriteLine(gen.RandomSet());
        }
    }
}
