using System;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Configuration;

namespace CodingTracker;
public class CRUDController
{
	readonly string connectionString = ConfigurationManager.AppSettings.Get("databaseSource");
	UserInput userInput = new();
	Validation val = new();    
    public void GetAllRecords(string record)
	{
        DataVisualisation table = new();
        List<CodingSession> tableData = new();
        using var conn = new SqliteConnection(connectionString);

		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"SELECT * FROM {record}";	

		SqliteDataReader reader = tableCmd.ExecuteReader();

		if (reader.HasRows)
		{
			while (reader.Read())
			{
				tableData.Add(
					new CodingSession
					{
						Id = reader.GetInt32(0),
						Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("end-US")),
						StartTime= reader.GetString(2),
						EndTime = reader.GetString(3),
						Duration = reader.GetInt32(4)
					}
					); ;
			}
		}
		else
		{
			Console.WriteLine("No rows found");
		}
		conn.Close();
        
        table.ShowingTable(tableData);
	}
	public void Insert(string record)
	{		
        string dateInsert = userInput.GetDate();		
		List<int> timeInsert = userInput.GetUserTime();

		using var conn = new SqliteConnection(connectionString);

		conn.Open();
		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"INSERT INTO {record} (Date, StartTime, EndTime, Duration) 
			VALUES ('{dateInsert}', '{timeInsert[0]}:{timeInsert[1]}', 
			'{timeInsert[2]}:{timeInsert[3]}', {timeInsert[4]})";
		val.QueryHandling(tableCmd);

		Console.ReadLine();
    }
}
