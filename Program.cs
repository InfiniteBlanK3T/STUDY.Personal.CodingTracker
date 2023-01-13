using System.Configuration;
using System.Collections.Specialized;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {

        MainMenu();
    }
    static void MainMenu()
    {
        bool endApp = false;
        UserInput userInput = new();

        while (!endApp)
        {
            Console.Clear();
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine("\tCODING TRACKER");
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine("Welcome, ($NAME)!. Today ($DATE). Coding time: $TIME");
            Console.WriteLine("\nPress 0 to Start Tracking. . .\n");
            Console.WriteLine("-------------------------------");            
            Console.WriteLine("Press 1 to View Records.");
            Console.WriteLine("Press 2 to Insert Records.");
            Console.WriteLine("Press 3 to Delete Records.");
            Console.WriteLine("Press 4 to Update Records.");
            Console.WriteLine("Press 5 to See Record Report.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Press 6 to CREATE YOUR OWN TRACKER !");            
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("Press 7 to exit.\n");
            Console.Write("Your option: ");

            userInput.ReadUserKey(Console.ReadLine());
            

        }
    }
}