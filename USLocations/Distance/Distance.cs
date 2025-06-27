using System;
using System.Collections.Generic;
using System.Text;

namespace Distance
{
    class Distance
    {
        static void Main(string[] args)
        {
            USLocations  usLocations= new USLocations("zipcodes.tsv.txt");
            while (true)
            {
                Console.Write("distance> ");
                string zipcodes = Console.ReadLine();
                if (zipcodes.Equals("exit"))
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }
                string[] split = zipcodes.Split(' ');
                int zip1 = Int32.Parse(split[0]);
                int zip2 = Int32.Parse(split[1]);
                double km = usLocations.Distance(zip1, zip2);
                double miles = km / 1.609;
                Console.WriteLine("The distance between " + zip1 + " and " + zip2 + " is " + String.Format("{0:0.00}", miles) + " miles (" + String.Format("{0:0.00}", km) + " km)");
            }
        }
    }
}
