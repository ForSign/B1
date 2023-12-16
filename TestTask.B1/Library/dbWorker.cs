using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Xaml;

namespace TestTask.B1.Library
{
    internal class dbWorker
    {
        private static dbWorker _instance;
        private static readonly object syncRoot = new Object();

        private static string[] InitTables = new string[] { "" };
        private string dbPath = System.Environment.CurrentDirectory + "\\db";
        private string strConnection;

        private dbWorker() 
        {
            this.strConnection = string.Format("Data Source={0};", this.dbPath);

            InitTables[0] = "" +
                "CREATE TABLE IF NOT EXISTS Task_1 (" +
                "`id` INTEGER NOT NULL UNIQUE, " +
                "`DateTimeStamp` TEXT, " +
                "`CharsetENG` TEXT, " +
                "`CharsetRUS` TEXT, " +
                "`DecimalValue` INT, " +
                "`DoubleValue` REAL, " +
                "PRIMARY KEY(`id` AUTOINCREMENT));";

        }

        internal static dbWorker getInstance()
        {
            if (_instance == null)
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new dbWorker();
                }

            return _instance;
        }

        internal static void QueryDataToSQL()
        {
            try
            {
                using (var connection = new SqlConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    // Do whatever else you need to.
                }
            }
            catch (Exception ex)
            {
                // Handle any exception.
            }
        }
    }
}
