using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xaml;

namespace TestTask.B1.Library
{
    internal class dbWorker
    {
        private static dbWorker _instance;
        private static readonly object syncRoot = new Object();

        private static string[] InitTables = new string[] { "" };
        private string dbPath = System.Environment.CurrentDirectory + "\\db";
        private string dbFilePath;
        private string strConnection;

        private SQLiteConnection connection;

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

            CreateSQLiteDbFile();
            this.connection = new SQLiteConnection(strConnection);
            this.connection.Open();

            ExecuteNonQuery(InitTables);
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

        private void CreateSQLiteDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            dbFilePath = dbPath + "\\TestTask.sqlite";

            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }

        internal void ExecuteNonQuery(string[] sqliteCommands)
        {
            try
            {
                using (var command = new SQLiteCommand("", this.connection))
                {
                    foreach (var cmd in sqliteCommands)
                    {
                        command.CommandText = cmd;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
        }
    }
}
