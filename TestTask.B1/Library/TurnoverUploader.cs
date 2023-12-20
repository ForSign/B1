using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestTask.B1.Model;

namespace TestTask.B1.Library
{
    class TurnoverUploader
    {
        /// <summary>
        /// Create sql insert string from row
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Row insert string</returns>
        private static string getInsertRowString(TurnoverTableRow row)
        {
            return "INSERT INTO `B1`.`turnover_row` " +
                   "(idx, InputBalanceActive, InputBalancePassive, DebitTurnover," +
                   " LoanTurnover, OutputBalanceActive, OutputBalancePassive)" +
                   "values " +
                   $"('{row.ID}'," +
                   $" '{row.InputBalanceActive}'," +
                   $" '{row.InputBalancePassive}'," +
                   $" '{row.DebitTurnover}'," +
                   $" '{row.LoanTurnover}'," +
                   $" '{row.OutputBalanceActive}'," +
                   $" '{row.OutputBalancePassive}');";
        }

        /// <summary>
        /// Returns id of last imported row
        /// </summary>
        /// <returns></returns>
        private static UInt64 getLastInsertId()
        {
            UInt64 id = 0;
            var db = dbWorker.getInstance();

            var reader = db.ExecuteReader("SELECT LAST_INSERT_ID();");
            if (reader.Read())
                id = (UInt64)reader.GetValue(0);
            reader.Close();

            return id;
        }

        /// <summary>
        /// Take sheet data and put them into insert string
        /// Cause all values in db are not optional may throw error
        /// if null ends up in sheet object somehow
        /// </summary>
        /// <param name="sheet"></param>
        public static void InsertToDB(TurnoverSheet? sheet)
        {
            UInt64 turnover_header_id = UInt64.MaxValue;
            UInt64 turnover_sheet_id = UInt64.MaxValue;

            if (sheet != null)
            {
                var db = dbWorker.getInstance();
                List<string> InsertToDB = new List<string>();

                #region #Header

                InsertToDB.Add("INSERT INTO `B1`.`turnover_subheader` " +
                               "(header, column1, column2)" +
                               "values " +
                               $"('{sheet.Header.Column2.Header}'," +
                               $" '{sheet.Header.Column2.ColumnSplit1}'," +
                               $" '{sheet.Header.Column2.ColumnSplit2}');");

                InsertToDB.Add("INSERT INTO `B1`.`turnover_subheader` " +
                               "(header, column1, column2)" +
                               "values " +
                               $"('{sheet.Header.Column3.Header}'," +
                               $" '{sheet.Header.Column3.ColumnSplit1}'," +
                               $" '{sheet.Header.Column3.ColumnSplit2}');");

                InsertToDB.Add("INSERT INTO `B1`.`turnover_subheader` " +
                               "(header, column1, column2)" +
                               "values " +
                               $"('{sheet.Header.Column4.Header}'," +
                               $" '{sheet.Header.Column4.ColumnSplit1}'," +
                               $" '{sheet.Header.Column4.ColumnSplit2}');");

                InsertToDB.Add("INSERT INTO `B1`.`turnover_header` " +
                               "(column1, subheader1_id, subheader2_id, subheader3_id)" +
                               "values " +
                               $"('{sheet.Header.Column1}'," +
                               $" LAST_INSERT_ID() - 2," +
                               $" LAST_INSERT_ID() - 1," +
                               $" LAST_INSERT_ID());");

                db.ExecuteNonQuery(InsertToDB.ToArray());
                InsertToDB.Clear();

                turnover_header_id = getLastInsertId();

                #endregion

                #region #Sheet

                InsertToDB.Add(getInsertRowString(sheet.TotalBySheet));
                InsertToDB.Add("INSERT INTO `B1`.`turnover_sheet` " +
                               "(bank_name, description, currency, sheet_date, turnover_header_id, total_by_sheet_row_id)" +
                               "values " +
                               $"('{sheet.BankName}'," +
                               $" '{sheet.SheetDescription}'," +
                               $" '{sheet.Currency}'," +
                               $" '{sheet.SheetDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}'," +
                               $" {turnover_header_id}," +
                               $" LAST_INSERT_ID());");

                db.ExecuteNonQuery(InsertToDB.ToArray());
                InsertToDB.Clear();

                turnover_sheet_id = getLastInsertId();

                #endregion

                #region #Table, groups, rows

                sheet.Tables.Each((table, _) =>
                {
                    db.ExecuteNonQuery(new string[] { getInsertRowString(table.TotalByClass) });
                    db.ExecuteNonQuery(new string[] {"INSERT INTO `B1`.`turnover_table` " +
                                                     "(sheet_id, class_name, total_by_class_row_id)" +
                                                     "values " +
                                                     $"('{turnover_sheet_id}'," +
                                                     $" '{table.ClassName}'," +
                                                     $" LAST_INSERT_ID());" });

                    UInt64 table_id = getLastInsertId();
                    table.AccountGroups.Each((group, _) =>
                    {
                        group.TableRows.Each((row, _) =>
                        {
                            db.ExecuteNonQuery(new string[] { getInsertRowString(row) });
                            db.ExecuteNonQuery(new string[] {"INSERT INTO `B1`.`turnover_group` " +
                                                         "(table_id, group_idx, row_id)" +
                                                         "values " +
                                                         $"('{table_id}'," +
                                                         $" '{group.GroupID}'," +
                                                         $" LAST_INSERT_ID());" });
                        });
                    });
                });

                #endregion

            }
        }
    }
}
