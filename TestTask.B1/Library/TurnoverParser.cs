using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTask.B1.Model;

namespace TestTask.B1.Library
{
    internal class TurnoverParser
    {
        /// <summary>
        ///     Parses data of an excel file
        ///     and put it into model
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns TurnoverSheet>Sheet model</returns>
        public static TurnoverSheet? Parse(string fileName)
        {
            try
            {
                TurnoverSheet sheet = new TurnoverSheet();
                List<TurnoverTable> tables = new List<TurnoverTable>();

                DataSet ds;

                if (fileName is null)
                    return null;

                #region #Read Table

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
                    return null;

                #endregion

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

                meta = meta.Substring(0, meta.Length - 2);                                   /// Make string splitable to restore it from database

                #endregion

                #region #Table Headers

                var subHeaders = new List<TurnoverHeader.TurnoverSubHeader>() { };           /// Create header of document outline

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
                                     .Select(x => x[0].ToString()).ToList();
                var classTotal = data.Where(x => x[0].ToString().StartsWith("ПО КЛАССУ"))    /// Class total data, has same index as class name
                                     .Select(x => new Model.TurnoverTableRow                    /// and stores Them as array of Models
                                     {
                                         ID = x[0].ToString(),
                                         InputBalanceActive = Convert.ToDouble(x[1].ToString()),
                                         InputBalancePassive = Convert.ToDouble(x[2].ToString()),
                                         DebitTurnover = Convert.ToDouble(x[3].ToString()),
                                         LoanTurnover = Convert.ToDouble(x[4].ToString()),
                                         OutputBalanceActive = Convert.ToDouble(x[5].ToString()),
                                         OutputBalancePassive = Convert.ToDouble(x[6].ToString()),
                                     }).ToList();

                var query = data.Where(x => x[0].ToString().IntCompatible()).Select(x =>        /// Query all data from table that contain values
                    new Model.TurnoverTableRow                                                  /// and stores Them as array of Models
                    {
                        ID = x[0].ToString(),
                        InputBalanceActive = Convert.ToDouble(x[1].ToString()),
                        InputBalancePassive = Convert.ToDouble(x[2].ToString()),
                        DebitTurnover = Convert.ToDouble(x[3].ToString()),
                        LoanTurnover = Convert.ToDouble(x[4].ToString()),
                        OutputBalanceActive = Convert.ToDouble(x[5].ToString()),
                        OutputBalancePassive = Convert.ToDouble(x[6].ToString()),
                    });

                var groupIDs = query.Where(x => x.ID.Length == 2).Select(x => x.ID).ToList();   /// Get all group 2 digit ids

                classNames.Each((o, i) =>
                {
                    List<TurnoverTableGroup> groups = new List<TurnoverTableGroup>();
                    foreach (var groupID in groupIDs.Where(x => x.StartsWith((i + 1).ToString()))) /// Filter through groups with class index
                    {
                        groups.Add(new TurnoverTableGroup                                       /// Create element in subgroup of class groups
                        {
                            GroupID = groupID,
                            TableRows = query.Where(x => x.ID.StartsWith(groupID))
                                         .OrderBy(x => x.ID)
                                         .ToList()
                        });
                    }
                    tables.Add(new TurnoverTable                                                /// Form class table object
                    {
                        ClassName = o,
                        AccountGroups = groups,
                        TotalByClass = classTotal[i]
                    });
                });

                var lastRow = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];                   /// Total balance of table (last row)
                var totalTotal = new Model.TurnoverTableRow
                {
                    ID = lastRow[0].ToString(),
                    InputBalanceActive = Convert.ToDouble(lastRow[1].ToString()),
                    InputBalancePassive = Convert.ToDouble(lastRow[2].ToString()),
                    DebitTurnover = Convert.ToDouble(lastRow[3].ToString()),
                    LoanTurnover = Convert.ToDouble(lastRow[4].ToString()),
                    OutputBalanceActive = Convert.ToDouble(lastRow[5].ToString()),
                    OutputBalancePassive = Convert.ToDouble(lastRow[6].ToString()),
                };

                #endregion

                sheet.BankName = bankName.ToString();                                           /// Form sheet object
                sheet.SheetDescription = meta;
                sheet.Currency = currency;
                sheet.SheetDate = DateTime.Parse(calendarDate);

                sheet.Header = header;
                sheet.Tables = tables;

                sheet.TotalBySheet = totalTotal;

                return sheet;                                                                   /// Return object of excel document
            }
            catch (FormatException e)
            {
                /// Just in case parse fails
                Trace.TraceError("Cannot Parse file");
                return null;
                throw;
            }
        }
    }
}
