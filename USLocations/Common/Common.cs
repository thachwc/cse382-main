using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    class Common
    {
        static void Main(string[] args)
        {
            USLocations usLocations = new USLocations("zipcodes.tsv.txt");
            while (true)
            {
                Console.Write("commons> ");
                string states = Console.ReadLine();
                if (states.Equals("exit"))
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                string[] split = states.Split(' ');
                string state1 = split[0];
                string state2 = split[1];
                ISet<string> str = usLocations.GetCommonCityNames(state1, state2);
                foreach (string city in str)
                {
                    Console.WriteLine(city);
                    Console.WriteLine("...");
                }
            }
        }
    }
}
