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
            DataSet ds;

            if (fileName is null)
                return;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var streamval = File.Open(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(streamval))
            {
                var configuration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false
                    }
                };
                ds = reader.AsDataSet(configuration);
            }

            var data = ds.Tables[0]?.AsEnumerable();

            if (data is null)
                return;

            #region #Table Information

            var bankName = ds.Tables[0].Rows[0][0];                                      /// Get Bank Name
            
            var idx_date = data.Where(x => x[0].ToString().DateCompatible())             /// Get index of date to grep meta information
                               .Select(x => x.Table.Rows.IndexOf(x))                     /// between date and bank name
                               .ToList<int>()[0];
            var calendarDate = ds.Tables[0].Rows[idx_date][0].ToString();                /// Get date of document (right before table itself)
            var currency = ds.Tables[0].Rows[idx_date][6].ToString();                    /// Get currency type (same row, last idx)

            string meta = "";                                                            /// Meta information. example: оборотная ведомость, за период etc.
            for (int i = 1; i < idx_date; i++)
                meta += ds.Tables[0].Rows[i][0] + ";;";

            meta = meta.Substring(meta.Length - 2);                                      /// Make string splitable to restore it from database

            #endregion

            #region #Table Headers

            var subHeaders = new List<TurnoverHeader.TurnoverSubHeader>() { };

            for (int i = 0; i < 3; i++)
            {
                subHeaders.Add(new TurnoverHeader.TurnoverSubHeader
                {
                    Header = ds.Tables[0].Rows[idx_date + 1][i * 2 + 1].ToString(),
                    ColumnSplit1 = ds.Tables[0].Rows[idx_date + 2][i * 2 + 1].ToString(),
                    ColumnSplit2 = ds.Tables[0].Rows[idx_date + 2][i * 2 + 2].ToString()
                });
            }

            var header = new TurnoverHeader(
                c1: ds.Tables[0].Rows[idx_date + 1][0].ToString(), 
                c2: subHeaders[0],
                c3: subHeaders[1],
                c4: subHeaders[2]
                );

            #endregion

            #region #Table Data

            var classNames = data.Where(x => x[0].ToString().StartsWith("КЛАСС"))        /// Class names
                                 .Select(x => x[0].ToString());
            var classTotal = data.Where(x => x[0].ToString().StartsWith("ПО КЛАССУ"))    /// Class total data, has same index as class name
                                 .Select(x => x[0].ToString());

            var query = data.Where(x => x[0].ToString().IntCompatible()).Select(x =>     /// Query all data from table that contain values
                new Model.TurnoverTable                                                  /// and stores Them as array of Models
                {
                    ID = x[0].ToString(),
                    InputBalanceActive = x[1].ToString(),
                    InputBalancePassive = x[2].ToString(),
                    DebitTurnover = x[3].ToString(),
                    LoanTurnover = x[4].ToString(),
                    OutputBalanceActive = x[5].ToString(),
                    OutputBalancePassive = x[6].ToString(),
                });

            #endregion

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
