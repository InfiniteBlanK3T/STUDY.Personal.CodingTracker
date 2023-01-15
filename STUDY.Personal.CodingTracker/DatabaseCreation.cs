using Microsoft.Data.Sqlite;
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
        public DatabaseCreation() : this("Thomas") { }
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

            connection.Close();            
        }
    }
}
