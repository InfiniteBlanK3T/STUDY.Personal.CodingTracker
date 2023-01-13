using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class UserInput
    {
        public void ReadUserKey(string input)
        {
            switch(input)
            {
                case "0":
                    break;
                case "1":
                    break;
                case "2":
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
