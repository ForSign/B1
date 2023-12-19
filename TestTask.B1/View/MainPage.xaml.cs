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
using TestTask.B1.Library;
using System.Diagnostics.Metrics;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using ExcelDataReader;
using static TestTask.B1.Library.Extensions;
using TestTask.B1.Model;

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
#if DEBUG
            string fileName = @"C:\\Users\\AAA\\Desktop\\TestTask\\Excel\\ОСВ для тренинга.xls";
            TurnoverSheet? sheet = TurnoverParser.Parse(fileName);
            TableViewPage pp = new TableViewPage(sheet);
            pp.ShowDialog();
            App.Current.Shutdown();
            MenuItem_UploadXLS(new object(), new RoutedEventArgs());
#endif
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
        }

        private void MenuItem_InsertFiles(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to filter files beforehand?", "Warning", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
                (new MergeFilesPage()).ShowDialog();

            MessageBox.Show("Select File / Files you wish to import");

            string[]? files = FileWorker.OpenFile(Multiselect: true);
            string tempFile = System.IO.Path.GetTempFileName();

            if (files is null)
                return;

            FileWorker.MergeFiles(files, tempFile);

            int totalLines = File.ReadLines(tempFile).Count();

            using (var sr = new StreamReader(tempFile))
            {
                string? line;
                string[] data;
                string sqlCommand;
                int currentLine = 0;

                var db = dbWorker.getInstance();

                while ((line = sr.ReadLine()) != null)
                {
                    currentLine++;
                    Trace.WriteLine($"Importing {currentLine} line out of {totalLines}. Left: {totalLines - currentLine}");

                    data = line.Split("||");
                    sqlCommand = "insert into Task_1 " +
                        $"(DateTimeStamp, CharsetENG, CharsetRUS, DecimalValue, DoubleValue) " +
                        $"values " +
                        $"('{data[0]}', '{data[1]}', '{data[2]}', '{data[3]}', '{data[4]}');";

                    db.ExecuteNonQuery(new string[] { sqlCommand });
                }

                Trace.WriteLine("Done importing");
            }
        }

        private void MenuItem_CountMAS(object sender, RoutedEventArgs e)
        {
            string script = File.ReadAllText("static/external_sql.sql");

            if (script != null)
            {
                var db = dbWorker.getInstance();
                var dt = new DataTable();
                dt.Load(db.ExecuteReader(script));
                var rows = dt.AsEnumerable().ToArray();

                MessageBox.Show("" +
                    $"Decimal Summary: {rows[0][0]}\n" +
                    $"Double Median: {rows[0][1]}");

                Trace.WriteLine($"Decimal Summary: {rows[0][0]}");
                Trace.WriteLine($"Double Median: {rows[0][1]}");
            }
        }

        private void MenuItem_UploadXLS(object sender, RoutedEventArgs e)
        {

#if RELEASE
            string? fileName = FileWorker.OpenFile(Multiselect: false, Filter: "Excel files (.xls)|*.xls")?[0];
#elif DEBUG
            string fileName = @"C:\\Users\\AAA\\Desktop\\TestTask\\Excel\\ОСВ для тренинга.xls";
#endif
            TurnoverSheet? sheet = TurnoverParser.Parse(fileName);
            return;
        }

        private void MenuItem_ViewUploaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_SwitchFileView(object sender, RoutedEventArgs e)
        {

        }
    }
}
