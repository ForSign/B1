using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private string dbScript = "static/sql/db_initial.sql";

        string connString;
        private MySqlConnection connection;

        private dbWorker() 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings/config.json", optional: false, reloadOnChange: true);
            IConfiguration _configuration = builder.Build();
            connString = _configuration.GetConnectionString("Default");

            connection = new MySqlConnection(connString);
            connection.Open();

            this.Initialize();
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

        private void Initialize()
        {
            string script = File.ReadAllText(this.dbScript);

            if (script != null)
            {
                this.ExecuteNonQuery(new string[] {script});
                Trace.WriteLine($"DB Initialized");
            }
        }

        internal void ExecuteNonQuery(string[] sqliteCommands)
        {
            try
            {
                //using (var conn = new MySqlConnection(connString))
                using (var command = new MySqlCommand("", this.connection))
                {
                    //conn.Open();
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

        internal MySqlDataReader ExecuteReader(string sqlCommand)
        {
            var command = new MySqlCommand("", connection);
            command.CommandText = sqlCommand;
            return command.ExecuteReader();
        }
    }
}
