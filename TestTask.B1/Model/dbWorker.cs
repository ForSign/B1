using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
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
            CreateSQLiteDbFile();
            this.strConnection = $"Data Source={dbFilePath};";

            InitTables[0] = "" +
                "CREATE TABLE IF NOT EXISTS Task_1 (" +
                "`id` INTEGER NOT NULL UNIQUE, " +
                "`DateTimeStamp` TEXT, " +
                "`CharsetENG` TEXT, " +
                "`CharsetRUS` TEXT, " +
                "`DecimalValue` INT, " +
                "`DoubleValue` REAL, " +
                "PRIMARY KEY(`id` AUTOINCREMENT));";

            connection = new SQLiteConnection(this.strConnection);
            connection.Open();

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

        internal SQLiteDataReader ExecuteReader(string sqlCommand)
        {
            MethodInfo dbCreateCommand = typeof(SQLiteConnection).GetMethod("CreateCommand");
            SQLiteCommand triggerCommand = (SQLiteCommand)dbCreateCommand.Invoke(connection, null);
            typeof(SQLiteCommand).GetProperty("CommandText").SetValue(triggerCommand, sqlCommand);
            MethodInfo executeReader = typeof(SQLiteCommand).GetMethod("ExecuteReader", Type.EmptyTypes);
            return (SQLiteDataReader)executeReader.Invoke(triggerCommand, null);
        }
    }
}
