using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class UserInput
    {
                
        Validation val = new();
        
        public void ReadUserKey(string input)
        {
            DatabaseCreation database = new();
            CRUDController action = new();
            Thread.Sleep(1000);
            Console.Clear();
            switch (input)
            {
                case "0":
                    break;
                case "1":
                    action.GetAllRecords(database.Name);
                    TaskComplete();
                    break;
                case "2":
                    action.Insert(database.Name);
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
                    TaskComplete();
                    break;
                case "6":
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
        internal void TaskComplete()
        {
            Thread.Sleep(1000);
            Console.WriteLine("-------------------------------");
            Console.Write("Task completed. Press ENTER to continue.");
            Console.ReadLine();
        }
        public string GetDate()
        {
            Console.Write("Insert the date: (Format dd-MM-yy): ");
            string dateInput = Console.ReadLine();

            while(!val.CheckDateInput(dateInput))
            {
                Console.Write("\n\nInvalid date. (Formate: dd-MM-yy). Try again: ");
                dateInput = Console.ReadLine();
            }           

            return dateInput;
        }
        internal int GetHour(string input)
        {
            Console.Write(input);

            string numberInput = Console.ReadLine();

            while (!val.CheckHourInput(numberInput))
            {
                Console.Write("Invalid number. Hour has to be between (0-23). Try again: ");
                numberInput = Console.ReadLine();

            }
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
        internal int GetMin(string input)
        {
            Console.Write(input);

            string numberInput = Console.ReadLine();

            while (!val.CheckMinInput(numberInput))
            {
                Console.Write("Invalid number. Minute has to be between (0-60). Try again: ");
                numberInput = Console.ReadLine();

            }
            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
        public List<int> GetUserTime()
        {
            // Set a list in which endTime Hour is less than
            // StartTime then loop if endT still < StartT.
            int[] defaultTime = { 1, 0, 0, 0 };
            List<int> timeInsert = new(defaultTime);

            //Not sure this is the optimal way to do this?
            while (timeInsert[2] < timeInsert[0])
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Time format - 24 hour");
                timeInsert.Clear();

                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("Start Time");
                    }
                    else
                    {
                        Console.WriteLine("---------------");
                        Console.WriteLine("End Time");
                    }
                    int timeInputHour = GetHour("Hour: ");
                    int timeInputMinute = GetMin("Minute: ");
                    timeInsert.Add(timeInputHour);
                    timeInsert.Add(timeInputMinute);
                }
                if (timeInsert[2] < timeInsert[0])
                {
                    Console.WriteLine("EndTime cannot be before StartTime. Press ENTER to try again.");
                    Console.ReadLine();
                    Console.Clear();
                }
                if (timeInsert[0] == timeInsert[2] && timeInsert[1] == timeInsert[3])
                {
                    Console.WriteLine("EndTime and StartTime are the same. Press ENTER to try again.");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            timeInsert.Add(val.CalculateDuration(timeInsert));
            return timeInsert;
        }        
    }
}
