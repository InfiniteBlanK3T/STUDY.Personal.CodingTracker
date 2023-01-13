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
            Console.ReadLine();
        }
    }
    public string GetDate()
    {
        Console.Write("Insert the date: (Format dd-MM-yy): ");
        string dateInput = Console.ReadLine();

        while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            Console.Write("\n\nInvalid date. (Formate: dd-MM-yy). Try again: ");
            dateInput = Console.ReadLine();
        }
        return dateInput;
    }
    public int GetNumber(string input)
    {
        Console.Write(input);

        string numberInput = Console.ReadLine();

        while(!int.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.Write("\n\nInvalid number. Try again: ");
            numberInput = Console.ReadLine();

        }
        int finalInput = Convert.ToInt32(numberInput);
        return finalInput;
    }

}