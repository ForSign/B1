using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;

namespace TestTask.B1.Library
{
    internal class dbWorker
    {
        private static dbWorker _instance;
        private static readonly object syncRoot = new Object();
        private string dbScript = "static/sql/db_initial.sql";

        string connString;
        private MySqlConnection connection;

        /// <summary>
        /// Constructor for singleton
        /// Initilizes db
        /// </summary>
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

        /// <summary>
        /// Singleton instance to maintain single connection
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Run Initial scripts for db
        /// </summary>
        private void Initialize()
        {
            string script = File.ReadAllText(this.dbScript);

            if (script != null)
            {
                this.ExecuteNonQuery(new string[] {script});
                Trace.WriteLine($"DB Initialized");
            }
        }

        /// <summary>
        /// Execute sql transaction with array of commands
        /// </summary>
        /// <param name="sqliteCommands"></param>
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

        /// <summary>
        /// Execute sql reader and return its content
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <returns></returns>
        internal MySqlDataReader ExecuteReader(string sqlCommand)
        {
            var command = new MySqlCommand("", connection);
            command.CommandText = sqlCommand;
            return command.ExecuteReader();
        }
    }
}
