using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace TestTask.B1.View
{
    /// <summary>
    /// Interaction logic for GenerateFiles.xaml
    /// </summary>
    public partial class GenerateFilesPage : Window
    {
        private string? FolderPath;

        private const int DefaultFileCount = 100;
        private const int DefaultLineCount = 100_000;

        public GenerateFilesPage()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            xName_FileCount.Text = DefaultFileCount.ToString();
            xName_LineCount.Text = DefaultLineCount.ToString();

            xBtn_SelectFolder.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void Btn_Generate(object sender, RoutedEventArgs e)
        {
            int fileCount;
            int lineCount;

            try
            {
                fileCount = Int32.Parse(xName_FileCount.Text);
                lineCount = Int32.Parse(xName_LineCount.Text);
                if (String.IsNullOrEmpty(FolderPath))
                {
                    MessageBox.Show("You need to select destination folder");
                    return;
                }
            }
            catch 
            {
                MessageBox.Show("Cannot parse line or file count \nData provided must be integer");
                return;
            }

            this.Visibility = Visibility.Hidden;

            Model.Generator generator = new Model.Generator();
            generator.GenerateFiles(generator.RandomSet, fileCount, lineCount, FolderPath);
            Trace.WriteLine("Generate Files Complete");

            this.Close(); // Return Control due to ShowDialog();
        }

        private void Btn_SelectFolder(object sender, RoutedEventArgs e)
        {
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    if (files.Length > 0)
                        if (MessageBox.Show("WARNING! This is non-empty folder! Continue?", "Warning", MessageBoxButton.OKCancel)
                            == MessageBoxResult.Cancel)
                            return;

                    FolderPath = fbd.SelectedPath;
                }
            }
        }
    }
}
