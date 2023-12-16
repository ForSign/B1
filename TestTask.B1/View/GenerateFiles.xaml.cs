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
using System.Windows.Shapes;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for GenerateFiles.xaml
    /// </summary>
    public partial class GenerateFiles : Window
    {
        public GenerateFiles()
        {
            InitializeComponent();
            Loaded += WindwLoaded;
        }

        private void WindwLoaded(object sender, RoutedEventArgs e)
        {
            // do work here
        }
    }
}
