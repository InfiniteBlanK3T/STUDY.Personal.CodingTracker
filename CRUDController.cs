using System;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Configuration;

namespace CodingTracker;
public class CRUDController
{
	string connectionString = ConfigurationManager.AppSettings.Get("databaseSource");
	public CRUDController() { }

	public void GetAllRecords(string record)
	{
		using var conn = new SqliteConnection(connectionString);

		conn.Open();

		var tableCmd = conn.CreateCommand();

		tableCmd.CommandText = $"SELECT * FROM {record}";

		List<CodingSession> tableData = new();

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
						Duration = reader.GetInt32(2)
					}
					); ;
			}
		}
		else
		{
			Console.WriteLine("No rows found");
		}
		conn.Close();

		DataVisualisation table = new();

		table.ShowingTable(tableData);
	}
	public void Insert(string record)
	{
		Validation checkInput = new Validation();
        string dateInsert = checkInput.GetDate();
		List<int> timeInsert = new();
		
		//Not sure this is the optimal way to do this?
		for (int i = 0; i < 2; i++)
		{
			if (i == 0) {
				Console.WriteLine("Start Time");
			}
			else
			{
                Console.WriteLine("---------------");
                Console.WriteLine("End Time");
			}
            int timeInputHour = checkInput.GetNumber("Hour: ");
            int timeInputMinute = checkInput.GetNumber("Minute: ");
        }

		Console.ReadLine();
    }
}
