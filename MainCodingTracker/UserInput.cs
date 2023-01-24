﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using static System.Collections.Specialized.BitVector32;

namespace CodingTracker
{
    public class UserInput
    {
                
        Validation val = new();
        
        public void GetRecordFromUser(string table)
        {
            CRUDController action = new();
            string dateInsert = GetDate();
            List<int> timeInsert = GetUserTime();
            action.Insert(table,dateInsert, timeInsert);
        }

        public List<int> GetConcurrentTimeList(string start, string end)
        {            
            var startT = start.Split(":").Select(Int32.Parse).ToList();
            var endT = end.Split(":").Select(Int32.Parse).ToList();
            List<int> timeList = startT.Concat(endT).ToList();
            timeList.Add(val.CalculateDuration(timeList));

            return timeList;
        }

        public string GetDate()
        {
            Console.Write("Insert the date (Format dd-MM-yy): ");
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

        public void StartingTimerCount(string table)
        {
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
                List<int> timerList = GetConcurrentTimeList(startTime, endTime);
                CRUDController action = new();
                action.Insert(table, date, timerList);
                Console.WriteLine("Timer added to the record.");                
            }
        }

        internal bool ErrorPrompt()
        {
            Console.Write("Invalid input. Press ENTER to try again or Type 0 to quit.");
            var quit = Console.ReadLine();
            if (quit == "0") 
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("User quits."); 
                return true; 
            }
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Thread.Sleep(1000);
            return false;
        }

        public bool GetReportFilter(string table)
        {                      
            var inputMonth = val.CheckMonthInputReport("Choose a month (01-12) or Press ENTER for CURRENT month: ");           

            if (inputMonth == null)
            {
                var error = ErrorPrompt();
                if (error) return true;
                GetReportFilter(table);
            }

            var sortOption = ReportSorting("Sort by: Ascending(a)/Descending(d). (a/d)? ");

            // How can I make this not repetitive?
            if (sortOption == null)
            {
                var error = ErrorPrompt();
                if (error) return true;
                GetReportFilter(table);
            }
            Console.WriteLine("Done");

            return true;
        }

        public string ReportSorting(string prompt)
        {
            Console.Write(prompt);

            var option = Console.ReadLine();

            switch(option)
            {
                case "a":
                    return "ASC";                    
                case "d":
                    return "DESC";                    
                default:
                    return null;
            }
        }

    }
}