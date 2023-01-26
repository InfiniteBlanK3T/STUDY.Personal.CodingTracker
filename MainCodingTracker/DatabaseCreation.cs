﻿using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker
{    
    public class DatabaseCreation
    {
        Validation val = new();
        private string _name;
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
        readonly string path = ConfigurationManager.AppSettings.Get("Path");

        public DatabaseCreation(string name) 
        {
            _name = name;                        
            AppDomain.CurrentDomain.SetData("DataDirectory", path);            
            CheckDatabaseExist();            
        }

        public DatabaseCreation() : this("Thomas_Default") { }
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        public void CheckDatabaseExist()
        {
            using var connection = new SqliteConnection(connectionString);
            
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                    @$"CREATE TABLE IF NOT EXISTS {Name} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER
                    );";
            val.QueryHandling(tableCmd);         
        }
        public string CreateNewRecord()
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

            DatabaseCreation _ = new(newRecordName);
            Console.WriteLine($"New record <<{newRecordName}>> created!");
            Thread.Sleep(1000);

            return newRecordName;
        }
    }
}
