using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker;

public class Validation
{
    public void QueryHandling(SqliteCommand e)
    {
        try
        {
            e.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);           
        }
    }
    public bool CheckDateInput(string date)
    {
        if(!DateTime.TryParseExact(date, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }
    public int GetNumber(string input)
    {
        Console.Write(input);

        string numberInput = Console.ReadLine();

        while (!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.Write("\n\nInvalid number. Try again: ");
            numberInput = Console.ReadLine();

        }
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }
    public bool CheckHourInput(string input)
    {
        if(!int.TryParse(input, out _) || Convert.ToInt32(input) < 0 || Convert.ToInt32(input) > 23)
        {
            return false;
        }       
        return true;
    }
    public bool CheckMinInput(string input)
    {
        if (!int.TryParse(input, out _) || Convert.ToInt32(input) < 0 || Convert.ToInt32(input) > 59)
        {
            return false;
        }
        return true;
    }
    public int CalculateDuration(List<int> list)
    {
        int minute;
        int hour   = list[2] - list[0];
        if (list[3] < list[1])
        {
            minute = 60 - list[1] + list[3];
        }
        minute = list[3] - list[1];
       
        int totalDuration = (hour * 60) + minute;

        return totalDuration;
    }

}