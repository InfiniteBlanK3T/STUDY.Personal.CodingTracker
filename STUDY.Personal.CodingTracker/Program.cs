namespace CodingTracker;

class Program
{
    static DatabaseCreation database = new();
    static void Main()
    {
        MainMenu();
    }
    static void MainMenu()
    {
       
        CRUDController action = new();
        UserInput input = new();
        bool endApp = false;

        while (!endApp)
        {
            
            Console.Clear();
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine("\tCODING TRACKER");
            Console.WriteLine("\n-------------------------------\n");
            Console.WriteLine($"Welcome, {database.Name.Replace("_", " ")}!. Today {(DateTime.Now).ToString("dd - MM - yyyy")}");
            Console.WriteLine("\n0*. Start Tracking. . .");
            Console.WriteLine("-------------------------------");            
            Console.WriteLine("1. View Records.");
            Console.WriteLine("2. Insert Records.");
            Console.WriteLine("3. Delete Records.");
            Console.WriteLine("4. Update Records.");
            Console.WriteLine("5*. See Record Report.");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("6. CREATE YOUR OWN TRACKER !");            
            Console.WriteLine("-------------------------------\n");
            Console.WriteLine("7. Exit.\n");
            Console.Write("Your option: ");

            var userOption = Console.ReadLine();
            Console.Clear();

            switch (userOption)
            {
                case "0":
                    string date = DateTime.Now.ToString("dd-MM-yy");
                    string startTime = DateTime.Now.ToString("HH:mm");
                    string endTime = DateTime.Now.ToString("HH:mm");

                    Console.WriteLine($"Timer start now.\nToday: {DateTime.Now.ToString("dd - MM - yyyy")}.");
                    Console.WriteLine("-------------------------------\n");
                    Console.WriteLine($"Start Time: {startTime}");
                    Console.Write("\nPress SPACEBAR to stop the Timer");
                    if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                    {
                        Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                        Console.WriteLine($"Stop Time: {endTime}");
                        Console.WriteLine("-------------------------------");
                        List<int> timerList = input.GetConcurrentTimeList(startTime, endTime);
                        action.Insert(database.Name, date, timerList);
                        Console.WriteLine("Timer added to the record.");
                    }
                    TaskComplete();
                    Console.ReadLine();                    
                    break;
                case "1":
                    action.GetAllRecords(database.Name);
                    TaskComplete();
                    break;
                case "2":
                    input.GetRecordFromUser(database.Name);
                    TaskComplete();
                    break;
                case "3":
                    action.Delete(database.Name);
                    TaskComplete();
                    break;
                case "4":
                    action.Update(database.Name);
                    TaskComplete();
                    break;
                case "5":
                    ReportTable(database.Name);
                    TaskComplete();
                    break;
                case "6":
                    database.Name = database.CreateNewRecord();                    
                    TaskComplete();
                    break;
                case "7":
                    Console.WriteLine("\nGoodbye!\n");
                    Environment.Exit(0);
                    break;
                default:
                    Console.Write("\nInvalid input! Please try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }
    internal static void TaskComplete()
    {
        Thread.Sleep(500);
        Console.WriteLine("-------------------------------");
        Console.Write("Task completed. Press ENTER to continue.");
        Console.ReadLine();
        MainMenu();
    }
    static void ReportTable(string table)
    {
        CRUDController action = new();

        bool reportable = action.Report(table);
        bool endReport = false;

        while (reportable && !endReport)
        {
            action.GetAllRecords(table);
            Console.WriteLine("-------------------------------");
            Console.WriteLine($"\tREPORT OPTION\r");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("1. Filter Record");
            Console.WriteLine("2. Report of the Record");
            Console.WriteLine("2. Set your own goals");
            Console.WriteLine("3. Return to Menu");
            Console.WriteLine("-------------------------------");
            Console.Write("Your option: ");
            var input = Console.ReadLine();

            switch(input)
            {
                case "1":
                    ReportTableOption1(table);
                    break;
                case "2":
                    Console.WriteLine("");
                    break;
                case "3":
                    endReport = true;
                    break;
                default:
                    Console.Write("\nInvalid input! Please try again.");
                    Console.ReadLine();
                    ReportTable(table);
                    break;
            }

            return;
        }        
    }
    static void ReportTableOption1(string table)
    {
        CRUDController action = new();

        Console.WriteLine("-------------------------------");
        Console.WriteLine("Period of time");
        Console.WriteLine("------------------");
        Console.WriteLine("1. This Month");
        Console.WriteLine("2. This Year");
        Console.WriteLine("3. Go back to Report Menu");
        Console.WriteLine("-------------------------------");
        Console.Write("Your option: ");
        var input = Console.ReadLine();       

        switch(input)
        {
            
            case "1":                
                action.ReportMontly(table);
                Console.ReadLine();
                break;
            case "2":
                break;
            case "3":
                break;
            case"4":
                break;
            default:
                break;
        }
    }
}