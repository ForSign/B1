using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using TestTask.B1.View;
using TestTask.B1.Library;
using System.IO;
using System.Data;
using TestTask.B1.Model;
using System.ComponentModel;

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
        }

        private void MenuItem_InsertFiles(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to filter files beforehand?", "Warning", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
                (new MergeFilesPage()).ShowDialog();

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;

            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object? sender, DoWorkEventArgs e)
        {

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
                bool dataCorrupted = false;

                while ((line = sr.ReadLine()) != null)
                {
                    currentLine++;
                    Trace.WriteLine($"Importing {currentLine} line out of {totalLines}. Left: {totalLines - currentLine}");

                    data = line.Split("||");
                    if (data.Length < 5)
                    {
                        dataCorrupted = true;
                        continue;
                    }

                    sqlCommand = "insert into Task_1 " +
                        $"(date_timestamp, charset_eng, charset_rus, decimal_value, double_value) " +
                        $"values " +
                        $"('{data[0]}', '{data[1]}', '{data[2]}', '{data[3]}', '{data[4]}');";

                    db.ExecuteNonQuery(new string[] { sqlCommand });
                }

                if (dataCorrupted)
                    Trace.TraceWarning("Part of the date was corrupted and cannot be imported, therefor it was skipped");
                Trace.WriteLine("Done importing");
            }
        }

        private void MenuItem_CountMAS(object sender, RoutedEventArgs e)
        {
            string script = File.ReadAllText("static/sql/external_sql.sql");

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
            string? fileName = FileWorker.OpenFile(Multiselect: false, Filter: "Excel files (.xls)|*.xls")?[0];
            if (fileName != null)
            {
                TurnoverSheet? sheet = TurnoverParser.Parse(fileName);
                TurnoverUploader.InsertToDB(sheet);
                MessageBox.Show("Upload Complete!");
                Trace.WriteLine($"Upload to database complete for: {fileName}");
            }
        }

        private void MenuItem_ViewUploaded(object sender, RoutedEventArgs e)
        {
            ChooseSQLPage chooseSQLPage = new ChooseSQLPage();
            chooseSQLPage.ShowDialog();
        }
    }
}
