using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.B1.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TestTask.B1.Library
{
    internal class TurnoverLoader
    {
        /// <summary>
        /// Return TurnoverTableRow from DB by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private static TurnoverTableRow GetTurnoverTableRow(string id)
        {
            try
            {
                var dt = new DataTable();
                var db = dbWorker.getInstance();

                dt.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_row` " +
                                         $"WHERE id='{id}'"));
                var rows = dt.AsEnumerable().ToArray();

                TurnoverTableRow row = new TurnoverTableRow();

                row.ID = rows[0][1].ToString();
                row.InputBalanceActive = Convert.ToDouble(rows[0][2].ToString());
                row.InputBalancePassive = Convert.ToDouble(rows[0][3].ToString());
                row.DebitTurnover = Convert.ToDouble(rows[0][4].ToString());
                row.LoanTurnover = Convert.ToDouble(rows[0][5].ToString());
                row.OutputBalanceActive = Convert.ToDouble(rows[0][6].ToString());
                row.OutputBalancePassive = Convert.ToDouble(rows[0][7].ToString());

                return row;
            }
            catch (Exception ex)
            {
                Trace.TraceError("Can't get TurnoverTableRow object");
                return null;
            }

        }

        /// <summary>
        /// Load data from sql table related to sheet_id
        /// Forms new TurnoverSheet object from selected data
        /// </summary>
        /// <param name="sheet_id"></param>
        /// <returns></returns>
        public static TurnoverSheet LoadSheetFromDB(int sheet_id)
        {
            var sheet = new TurnoverSheet();
            var db = dbWorker.getInstance();
            var dt = new DataTable();
            DataRow[]? rows;

            string header_id;
            string totalRow_id;

            string header_name;
            string[] header_columns;

            List<string> table_id = new List<string>();

            #region #Load Sheet
            /// Load data from sql sheet to model

            dt.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_sheet` " +
                                     $"WHERE id='{sheet_id}'"));
            rows = dt.AsEnumerable().ToArray();

            sheet.BankName = rows[0][1].ToString();
            sheet.SheetDescription = rows[0][2].ToString();
            sheet.Currency = rows[0][3].ToString();
            sheet.SheetDate = DateTime.Parse(rows[0][4].ToString());

            header_id = rows[0][5].ToString();
            sheet.TotalBySheet = GetTurnoverTableRow(rows[0][6].ToString());

            dt.Reset();

            #endregion

            #region #Load Header
            /// Load data from sql header to model

            dt.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_header` " +
                                     $"WHERE id='{header_id}'"));
            rows = dt.AsEnumerable().ToArray();

            header_name = rows[0][1].ToString();
            header_columns = new string[] { rows[0][2].ToString(), rows[0][3].ToString(), rows[0][4].ToString() };

            dt.Reset();

            dt.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_subheader` " +
                                     $"WHERE id in ('{header_columns[0]}', '{header_columns[1]}', '{header_columns[2]}')"));
            rows = dt.AsEnumerable().ToArray();

            sheet.Header = new TurnoverHeader(header_name,
                rows[0][1].ToString(), rows[0][2].ToString(), rows[0][3].ToString(),  // Turnover Sub Header 1
                rows[1][1].ToString(), rows[1][2].ToString(), rows[1][3].ToString(),  // Turnover Sub Header 2
                rows[2][1].ToString(), rows[2][2].ToString(), rows[2][3].ToString()); // Turnover Sub Header 3

            dt.Reset();

            #endregion

            #region #Load Tables
            /// Load data from sql tables to model

            sheet.Tables = new List<TurnoverTable>();

            dt.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_table` " +
                                     $"WHERE sheet_id='{sheet_id}'"));
            rows = dt.AsEnumerable().ToArray();

            rows.Each((row, _) =>
            {
                sheet.Tables.Add(new TurnoverTable
                {
                    ClassName = row[2].ToString(),
                    TotalByClass = GetTurnoverTableRow(row[3].ToString())
                });
                table_id.Add(row[0].ToString());
            });

            dt.Reset();

            #endregion

            #region #Load Groups and Rows
            /// Load data from sql groups and rows to model

            sheet.Tables.Each((table, i) =>
            {
                table.AccountGroups = new List<TurnoverTableGroup>();

                dt.Load(db.ExecuteReader("SELECT distinct group_idx FROM `B1`.`turnover_group` " +
                                         $"WHERE table_id='{table_id[i]}'"));
                rows = dt.AsEnumerable().ToArray();

                rows.Each((row, _) =>
                {
                    List<TurnoverTableRow> list_rows = new List<TurnoverTableRow>();
                    string group_idx = row[0].ToString();

                    var group_table = new DataTable();
                    group_table.Load(db.ExecuteReader("SELECT * FROM `B1`.`turnover_row` " +
                                                      "WHERE id in " +
                                                      "(SELECT row_id FROM `B1`.`turnover_group` " +
                                                      $"WHERE group_idx='{group_idx}' and table_id='{table_id[i]}')"));
                    var group_rows = group_table.AsEnumerable().ToArray();

                    group_rows.Each((r, _) =>
                    {
                        list_rows.Add(new TurnoverTableRow
                        {
                            ID = r[1].ToString(),
                            InputBalanceActive = Convert.ToDouble(r[2].ToString()),
                            InputBalancePassive = Convert.ToDouble(r[3].ToString()),
                            DebitTurnover = Convert.ToDouble(r[4].ToString()),
                            LoanTurnover = Convert.ToDouble(r[5].ToString()),
                            OutputBalanceActive = Convert.ToDouble(r[6].ToString()),
                            OutputBalancePassive = Convert.ToDouble(r[7].ToString()),
                        });
                    });

                    table.AccountGroups.Add(new TurnoverTableGroup
                    {
                        GroupID = group_idx,
                        TableRows = list_rows
                    });
                });

                dt.Reset();
            });

            #endregion

            return sheet;
        }
    }
}
