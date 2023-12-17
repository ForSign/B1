using System;
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
using TestTask.B1.View;

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

        private void MenuItem_GenFiles(object sender, RoutedEventArgs e)
        {
            GenerateFilesPage generateFilesPage = new GenerateFilesPage();
            generateFilesPage.ShowDialog();
        }

        private void MenuItem_MergeFiles(object sender, RoutedEventArgs e)
        {
            MergeFilesPage mergeFilesPage = new MergeFilesPage();
            mergeFilesPage.ShowDialog();

            //Library.FileMerge fm = new Library.FileMerge();
            //fm.PurgeFiles(new string[] {"mo", "CX", "Y"}, "COOK");
            //fm.MergeFiles("COOK", "FINISH\\finish.txt");
        }

        private void MenuItem_InsertFiles(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_CountMAS(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_UploadXLS(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_ViewUploaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_SwitchFileView(object sender, RoutedEventArgs e)
        {

        }
    }
}
