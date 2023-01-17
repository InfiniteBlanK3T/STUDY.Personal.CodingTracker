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
        if (CheckEmptyTable(record)) return;
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
        
        table.ShowingTable(tableData, record);
	}
	public void Insert(string record, string dateInsert, List<int> timeInsert)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();
		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"INSERT INTO {record} (Date, StartTime, EndTime, Duration) 
			VALUES ('{dateInsert}', '{timeInsert[0]}:{timeInsert[1]}', 
			'{timeInsert[2]}:{timeInsert[3]}', {timeInsert[4]})";
		val.QueryHandling(tableCmd);
    }
	public void Delete(string record)
	{
        var recordId = val.GetNumber("Please type the Id of record you want to delete or Type 0 to back to the Menu: ");
		if (recordId == 0) return;

		using var conn = new SqliteConnection(connectionString);
		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"DELETE FROM {record} WHERE Id = '{recordId}'";
		int rowCount = tableCmd.ExecuteNonQuery();

		if (rowCount == 0)
		{
			Console.WriteLine($"\n\nRecord with Id {recordId} does not exist. Press ENTER to try again.\n\n");
			Console.ReadLine();
			Console.Clear();
			Delete(record);
			return;
		}
		Thread.Sleep(1000);
        GetAllRecords(record);
        Console.WriteLine($"\n\nRecord with Id {recordId} was deleted.\n\n");

		conn.Close();
	}
	public void Update(string record)
	{
        if (CheckEmptyTable(record)) return;
		GetAllRecords(record);
        var recordId = val.GetNumber("Please type the Id of record you want to Update or Type 0 to back to the Menu: ");
        if (recordId == 0) return;

        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM {record} WHERE Id = '{recordId}')";
        int rowCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (rowCount == 0)
        {
            Console.Write($"\n\nRecord with Id {recordId} does not exist. Press ENTER to try again.\n\n");
            Console.ReadLine();
            Console.Clear();
            Update(record);
            return;
        }
		string date = userInput.GetDate();
        List<int> timeInsert = userInput.GetUserTime();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"UPDATE {record} SET date = '{date}', 
			StartTime = '{timeInsert[0]}:{timeInsert[1]}',
			EndTime = '{timeInsert[2]}:{timeInsert[3]}',
			Duration = '{timeInsert[4]}'";
		val.QueryHandling(tableCmd);
		Thread.Sleep(1000);		
		GetAllRecords(record);
        Console.WriteLine("\n\n Update Completed!");
        conn.Close();		
    }
	internal bool CheckEmptyTable(string record)
	{
		using var conn = new SqliteConnection(connectionString);
		conn.Open();
		
		var checkCmd = conn.CreateCommand();
		
		checkCmd.CommandText = $"SELECT COUNT(*) FROM {record}";
		int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

		conn.Close();

		if (checkQuery == 0)
		{
			Console.WriteLine("No record in this table.");
			return true;
		}
		return false;
	}
}
