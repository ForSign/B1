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
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine("TestMenuBAR");
        }

        private void MenuItem_GenFiles(object sender, RoutedEventArgs e)
        {
            Model.Generator generator = new Model.Generator();
            generator.GenerateFiles(generator.RandomSet, 5, 50, "COOK");
            Trace.WriteLine("Generate Files Complete");
        }

        private void MenuItem_MergeFiles(object sender, RoutedEventArgs e)
        {
            Library.FileMerge fm = new Library.FileMerge();
            fm.PurgeFiles(new string[] {"mo", "CX", "Y"}, "COOK");
            fm.MergeFiles("COOK", "FINISH\\finish.txt");
        }
    }
}
