using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class UserInput
    {
        DatabaseCreation database = new();
        CRUDController action = new();
        public void ReadUserKey(string input)
        {
            Console.WriteLine("\n-------------------------------\n");
            switch (input)
            {
                case "0":
                    break;
                case "1":
                    action.GetAllRecords(database.Name);
                    break;
                case "2":
                    action.Insert(database.Name);
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                case "6":
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
}
