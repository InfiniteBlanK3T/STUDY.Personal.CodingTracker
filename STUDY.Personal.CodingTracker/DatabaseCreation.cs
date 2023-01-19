﻿using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker
{    
    public class DatabaseCreation
    {
        Validation val = new();
        string connectionString = ConfigurationManager.AppSettings.Get("databaseSource");
        private string _name;
        public DatabaseCreation(string name) 
        {
            _name = name;
            CheckDatabaseExist(name);
        }
        public DatabaseCreation() : this("Thomas_Default") { }
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public void CheckDatabaseExist(string name)
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();

            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS {name} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER
                    );";
            val.QueryHandling(tableCmd);         
        }
        public string CreateNewRecord(string tableName)
        {
            Console.Clear();
            Console.Write("Your name for the record: ");

            string? newRecordName = Console.ReadLine();
            newRecordName = newRecordName.Replace(" ", "_");

            while (newRecordName == null || newRecordName == "")
            {
                Console.Write("Invalid name please try again: ");
                newRecordName = Console.ReadLine();
            }

            DatabaseCreation _ = new(tableName);
            Console.WriteLine($"New record <<{tableName}>> created!");
            Thread.Sleep(1000);

            return tableName;
        }
    }
}
