using System;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Configuration;

namespace CodingTracker;
public class CRUDController
{
    string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    string path = ConfigurationManager.AppSettings.Get("Path");
    UserInput userInput = new();
	Validation val = new(); 

    public void GetAllRecords(string record)
	{
        if (CheckEmptyTable(record)) return;        

        using var conn = new SqliteConnection(connectionString);

		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"SELECT * FROM {record}";	

		FromQueryToTable(tableCmd, record);

	}

	public void Insert(string record, string dateInsert, List<int> timeInsert)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();
		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText =
			$@"INSERT INTO {record} (Date, StartTime, EndTime, Duration) 
			VALUES ('{dateInsert}', '{timeInsert[0]}:{timeInsert[1].ToString("D2")}', 
			'{timeInsert[2].ToString("D2")}:{timeInsert[3].ToString("D2")}', {timeInsert[4]})";
		val.QueryHandling(tableCmd);
    }

	public void Delete(string record)
	{
        if (CheckEmptyTable(record)) return;

        GetAllRecords(record);
        var recordId = val.GetNumber("Please type the Id of record you want to delete or Type 0 to back to the Menu: ");
		if (recordId == 0) return;        

        using var conn = new SqliteConnection(connectionString);
		conn.Open();

		var tableCmd = conn.CreateCommand();
		tableCmd.CommandText = $"DELETE FROM {record} WHERE Id = '{recordId}'";
		int rowCount = tableCmd.ExecuteNonQuery();

		if (rowCount == 0)
		{
			Console.WriteLine($"\n\nRecord with Id <<{recordId}>> does not exist. Press ENTER to try again.\n\n");
			Console.ReadLine();
			Console.Clear();
			Delete(record);
			return;
		}

		Thread.Sleep(1000);
        GetAllRecords(record);
        Console.WriteLine($"\n\nRecord with Id <<{recordId}>> was deleted.\n\n");
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
    }

	internal bool CheckEmptyTable(string table)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();
		
		var checkCmd = conn.CreateCommand();		
		checkCmd.CommandText = $"SELECT COUNT(*) FROM {table}";
		int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

		if (checkQuery == 0)
		{
			Console.WriteLine("No record in this table.");
			return true;
		}

		return false;
	}

    public bool Report(string table)
    {
        Console.Clear();

		using var connection = new SqliteConnection(connectionString);
        
        connection.Open();

        if (CheckEmptyTable(table)) return false;

        var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"SELECT COUNT(*) FROM {table}";
        int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

        if (checkQuery < 3)
        {
            Console.WriteLine($"\n\nInsert at least {3 - checkQuery} more entries for the report !\n\n");

            connection.Close();
			return false;
        }

		return true;        
    }

	public void ReportMontly(string table)
	{
        Console.Clear();
        using var conn = new SqliteConnection(connectionString);
		conn.Open();

		var currentMonth = DateTime.Now.Month.ToString("MM");
		var currentYear = DateTime.Now.Year.ToString("yyyy");
        var queryRecord = conn.CreateCommand();
		queryRecord.CommandText = $"SELECT * FROM {table} WHERE DATE = '22-01-23'";

		FromQueryToTable(queryRecord, table);
	}

    public void FromQueryToTable(SqliteCommand query, string table)
    {
        DataVisualisation makingTable = new();
        List<CodingSession> tableData = new();

        SqliteDataReader reader = query.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                tableData.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("end-US")),
                        StartTime = reader.GetString(2),
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

        makingTable.ShowingTable(tableData, table);
    }
}
